<?php

/*
	Controller is the base class for all controllers
	and provides functionalities such as loading views, 
	redirection, helpers for post variables, ...
*/

class Controller
{
	protected $ready = false;
	protected $gamar = null;
	
	function __construct($loginType = "")
	{
		$this->gamar = GamAR::$Instance; // each controller gets a reference to the main library object
		
		if (empty($loginType))
		{
			$this->ready = true;
			return;
		}
		
		$this->ready = GamAR::LoggedIn($loginType);
	}
	
	# POST
	protected function postContains($value, $minLength = 0)
	{
		if ($minLength > 0 && strlen($value) >= $minLength) return false; 
		if (!is_array($value)) $value = array($value);
		return count(array_diff($value, array_keys($_POST))) == 0;
	}
	
	protected function post($key)
	{
		if (array_key_exists($key, $_POST)) return $_POST[$key];
		return "";
	}
	
	# HELPERS
	protected function redirect($path)
	{
		header("Location: ".$this->gamar->getBasePath().$path);
	}
	
	protected function loadView($name, $data = null)
	{
		$path = "views/$name.php";
		
		if (file_exists($path)) 
		{
			# make some variables available to the view script
			$basePath = $this->gamar->GetBasePath();
			$segments = $this->gamar->GetSegments();
			$methodName = $this->gamar->GetMethodName();
			
			# expose all variables from the data array
			if (!is_null($data)) extract($data);
			require $path;
			return true;
		}
		else echo "View '$name' not found.";
		return false;
	}
	
	/*
	protected function errorView($headline, $message)
	{
		$this->loadView("error", array("headline" => $headline, "message" => $message));
	}*/
	
	protected function output($array) // associative array
	{
		global $apiEncoded;
		
		if ($apiEncoded) echo base64_encode(json_encode($array));
		else echo json_encode($array);
	}
	
	protected function loadLibrary($name)
	{
		$file = "library/$name.php";
		if (!file_exists($file)) 
		{
			error_log("Library '$name' not found!");
			return false;
		}
		require_once $file;
		return true;
	}
	
	protected function logActivity($message, $userId = 0, $gameId = 0)
	{
		$a = new GamarActivity();
		$a->setMessage($message);
		$a->setUserId($userId);
		$a->setGameId($gameId);
		$a->save();
	}
	
	public function isReady()
	{
		return $this->ready;
	}
	
}

?>