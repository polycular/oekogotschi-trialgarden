<?php

class GamarScore extends DatabaseObject
{
	public static function Highscore($metagameId, $limit, $grouped = false)
	{
		if ($grouped)
		{
			$r = mysql_query("SELECT user.id AS user_id, user.name, user.character, usergroup.id AS group_id, usergroup.name AS group_name, SUM(points) AS points FROM 
				(SELECT user_id, game_id, MAX(value) AS points FROM score
				JOIN game ON game.id = game_id
				WHERE game.metagame_id = $metagameId
				GROUP BY user_id, game_id
				ORDER BY game_id, points DESC) AS board
				JOIN user ON user.id = board.user_id
				JOIN usergroup ON usergroup.id = user.usergroup_id
				GROUP BY user_id
				LIMIT 0,$limit");
				
			$list = array();
			while($d = mysql_fetch_assoc($r)) $list[] = $d;
			return $list;
		}
		else
		{
			$games = GamarGame::GetAll($metagameId);
		
			$hs = array();
			foreach($games as $game)
			{
				$hs[] = array('game' => $game->getName(), 'game_id' => $game->getId(), 'highscore' => self::GameHighscore($game->getId(), $limit));
			}
		}
		
		return $hs;
	}
	
	public static function GameHighscore($gameId, $limit)
	{
		$r = mysql_query("SELECT user.id AS user_id, user.name, user.character, user.usergroup_id AS group_id, 
			usergroup.name AS group_name, 
			MAX(value) AS points 
			FROM score
			JOIN user ON user.id = score.user_id
			JOIN usergroup ON usergroup.id = user.usergroup_id
			WHERE game_id = $gameId 
			GROUP BY user_id
			ORDER BY points DESC 
			LIMIT 0,$limit");
			
		$list = array();
		while($d = mysql_fetch_assoc($r)) $list[] = $d;
		return $list;
	}

	public function __construct()
	{
		parent::__construct("score");
	}

	############################################################################
	# GETTER
	public function getUserId()
	{
		return $this->get("user_id");
	}

	public function getGameId()
	{
		return $this->get("game_id");
	}

	public function getTime()
	{
		return $this->get("time");
	}

	public function getValue()
	{
		return $this->get("value");
	}

	public function getNote()
	{
		return $this->get("note");
	}

	############################################################################
	# SETTER
	public function setUserId($value)
	{
		$this->set("user_id", $value);
	}

	public function setGameId($value)
	{
		$this->set("game_id", $value);
	}

	public function setTime($value)
	{
		$this->set("time", $value);
	}

	public function setValue($value)
	{
		$this->set("value", $value);
	}

	public function setNote($value)
	{
		$this->set("note", $value);
	}

}

?>