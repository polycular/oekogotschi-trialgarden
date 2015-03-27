<?php

# this controller handles all requests that do not need a valid login

class Home extends Controller
{
	function __construct()
	{
		parent::__construct();
	}
	
	/*
	public function register($code, $name, $char, $email, $password, $device)
	{
		$error = 0;
		$message = "";
		
		if (validateEmail($email))
		{
			if (strlen($password) > 3)
			{
				$group = new GamarUsergroup();
				if (!empty($code) && $group->initByHash($code))
				{
					$user = new GamarUser();
					if (!$user->initByEmail($email))
					{
						$user->setUsergroupId($group->getId());
						$user->setName($name);
						$user->setCharacter($char);
						$user->setEmail($email);
						$user->setPassword($password);
						$user->setDeviceId($device);
						if ($user->save()) 
						{
							$this->logActivity("registered", $user->getId());
							$user->login();
						}
						else 
						{
							$error = 11;
							$message = "failed to save";
						}
					}
					else 
					{
						$error = 10;
						$message = "email exists";
					}
				}
				else 
				{
					$error = 9;
					$message = "group not found";
				}
			} 
			else 
			{
				$error = 7;
				$message = "invalid password";
			}
		}
		else 
		{
			$error = 6;
			$message = "invalid email address";
		}
	
		if ($error == 0) $response = array("success" => true, "session" => session_id());
		else $response = array("success" => false, "error" => $error, "message" => $message);
		$this->output($response);
	}
	*/
	
	public function register($code, $name, $char, $email, $password, $device)
	{
		$error = 0;
		$message = "";
		
		$group = new GamarUsergroup();
		if (!empty($code) && $group->initByHash($code))
		{
			$user = new GamarUser();
			if (!$user->initByName($name))
			{
				$user->setUsergroupId($group->getId());
				$user->setName($name);
				$user->setCharacter($char);
				$user->setEmail($email);
				if (!empty($password)) $user->setPassword($password);
				$user->setDeviceId($device);
				
				if ($user->save()) 
				{
					$this->logActivity("registered", $user->getId());
					$user->login();
				}
				else 
				{
					$error = 11;
					$message = "failed to save";
				}
			}
			else 
			{
				$error = 10;
				$message = "name exists";
			}
		}
		else 
		{
			$error = 9;
			$message = "group not found";
		}
	
		if ($error == 0) $response = array("success" => true, "session" => session_id());
		else $response = array("success" => false, "error" => $error, "message" => $message);
		$this->output($response);
	}
	
	public function login($name, $device)
	{
		$error = 0;
		$message = "";
		
		$user = new GamarUser();
		if ($user->initByLogin($name, $device))
		{
			$user->login();
			$this->logActivity("login", $user->getId());
		}
		else 
		{
			$error = 8;
			$message = "login failed";
		}
	
		if ($error == 0) $response = array("success" => true, "session" => session_id());
		else $response = array("success" => false, "error" => $error, "message" => $message);
		$this->output($response);
	}
	
	/*
	public function login($email, $password)
	{
		$error = 0;
		$message = "";
		
		if (validateEmail($email))
		{
			if (strlen($password) > 3)
			{
				$user = new GamarUser();
				if ($user->initByLogin($email, $password))
				{
					$user->login();
					$this->logActivity("login", $user->getId());
				}
				else 
				{
					$error = 8;
					$message = "login failed";
				}
			} 
			else 
			{
				$error = 7;
				$message = "invalid password";
			}
		}
		else 
		{
			$error = 6;
			$message = "invalid email address";
		}
	
		if ($error == 0) $response = array("success" => true, "session" => session_id());
		else $response = array("success" => false, "error" => $error, "message" => $message);
		$this->output($response);
	}
	*/
	
	public function logout()
	{
		GamAR::Logout();
		$response = array("success" => true);
		$this->output($response);
	}
	
}

?>