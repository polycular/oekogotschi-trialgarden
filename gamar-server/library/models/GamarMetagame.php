<?php

class GamarMetagame extends DatabaseObject
{
	public function __construct()
	{
		parent::__construct("metagame");
	}

	############################################################################
	# GETTER
	public function getName()
	{
		return $this->get("name");
	}

	############################################################################
	# SETTER
	public function setName($value)
	{
		$this->set("name", $value);
	}

}

?>