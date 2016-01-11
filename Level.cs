using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Level
{

	private List<int[]> lineList = new List<int[]> ();
	private List<int[]> collumList = new List<int[]> ();
	private List<int[]> enemyPos = new List<int[]> ();
	private int[] playerPos = new int[2];
	private int[] finishPos = new int[2];
	private float score = 0;
	private int timesPlayed = 0;

	public Level ()
	{
	}

	public void setLines (List<int[]> linePositions, List<int[]> collumPositions)
	{
		lineList = linePositions;
		collumList = collumPositions;
	}

	public void setPositions (int[] pP, int[] pF)
	{
		playerPos = pP;
		finishPos = pF;
	}

	public List<int[]> getLine ()
	{
		return lineList;
	}

	public List<int[]> getCollum ()
	{
		return collumList;
	}

	public List<int[]> getEnemies ()
	{
		return enemyPos;
	}

	public int[] getPlayerPos ()
	{
		return playerPos;
	}

	public int[] getFinishPos ()
	{
		return finishPos;
	}

	public void setEnemies (List<int[]> eL)
	{
		enemyPos = eL;
	}

	public void positiveEvaluation ()
	{
		score++;
	}

	public void negativeEvaluation ()
	{
		score--;
	}

	public float getScore ()
	{
		return score;
	}

	public void wasPlayed ()
	{
		timesPlayed++;
	}

	public float getApproval ()
	{
		if (timesPlayed != 0)
			return score / timesPlayed;
		else
			return 0;
	}

	public bool isPlayed ()
	{
		if (timesPlayed != 0)
			return true;
		else
			return false;
	}

	public int getPlayed ()
	{
		return timesPlayed;
	}









}

