<?php

class GamarGroupmanager extends DatabaseObject
{
	public function __construct()
	{
		parent::__construct("groupmanager");
	}

	public function login()
	{
		if (!$this->isReady()) return;
		$_SESSION['manager'] = $this->record;
	}
	
	public function initFromSession()
	{
		return isset($_SESSION['manager']) && $this->initById($_SESSION['manager']['id']);
	}
	
	public function initByLogin($email, $pass)
	{
		return $this->initByFields(array("email", "password"), array($email, encodePassword($pass)));
	}
	
	public function initByEmail($email)
	{
		return $this->initByField("email", $email);
	}
	
	############################################################################
	# GETTER
	public function getGroups($loadUsers = false)
	{
		$groups = array();
		$r1 = mysql_query("SELECT * FROM usergroup WHERE groupmanager_id =".$this->getId()." ORDER BY name");
		while($g = mysql_fetch_assoc($r1))
		{
			$group = new GamarUsergroup();
			if ($group->initFromRecord($g))
			{
				if ($loadUsers) $group->getUsers();
				$groups[] = $group;
			}
		}
		
		return $groups;
	}
	
	public function getName()
	{
		return $this->get("name");
	}
	
	public function getEmail()
	{
		return $this->get("email");
	}

	public function getPassword()
	{
		return $this->get("password");
	}

	############################################################################
	# SETTER
	public function setName($value)
	{
		$this->set("name", $value);
	}
	
	public function setEmail($value)
	{
		$this->set("email", $value);
	}

	public function setPassword($value)
	{
		$this->set("password", encodePassword($value)); // md5 for now
	}

}

?>