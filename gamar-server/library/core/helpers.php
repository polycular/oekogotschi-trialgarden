<?php 

	function encodePassword($password)
	{
		return md5($password);
	}

	function validateEmail($string)
	{
		return filter_var($string, FILTER_VALIDATE_EMAIL);
	}

	function humanHash($len)
	{
		return substr(str_shuffle("ABCDEFGHIJKLMNPQRSTUVWXYZ1234567890"), 0, $len);
	}

	function padNumber($input, $num)
	{
		return str_pad($input, $num, "0", STR_PAD_LEFT);
	}

	function match($regexp, $text)
	{
		preg_match($regexp, $text, $result);
		if (count($result) == 2)
		{
			return $result[1];
		}
		return "";
	}
	
	function matches($regexp, $text)
	{
		preg_match($regexp, $text, $result);
		return $result;
	}
	
	function matchAll($regexp, $text)
	{
		$output = array();
		preg_match_all($regexp, $text, $output);	
		return $output;
	}
	
	function removeWhitespace($string)
	{
		$string = preg_replace_callback("/(&#[0-9]+;)/", function($m) { return mb_convert_encoding($m[1], "UTF-8", "HTML-ENTITIES"); }, $string);
		$string = str_replace(array("\n", "\r", "\t", "&nbsp;"), array(" ", " ", " ", " "), $string);
		$string = preg_replace("/\>\s+\</", "><", $string);
		$string = preg_replace("/\s+/", " ", $string);
		$string = preg_replace("/>\s/", ">", $string);
		return $string;
	}
	
	function splitString($string, $separator, $index = -1)
	{
		$parts = explode($separator, $string);
		if ($index == -1) return $parts;
		return $index < count($parts) ? trim($parts[$index]) : "";
	}
	
	function html2array($html) 
	{
		$parser = new HtmlParser($html);
		return $parser->toArray();
	} 	

?>