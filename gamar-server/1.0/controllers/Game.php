<?php

 // this controller requires a valid user login
 
class Game extends Controller
{
	private $imageMarkerTypes = array("image_marker", "ibeacon", "hash");

	function __construct()
	{
		parent::__construct("user");
	}
	
	public function checkin($marker, $value)
	{
		$error = 0;
		$message = "";
	
		$user = new GamarUser();
		if ($user->initFromSession())
		{	
			if (in_array($marker, $this->imageMarkerTypes))
			{
				$location = new GamarLocation();
				if ($location->initByField($marker, $value))
				{
					$game = new GamarGame();
					if ($game->initByField("location_id", $location->getId()))
					{
						$user->checkinGame($game);
						
						$response = array("success" => true, 
							"game" => 	$game->getName(), 
							"tries" => 	$game->getTries(), 
							"url" => 	$game->getAssetsUrl());
					}
					else
					{
						$error = 13;
						$message = "game not found";
					}
				
					/*
					$group = $user->getUsergroup();
					if (!is_null($group))
					{
						
					}
					else
					{
						$error = 13;
						$message = "usergroup not found";
					}
					*/
				}
				else
				{
					$error = 12;
					$message = "invalid marker";
				}
			}	
			else 
			{
				$error = 11;
				$message = "unknown marker type '$marker'";
			}
		}
		else
		{
			$error = 10;
			$message = "dead session";
		}
		
		if ($error != 0) $response = array("success" => false, "error" => $error, "message" => $message);
		$this->output($response);
	}
	
	public function checkout()
	{
		$error = 0;
		$message = "";
	
		$user = new GamarUser();
		if ($user->initFromSession())
		{	
			$user->checkoutGame();
			$response = array("success" => true);
		}
		else
		{
			$error = 10;
			$message = "dead session";
		}
		
		if ($error != 0) $response = array("success" => false, "error" => $error, "message" => $message);
		$this->output($response);
	}
	
	public function score($score)
	{
		$error = 0;
		$message = "";
	
		$user = new GamarUser();
		if ($user->initFromSession())
		{	
			$user->checkoutGame($score);
			$response = array("success" => true, 
				"rank" => 	123, 
				"time" => 	123);			
		}
		else
		{
			$error = 10;
			$message = "dead session";
		}
		
		if ($error != 0) $response = array("success" => false, "error" => $error, "message" => $message);
		$this->output($response);
	}
}

?>