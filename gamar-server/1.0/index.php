<?php 
	/*
		GamAR API
		http, post, json, php, mysql
	*/
	$version = "1.0";
	
	# INIT
	require_once "../library/init.php";
	
	$uri = $_SERVER['REQUEST_URI'];
	$segments = explode("/", substr($uri, strrpos($uri, $version) + strlen($version) + 1));
	if (empty($segments[0])) invalid("$projectName API $version"); // request needs at least 1 segment (method of home controller)

	if (isset($_POST['s']))
	{
		//error_log('session restart: '.$_POST['s']);
		session_id($_POST['s']);
	}
	
	session_start();

	# create master instance
	require_once "../library/$masterClassName.php";
	$master = new $masterClassName(); // instance is stored as static member

	# find and prepare controller and method to run
	$controllerName = ucfirst(str_replace(array("."), array(""), $segments[0]));
	$controllerFilename = $controllerFolder ."/".$controllerName.".php";
	
	if (!file_exists($controllerFilename)) // if the controller does not exist, use the home controller
	{
		//failed("Controller '$controllerName' not found!");
		array_unshift($segments, "");
		$controllerName = $homeControllerName;
		$controllerFilename = $controllerFolder ."/".$controllerName.".php";
	}
	
	if (!file_exists($controllerFilename)) failed(1, "controller does not exist");

	require_once $controllerFilename;
	
	$controller = new $controllerName();
	
	if ($controller->isReady())
	{
		$methodName = $segments[1];
		
		if (method_exists($controllerName, $methodName))
		{
			#TODO: decryption
			$json = json_decode(@$_POST['json']);
			if (is_null($json)) 
			{
				failed(2, "failed to decode json");
			}

			$args = array();
			$reflectionMethod = new ReflectionMethod($controllerName, $methodName);
			$params = $reflectionMethod->getParameters();
			
			# loop method parameters and collect data array from json
			foreach ($params as $param) 
			{
				$paramName = $param->getName();
				if (property_exists($json, $paramName)) $args[] = $json->$paramName;
				else 
				{
					failed(3, "parameter missing: ".$paramName);
				}
			}
			
			# run method of controller
			$reflectionMethod->invokeArgs($controller, $args);		
		}
		else
		{
			failed(4, "method does not exist");
		}
	}
	else 
	{
		failed(5, "controller is not ready/allowed");
	}
	
	function failed($nr, $msg)
	{
		global $apiEncoded;
	
		error_log($msg);
		$response = array();
		$response['success'] = false;
		$response['error'] = $nr;
		$response['message'] = $msg;
		
		if ($apiEncoded) echo base64_encode(json_encode($response));
		else echo json_encode($response);
		
		exit();
	}

	function invalid($msg)
	{
		header("HTTP/1.0 404 Not Found");
		error_log($msg);
		die($msg);
	}	
	
?>