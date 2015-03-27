<?php require "inc/header.php"; ?>
<h1>GamAR HAll-Of-Fame</h1>

<h2>Highscore</h2>	
<?php
	$i = 0;
	foreach($highscore1 as $hs)
	{
		$i++;
		echo "#$i ".$hs['name']." (".$hs['group_name']."): ".$hs['points']." points<br />";
	}
	
	echo "<pre>".print_r($highscore1, true)."</pre>";
?>

<h2>Highscore by Game</h2>	
<?php
	foreach($highscore2 as $game)
	{
		echo "<b>".$game['game']."</b><br />";
	
		$i = 0;
		foreach($game['highscore'] as $hs)
		{
			$i++;
			echo "#$i ".$hs['name']." (".$hs['group_name']."): ".$hs['points']." points<br />";
		}
		
		echo "<br />";
	}
	
	echo "<pre>".print_r($highscore2, true)."</pre>";
?>


<?php require "inc/footer.php"; ?>