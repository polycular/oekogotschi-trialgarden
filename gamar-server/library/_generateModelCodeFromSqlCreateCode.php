<?php

$code = isset($_POST['sql']) ? $_POST['sql'] : "CREATE TABLE `course` (
	`id` BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
	`name` VARCHAR(50) NOT NULL,
	`location` VARCHAR(50) NULL DEFAULT NULL,
	`city` VARCHAR(50) NULL DEFAULT NULL,
	`longitude` FLOAT NULL DEFAULT NULL,
	`latitude` FLOAT NULL DEFAULT NULL,
	`num_holes` INT(10) UNSIGNED NULL DEFAULT NULL,
	PRIMARY KEY (`id`)
)
COLLATE='utf8_general_ci'
ENGINE=MyISAM;";

?>

<form action="" method="post">
	<textarea name="sql" style="width:550px; height:400px;"><? echo $code; ?></textarea>
	<br />
	<input type="submit" value="Generate" />
</form><hr />
<?

	function fieldName($in)
	{
		$parts = explode("_", $in);
		
		for($i = 1; $i < count($parts); $i++)
		{
			$parts[$i] = ucfirst($parts[$i]);
		}

		return implode("", $parts);
	}

	if (isset($_POST['sql']))
	{
		$code = $_POST['sql'];
		
		preg_match("/CREATE TABLE \`(.*)\`/", $code, $output);
		if (count($output) == 2)
		{		
			$className = $output[1];
			
			$getterPhp = "\t############################################################################\n\t# GETTER\n";
			$setterPhp = "\t############################################################################\n\t# SETTER\n";
			
			$hasCreationField = false;
			preg_match_all("/\t\`(.+)\`/", $code, $output);	
			foreach($output[1] as $field)
			{
				if ($field == "id") continue;
				if ($field == "creation") $hasCreationField = true;
				$fieldName = ucfirst(fieldName($field));
				$getterPhp .= "\tpublic function get".$fieldName."()\n\t{\n\t\treturn $"."this->get(\"$field\");\n\t}\n\n";
				$setterPhp .= "\tpublic function set".$fieldName."($"."value)\n\t{\n\t\t$"."this->set(\"$field\", $"."value);\n\t}\n\n";
			}
			
			$php = "<"."?php\n\nclass Gamar".ucfirst($className)." extends DatabaseObject\n{\n\tpublic function __construct()\n\t{\n\t\tparent::__construct(\"$className\");\n";
			if ($hasCreationField) $php .= "\t\t$"."this->set(\"creation\", time());\n";
			$php .= "\t}\n\n";
			$php .= $getterPhp;
			$php .= $setterPhp;
			$php .= "}\n\n?>";
			echo "<pre>".htmlspecialchars($php)."</pre>";
		}
	}
?>