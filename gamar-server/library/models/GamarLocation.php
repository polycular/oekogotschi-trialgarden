<?php

class GamarLocation extends DatabaseObject
{
	public function __construct()
	{
		parent::__construct("location");
	}

	############################################################################
	# GETTER
	public function getGeocoords()
	{
		return $this->get("geocoords");
	}

	public function getIbeacon()
	{
		return $this->get("ibeacon");
	}

	public function getImageMarker()
	{
		return $this->get("image_marker");
	}

	public function getHash()
	{
		return $this->get("hash");
	}

	############################################################################
	# SETTER
	public function setGeocoords($value)
	{
		$this->set("geocoords", $value);
	}

	public function setIbeacon($value)
	{
		$this->set("ibeacon", $value);
	}

	public function setImageMarker($value)
	{
		$this->set("image_marker", $value);
	}

	public function setHash($value)
	{
		$this->set("hash", $value);
	}

}

?>