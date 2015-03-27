<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01//EN"
    "http://www.w3.org/TR/html4/strict.dtd">
<html lang="en">
  <head>
	<style>

	li, ul {
		font-family:Arial;
	}
	
	ul {
		font-weight:bold;
		margin-top:20px;
	}
	
	li {
		font-weight:normal;
		margin-bottom:4px;
	}
	
	ul.players {
		
	}
	
	.hidden {
		display:none;
	}
	
	.toggleList {
		margin-left:10px;
		padding:4px 6px;
		border:1px solid #333;
		background-color:#FFF;
		color:#000;
		text-decoration:none;
	}
	
	a.selected {
		background-color:#000;
		color:#FFF;
	}
	
	.toggleList:hover {
		background-color:#EEE;
	}

	.toggleList.selected:hover {
		background-color:#333;
	}

	.tournamentDetails, .playerDetails {
		font-size:10px;
		color:#999;
	}
	
	</style>  
    <meta http-equiv="content-type" content="text/html; charset=utf-8">
    <title>Dump</title>
    <script type="text/javascript" src="<?php echo $basePath; ?>assets/jquery-2.1.3.min.js"></script>
    <script type="text/javascript">
		$(document).ready(function() {
			
			var currentName = "";
			
			$('.toggleList').click(function()
			{
				var newName = $(this).attr('href');
				if (newName == currentName) return false;
				
				$('.toggleList').removeClass('selected');
				$(this).addClass('selected');

				if (currentName != "") $(currentName).hide();
				currentName = newName;
				$(currentName).show();
				return false;
			});
			
		});	
	</script>
  </head>
  <body>
		<?php echo $list; ?>
  </body>
</html>