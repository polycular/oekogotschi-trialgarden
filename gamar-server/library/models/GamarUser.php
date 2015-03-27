<?php

class GamarUser extends DatabaseObject
{
	private $usergroup = null; // cached instance of usergroup

	public function __construct()
	{
		parent::__construct("user");
	}

	public function login()
	{
		if (!$this->isReady()) return;
		$_SESSION['user'] = $this->record;
		
		$this->setLastSeen(date("Y-m-d H:i:s"));
		$this->save();
	}
	
	/*
	public function getGameFromCheckin()
	{
		if (!isset($_SESSION['user']['checkin'])) return null;
		
		$game = new GamarGame();
		if (!$game->initById((int)$_SESSION['user']['checkin'])) return null;
		return $game;
	}
	*/
	
	public function checkinGame($game)
	{
		$userId = (int)$_SESSION['user']['id'];
		
		$play = new GamarPlay();
		$play->setGameId($game->getId());
		$play->setUserId($userId);
		$play->setStart(date("Y-m-d H:i:s"));
		$play->save();
		
		$_SESSION['user']['checkin'] = $play->getId();
	}
	
	public function checkoutGame($scoreValue = -1)
	{
		if (!isset($_SESSION['user']['checkin'])) return false;
	
		$userId = (int)$_SESSION['user']['id'];

		$play = new GamarPlay();
		if ($play->initById($_SESSION['user']['checkin']))
		{
			if ($scoreValue >= 0) 
			{
				$play->setFinished("1");
			
				$score = new GamarScore();
				$score->setUserId($userId);
				$score->setGameId($play->getGameId());
				$score->setTime(date("Y-m-d H:i:s"));
				$score->setValue($scoreValue);
				$score->save();
			}

			$play->setEnd(date("Y-m-d H:i:s"));
			$play->save();
		}
		
		unset($_SESSION['user']['checkin']);
		return true;
	}
	
	public function initFromSession()
	{
		return isset($_SESSION['user']) && $this->initById($_SESSION['user']['id']);
	}
	
	/*
	public function initByLogin($email, $pass)
	{
		return $this->initByFields(array("email", "password"), array($email, encodePassword($pass)));
	}
	*/

	public function initByLogin($name, $deviceId)
	{
		return $this->initByFields(array("name", "device_id"), array($name, $deviceId));
	}
	
	public function initByName($name)
	{
		return $this->initByField("name", $name);
	}
	
	public function initByEmail($email)
	{
		return $this->initByField("email", $email);
	}	
	
	############################################################################
	# GETTER
	public function getUsergroupId()
	{
		return $this->get("usergroup_id");
	}
	
	public function getUsergroup()
	{
		if (is_null($this->usergroup))
		{
			$this->usergroup = new GamarUsergroup();
			if (!$this->usergroup->initById($this->getUsergroupId())) $this->usergroup = null;
		}
		
		return $this->usergroup;
	}

	public function getDeviceId()
	{
		return $this->get("device_id");
	}

	public function getEmail()
	{
		return $this->get("email");
	}

	public function getPassword()
	{
		return $this->get("password");
	}

	public function getName()
	{
		return $this->get("name");
	}

	public function getCharacter()
	{
		return $this->get("character");
	}

	public function getLastSeen()
	{
		return $this->get("last_seen");
	}

	############################################################################
	# SETTER
	public function setUsergroupId($value)
	{
		$this->set("usergroup_id", $value);
	}

	public function setDeviceId($value)
	{
		$this->set("device_id", $value);
	}

	public function setEmail($value)
	{
		$this->set("email", $value);
	}

	public function setPassword($value)
	{
		$this->set("password", encodePassword($value));
	}

	public function setName($value)
	{
		$this->set("name", $value);
	}

	public function setCharacter($value)
	{
		$this->set("character", $value);
	}

	public function setLastSeen($value)
	{
		$this->set("last_seen", $value);
	}

}

?>