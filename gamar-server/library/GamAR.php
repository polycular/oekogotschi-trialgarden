<?php

class GamAR
{
	public static $Instance;

	private $basePath = "";
	private $methodName = "";
	private $segments = array();
	
	function __construct()
	{
		self::$Instance = $this;
	}
	
	public static function LoggedIn($type)
	{
		return isset($_SESSION[$type]);
	}
	
	public static function Logout()
	{
		unset($_SESSION['user']);
		unset($_SESSION['manager']);
		session_unset();
	}
	
	public static function SessionData($type)
	{
		if (isset($_SESSION[$type])) return $_SESSION[$type];
		return null;
	}
	
	public function setBasePath($path)
	{
		$this->basePath = $path;
	}
	
	public function getBasePath()
	{
		return $this->basePath;
	}
	
	public function setSegments($segments)
	{
		$this->segments = $segments;
	}
	
	public function getSegments()
	{
		return $this->segments;
	}	
	
	public function setMethodName($methodName)
	{
		$this->methodName = $methodName;
	}
	
	public function getMethodName()
	{
		return $this->methodName;
	}	
	
}


?>