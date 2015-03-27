<?php require "inc/header.php"; ?>
<h1>Gruppen Verwalten</h1>

<?php 
	
	foreach($groups as $group)
	{
		echo "<span class=\"groupHead\" href=\"".$basePath."manage/group/".$group->getId()."\">".$group->getName()."<p>".$group->getHash()."</p></span>";
	
		echo "<div class=\"usersPanel\">";
		$users = $group->getUsers();
		if (count($users) > 0)
		{
			foreach($users as $user)
			{
				echo "<span>".$user->getName()."</span>";
			}
		}
		#else echo "Kein Spieler eingetragen.";
		echo "</div>";
	}

	if (count($groups) > 0) echo "<div class=\"separator\"></div>";
	if (!empty($error)) echo "<div class=\"errorNote\"><div class=\"errorNoteText\">$error</div></div>";
?>

	<form action="<?php echo $basePath; ?>manage/group" method="post">
		<h2>Neue Gruppe eintragen</h2>
		
		<label for="i1">Name</label>
		<input id="i1" type="text" name="name" value="" />
	
		<button type="submit">Gruppe eintragen</button>
	</form>

<?php require "inc/footer.php"; ?>