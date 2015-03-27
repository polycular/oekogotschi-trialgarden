<html lang="de">
	<head>
		<meta http-equiv="content-type" content="text/html; charset=utf-8">
		<title>GamAR</title>
		<link rel="stylesheet" media="screen" href="<?php echo $basePath; ?>assets/website.css">  
	</head>
	<body>
	<div id="menuContainer">
		<div id="menu">
			<ul>
			<?php 
				for($i = 0; $i < count($menuItems); $i++)
				{
					echo "<li><a class=\"item".($i+1).(in_array(str_replace("-", "_", $menuItems[$i]['href']), array($methodName,$segments[0])) ? ' selected' : '')."\"".
						 " href=\"$basePath".$menuItems[$i]['href']."\">".$menuItems[$i]['caption']."</a></li>\n";
				}
			?>
			</ul>
		</div>
	</div>
	
	<div id="contentContainer">
