<?php require "inc/header.php"; ?>
<h1>Gruppe Anmelden</h1>

<?php 
	if (!empty($error)) echo "<div class=\"errorNote\"><div class=\"errorNoteText\">$error</div></div>";
?>

<form id="loginGroupManagerForm" action="<?php echo $basePath; ?>loginGroupManager" method="post">
	<h2>Ich möchte eine Gruppe verwalten und mich anmelden...</h2>
	
	<label for="i1">E-Mail</label>
	<input id="i1" type="text" name="email" value="<?php echo $email; ?>" />
	
	<label for="i2">Passwort</label>
	<input id="i2" type="password" name="pass" value="" />
	
	<button type="submit">Anmelden</button>
</form>

<div class="separator"></div>

<form id="registerGroupManagerForm" action="<?php echo $basePath; ?>registerGroupManager" method="post">
	<h2>Ich möchte eine Gruppe verwalten und bin noch nicht registriert...</h2>
	
	<label for="i3">Name</label>
	<input id="i3" type="text" name="name" value="<?php echo $name; ?>" />
	
	<label for="i4">E-Mail</label>
	<input id="i4" type="text" name="email" value="<?php echo $email; ?>" />
	
	<label for="i5">Passwort</label>
	<input id="i5" type="password" name="pass1" value="" />
	
	<label for="i6">Passwort Wiederholung</label>
	<input id="i6" type="password" name="pass2" value="" />
	
	<button type="submit">Registrieren</button>
</form>

<?php require "inc/footer.php"; ?>