<?php

class GamarUsergroup extends DatabaseObject
{
	private $users = null;

	public function __construct()
	{
		parent::__construct("usergroup");
	}
	
	public function initByHash($hash)
	{
		return $this->initByField("hash", $hash);
	}	
	
	public function initByName($name)
	{
		return $this->initByField("name", $name);
	}	
	
	############################################################################
	# GETTER
	public function getUsers() // cached
	{
		if (is_null($this->users))
		{
			$this->users = array();
			$r = mysql_query("SELECT * FROM user WHERE usergroup_id=".$this->getId()." ORDER BY name");
			while($u = mysql_fetch_assoc($r))
			{
				$user = new GamarUser();
				if ($user->initFromRecord($u)) $this->users[] = $user;
			}
		}
	
		return $this->users;
	}
	
	public function getGroupmanagerId()
	{
		return $this->get("groupmanager_id");
	}
	
	public function getMetagameId()
	{
		return $this->get("metagame_id");
	}
	
	public function getName()
	{
		return $this->get("name");
	}

	public function getHash()
	{
		return $this->get("hash");
	}

	public function getUserlimit()
	{
		return $this->get("userlimit");
	}

	############################################################################
	# SETTER
	public function setGroupmanagerId($value)
	{
		$this->set("groupmanager_id", $value);
	}

	public function setMetagameId($value)
	{
		$this->set("metagame_id", $value);
	}

	public function setName($value)
	{
		$this->set("name", $value);
	}

	public function setHash($value)
	{
		$this->set("hash", $value);
	}

	public function setUserlimit($value)
	{
		$this->set("userlimit", $value);
	}

}

?>