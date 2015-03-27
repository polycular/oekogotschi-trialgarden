<?php

class Manage extends Controller
{
	private $menuItems;
	private $manager;

	function __construct()
	{
		parent::__construct("manager");
		
		$this->manager = new GamarGroupmanager();
		if (!$this->manager->initFromSession()) $this->ready = false;
		
		$this->menuItems = array
		(
			array('href' => "manage", 		'caption' => "Gruppen Verwalten"),
			array('href' => "hall-of-fame",	'caption' => "Hall-Of-Fame"),
			array('href' => "about", 		'caption' => "Über GamAR"),
			array('href' => "logout", 		'caption' => "Logout")
		);
	}
	
	public function index()
	{
		$this->group();
	}

	public function group($id = 0)
	{
		$content = array();
		$content['menuItems'] = $this->menuItems;
		$content['error'] = "";
	
		if ($this->postContains("name")) // create new group
		{
			if (strlen($this->post("name")) > 3)
			{
				# require unique name
				$group = new GamarUsergroup();
				if (!$group->initByName($this->post("name")))
				{
					# force unique hash
					$group = new GamarUsergroup();
					do $hash = humanHash(6);
					while($group->initByHash($hash)); 
					
					# store group
					$group = new GamarUsergroup();
					$group->setHash($hash);
					$group->setGroupManagerId($this->manager->getId());
					$group->setName($this->post("name"));
					$group->save();
					
					$id = $group->getId();
				}
				else $content['error'] = "Es existiert bereits eine Gruppe mit diesem Namen!";
			}
			else $content['error'] = "Der Name muss mindestens 4 Buchstaben enthalten!";
		}
		
		$content['groups'] = $this->manager->getGroups();
		$content['groupId'] = $id;
		
		$this->loadView("manage", $content);
	}
	
}

?>