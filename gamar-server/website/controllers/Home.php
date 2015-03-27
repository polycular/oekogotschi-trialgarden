<?php

class Home extends Controller
{
	private $menuItems;

	function __construct()
	{
		parent::__construct();
		
		$this->menuItems = array
		(
			array('href' => "register", 	'caption' => "Gruppen Verwalten"),
			array('href' => "hall-of-fame",	'caption' => "Hall-Of-Fame"),
			array('href' => "about", 		'caption' => "Über GamAR")
		);
		
		if (GamAR::LoggedIn("manager")) $this->menuItems[] = array('href' => "logout", 'caption' => "Logout");
	}
	
	public function index()
	{
		$content = array();
		$content['menuItems'] = $this->menuItems;
		$this->loadView("home", $content);
	}
	
	public function register()
	{
		if (GamAR::LoggedIn("manager"))
		{
			$this->redirect("manage");
			return;
		}
	
		$content = array();
		$content['menuItems'] = $this->menuItems;
		$content['error'] = "";
		$content['name'] = "";
		$content['email'] = "";
		$this->loadView("register", $content);
	}
	
	public function registerGroupManager()
	{
		$content = array();
		$content['menuItems'] = $this->menuItems;
		$content['error'] = "";
		$content['name'] = "";
		$content['email'] = "";
		
		if ($this->postContains("name") && $this->postContains("email") && $this->postContains("pass1") && $this->postContains("pass2"))
		{
			$content['name'] = $this->post("name");
			$content['email'] = $this->post("email");
			
			if (strlen($this->post("name")) > 2)
			{		
				if (strlen($this->post("email")) > 5 && validateEmail($this->post("email")))
				{		
					if (strlen($this->post("pass1")) > 5)
					{		
						$man = new GamarGroupmanager();
						if (!$man->initByEmail($this->post("email")))
						{
							if ($this->post("pass1") == $this->post("pass2")) 
							{
								$man->setName($this->post("name"));
								$man->setEmail($this->post("email"));
								$man->setPassword($this->post("pass1"));
								$man->save();
							
								//TODO: send double-opt-in mail
								
								$man->login();
								$this->redirect("manage");
								return;
							}
							else $content['error'] = "Das Passwort stimmt nicht mit der Wiederholung überein!";
						}
						else $content['error'] = "Die angegebene E-Mail-Adresse ist bereits in Verwendung!";
					}
					else $content['error'] = "Das Passwort muss mindestens 6 Buchstaben enthalten!";
				}
				else $content['error'] = "Geben Sie eine gültige E-Mail-Adresse ein!";
			}
			else $content['error'] = "Geben Sie einen Namen mit mindestens 3 Buchstaben ein!";
		}
		else $content['error'] = "Es ist ein Fehler aufgetreten. Bitte versuchen Sie es erneut!";

		$this->loadView("register", $content);
	}
	
	public function loginGroupManager()
	{
		$content = array();
		$content['menuItems'] = $this->menuItems;
		$content['error'] = "";
		$content['name'] = "";
		$content['email'] = "";
		
		if ($this->postContains("email") && $this->postContains("pass"))
		{
			$content['email'] = $this->post("email");
		
			$man = new GamarGroupmanager();
			if ($man->initByLogin($this->post("email"), $this->post("pass")))
			{
				$man->login();
				$this->redirect("manage");
				return;
			}
			else $content['error'] = "Anmeldung fehlgeschlagen. Überprüfen Sie die Zugangsdaten!";
		}
		else $content['error'] = "Es ist ein Fehler aufgetreten. Bitte versuchen Sie es erneut!";

		$this->loadView("register", $content);
	}
	
	public function logout()
	{
		GamAR::Logout();
		$this->redirect("");
	}
	
	public function hall_of_fame()
	{
		$content = array();
		$content['menuItems'] = $this->menuItems;
		$content['highscore1'] = GamarScore::Highscore(1, 10, true); // metagame id, player limit, grouped
		$content['highscore2'] = GamarScore::Highscore(1, 10, false); // metagame id, player limit, not grouped
		$this->loadView("halloffame", $content);
	}
	
	public function highscore($metagameId = 1, $grouped = 1, $limit = 10)
	{
		$grouped = $grouped == 1;
		echo json_encode(GamarScore::Highscore($metagameId, $limit, $grouped));
	}
	
	public function about()
	{
		$content = array();
		$content['menuItems'] = $this->menuItems;
		$this->loadView("home", $content);
	}
	
}

?>