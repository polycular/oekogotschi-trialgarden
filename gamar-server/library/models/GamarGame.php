<?php

class GamarGame extends DatabaseObject
{
	public static function GetAll($metagameId)
	{
		if (!is_numeric($metagameId)) return array();
	
		$all = array();
		$r = mysql_query("SELECT * FROM game WHERE metagame_id = $metagameId ORDER BY metagame_id, location_id");
		while($d = mysql_fetch_assoc($r))
		{
			$game = new GamarGame();
			$game->initFromRecord($d);
			$all[] = $game;
		}
		
		return $all;
	}

	public function __construct()
	{
		parent::__construct("game");
	}

	############################################################################
	# GETTER
	public function getMetagameId()
	{
		return $this->get("metagame_id");
	}

	public function getLocationId()
	{
		return $this->get("location_id");
	}

	public function getName()
	{
		return $this->get("name");
	}

	public function getTries()
	{
		return $this->get("tries");
	}

	public function getAssetsUrl()
	{
		return $this->get("assets_url");
	}

	############################################################################
	# SETTER
	public function setMetagameId($value)
	{
		$this->set("metagame_id", $value);
	}

	public function setLocationId($value)
	{
		$this->set("location_id", $value);
	}

	public function setName($value)
	{
		$this->set("name", $value);
	}

	public function setTries($value)
	{
		$this->set("tries", $value);
	}

	public function setAssetsUrl($value)
	{
		$this->set("assets_url", $value);
	}

}

?>