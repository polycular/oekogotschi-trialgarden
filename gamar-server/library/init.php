<?
	$dir = dirname(__FILE__);
	
	require_once "$dir/settings.php";
	require_once "$dir/core/DatabaseObject.php";
	require_once "$dir/core/Controller.php";
	require_once "$dir/core/helpers.php";
	
	require_once "$dir/models/GamarActivity.php";
	require_once "$dir/models/GamarUser.php";
	require_once "$dir/models/GamarUsergroup.php";
	require_once "$dir/models/GamarGroupmanager.php";
	require_once "$dir/models/GamarLocation.php";
	require_once "$dir/models/GamarGame.php";
	require_once "$dir/models/GamarPlay.php";
	require_once "$dir/models/GamarScore.php";
	
	$dbconn = mysql_connect($dbhost,$dbuser,$dbpass) or exit("Database not available!"); 
	mysql_select_db($dbname,$dbconn) or exit("DB not found.");
	mysql_set_charset('utf8' , $dbconn);
	
?>