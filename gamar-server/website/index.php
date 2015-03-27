<?php 

	/*	GamAR Website
		- create system master instance
		- parse URI, get segments
		- find controller file and class
		- find and call method with arguments 
	*/

	require_once "../library/init.php";
	
	# get URL segments and site base path
	$basePath = "/";
	$path = trim($_SERVER['REQUEST_URI'], "/");
	$segments = explode("/", str_replace("-", "_", $path)); //NOTE: empty requests are redirected to the home controller below
	
	# establish database connection
	$dbc = mysql_connect($dbhost, $dbuser, $dbpass) or exit("Database not available!"); 
	mysql_select_db($dbname, $dbc) or exit("Database not found.");
	mysql_set_charset('utf8', $dbc);	

	# start session
	session_start();
	
	# create master instance
	require_once "../library/$masterClassName.php";
	$master = new $masterClassName(); // instance is stored as static member
	$master->SetBasePath($basePath);

	# find and run controller
	$controllerFolder = "controllers";
	$controllerName = (empty($segments[0]) ? $homeControllerName : ucfirst($segments[0]));
	$controllerFilename = $controllerFolder ."/".$controllerName.".php";
	
	if (!file_exists($controllerFilename)) // if the controller does not exist, use the home controller
	{
		//failed("Controller '$controllerName' not found!");
			
		array_unshift($segments, "");
		$controllerName = $homeControllerName;
		$controllerFilename = $controllerFolder ."/".$controllerName.".php";
	}
	
	require_once $controllerFilename;
	
	$controller = new $controllerName();
	if ($controller->isReady()) //NOTE: the controller checks login and permissions in its constructor
	{
		//session_write_close(); // close session to free session file (allows simultaneous connections from one client)

		# find method to run
		if (count($segments) > 1) $methodName = $segments[1];
		else $methodName = "index";
		
		if (method_exists($controllerName, $methodName))
		{
			$reflectionMethod = new ReflectionMethod($controllerName, $methodName);
			if ($reflectionMethod->class == $controllerName) // disallow inherited methods
			{			
				# add method arguments if available
				$args = array();
				if (count($segments) > 2)
				{
					$args = array_slice($segments, 2);
					$haveParams = count($args);
					
					$params = $reflectionMethod->getParameters();
					$requireParams = 0;
					foreach($params as $param) if (!$param->isOptional()) $requireParams++;
					
					if ($haveParams < $requireParams || $haveParams > count($params)) failed("Arguments mismatch!");
				}
				
				$master->SetSegments($segments);
				$master->SetMethodName($methodName);
				
				ob_start();

				# run method of controller
				$reflectionMethod->invokeArgs($controller, $args);

				# output
				$html = ob_get_clean();
				#$html = preg_replace("/[\n\r\t]/", "", $html); // remove whitespace
				header("Content-Type: text/html; charset=utf-8");
				echo '<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">'."\n";
				echo $html;
			}
			else failed("Method '$methodName' not found!");
		}
		else failed("Method '$methodName' not found!");
	}
	else 
	{
		//failed("Not allowed!");
		header("Location: ".$basePath."register");
	}

	function failed($msg)
	{
		header("Location: /", true, 301);
		error_log($msg);
		#header("HTTP/1.0 404 Not Found");
		#echo "Error: $msg";
		exit();
	}


?>