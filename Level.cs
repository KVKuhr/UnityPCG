using UnityEngine;
using System.Collections;

[System.Serializable]
public class Level 
{

	private ArrayList lineList = new ArrayList ();
	private ArrayList collumList = new ArrayList ();
	private ArrayList enemyPos = new ArrayList ();
	private int[] playerPos = new int[2];
	private int[] finishPos= new int[2];
	private float score = 0;

	public Level(){
	}

	public void setLines(ArrayList linePositions,ArrayList collumPositions){
		lineList = linePositions;
		collumList = collumPositions;
	}

	public void setPositions(int[] pP, int[] pF){
		playerPos = pP;
		finishPos = pF;
	}
	public ArrayList getLine(){
		return lineList;
	}

	public ArrayList getCollum(){
		return collumList;
	}
	public ArrayList getEnemies(){
		return enemyPos;
	}
	public int[] getPlayerPos (){
		return playerPos;
	}
	public int[] getFinishPos (){
		return finishPos;
	}
	public void setEnemies(ArrayList eL){
		enemyPos = eL;
	}
	public void positiveEvaluation(){
		score++;
	}
	public void negativeEvaluation(){
		score--;
	}
	public float getScore(){
		return score;
	}










}

