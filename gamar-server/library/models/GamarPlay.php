<?php

class GamarPlay extends DatabaseObject
{
	public function __construct()
	{
		parent::__construct("play");
	}

	############################################################################
	# GETTER
	public function getGameId()
	{
		return $this->get("game_id");
	}

	public function getUserId()
	{
		return $this->get("user_id");
	}

	public function getStart()
	{
		return $this->get("start");
	}

	public function getEnd()
	{
		return $this->get("end");
	}

	public function getFinished()
	{
		return $this->get("finished");
	}

	############################################################################
	# SETTER
	public function setGameId($value)
	{
		$this->set("game_id", $value);
	}

	public function setUserId($value)
	{
		$this->set("user_id", $value);
	}

	public function setStart($value)
	{
		$this->set("start", $value);
	}

	public function setEnd($value)
	{
		$this->set("end", $value);
	}

	public function setFinished($value)
	{
		$this->set("finished", $value);
	}

}

?>