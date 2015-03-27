<?php

class GamarActivity extends DatabaseObject
{
	public function __construct()
	{
		parent::__construct("activity");
		$this->setTime(date("Y-m-d H:i:s"));
	}

	############################################################################
	# GETTER
	public function getUserId()
	{
		return $this->get("user_id");
	}

	public function getGameId()
	{
		return $this->get("game_id");
	}

	public function getTime()
	{
		return $this->get("time");
	}

	public function getMessage()
	{
		return $this->get("message");
	}

	############################################################################
	# SETTER
	public function setUserId($value)
	{
		$this->set("user_id", $value);
	}

	public function setGameId($value)
	{
		$this->set("game_id", $value);
	}

	public function setTime($value)
	{
		$this->set("time", $value);
	}

	public function setMessage($value)
	{
		$this->set("message", $value);
	}

}

?>