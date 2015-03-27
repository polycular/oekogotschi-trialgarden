<?php

class DatabaseObject
{
	protected $ready = false;
	protected $errors = array();
	protected $tableName;
	protected $record;
	
	function __construct($tableName)
	{
		$this->tableName = $tableName;
		$this->record = array();
	}
	
	public function initById($id)
	{
		$r = mysql_query("SELECT * FROM $this->tableName WHERE id=$id");
		
		if ($record = mysql_fetch_assoc($r))
		{
			return $this->initFromRecord($record);
		}
		
		return false;
	}
	
	public function initByField($name, $value)
	{
		$value = mysql_real_escape_string($value);
		$r = mysql_query("SELECT * FROM $this->tableName WHERE `$name`='$value'");
		
		if ($record = mysql_fetch_assoc($r))
		{
			return $this->initFromRecord($record);
		}
		
		return false;
	}
	
	public function initByFields($names, $values)
	{
		if (count($names) != count($values)) return false;
		
		$where = array();
		for($i = 0; $i < count($names); $i++) $where[] = "`$names[$i]`='".mysql_real_escape_string($values[$i])."'";		
		$r = mysql_query("SELECT * FROM $this->tableName WHERE ".implode(" AND ", $where));
		
		if (mysql_num_rows($r) != 1) return false;
		
		if ($record = mysql_fetch_assoc($r))
		{
			return $this->initFromRecord($record);
		}
		
		return false;
	}
	
	public function initFromRecord($record)
	{
		$this->record = $record;
		$this->ready = true;
		return true;
	}
	
	public function isReady()
	{
		return $this->ready;
	}
	
	public function set($key, $value)
	{
		$this->record[$key] = $value;
	}
	
	public function get($key)
	{
		if (array_key_exists($key, $this->record)) return $this->record[$key];
		return "";
	}
	
	public function getId()
	{
		if (array_key_exists("id", $this->record)) return (int)$this->record["id"];
		return 0;
	}
	
	public function save()
	{
		$success = false;
		$id = $this->getId();
		
		if ($id <= 0) 
		{
			# insert new record
			$keys = array();
			$values = array();
			
			foreach($this->record as $key => $value)
			{
				if ($key == "id") continue;
				$keys[] = $key;
				$values[] = "'".mysql_real_escape_string($value)."'";
			}
			
			$q = "INSERT INTO $this->tableName (`".implode("`,`", $keys)."`) VALUES (".implode(",", $values).")";
			if (mysql_query($q))
			{
				$this->record["id"] = mysql_insert_id();
				$success = true;
			}
			else $this->error(mysql_error());
		}
		else
		{
			# update existing record
			$assign = array();
			
			foreach($this->record as $key => $value)
			{
				if ($key == "id") continue;
				$assign[] = "`".$key."`='".mysql_real_escape_string($value)."'";
			}
			
			$q = "UPDATE $this->tableName SET ".implode(",",$assign)." WHERE id=".$id;
			if (mysql_query($q)) $success = true;
			else $this->error(mysql_error());			
		}
		
		if ($success) $this->ready = true;
		return $success;
	}
	
	# logs
	protected function error($log)
	{
		error_log($log);
		$this->errors[] = $log;
	}
	
	public function getErrors()
	{
		return $this->errors;
	}
}

?>