using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PCGControler : MonoBehaviour
{

	[SerializeField]
	private GameObject
		PCGHolder;
	private NewPCG pcg;
	private List<Level> levelList;
	public static Level sentLevel;


	// Use this for initialization
	void Start ()
	{
		pcg = PCGHolder.GetComponent<NewPCG> ();

	}

	private void loadLevels ()
	{
		SaveLoad.Load ();
		levelList = SaveLoad.savedLevels;
	}

	public void saveLevel ()
	{
		lowerAll ();
		levelList.RemoveAt (getLowestRated ());
		levelList.Add (sentLevel);
		SaveLoad.savedLevels = levelList;
		SaveLoad.Save ();
	}

	private int getLowestRated ()
	{
		float l = Mathf.Infinity;
		int returning = 0;
		for (int i = 0; i<levelList.Count; i++) {
			if (l > levelList [i].getScore ()) {
				l = levelList [i].getScore ();
				returning = i;
			}				
		}
		return returning;
	}

	private int getHighestRated ()
	{
		float l = -Mathf.Infinity;
		int returning = 0;
		for (int i = 0; i<levelList.Count; i++) {
			if (l < levelList [i].getScore ()) {
				l = levelList [i].getScore ();
				returning = i;
			}				
		}
		return returning;
	}
	
	public void startPCGLevel ()
	{
		Application.LoadLevel ("ac");
		
	}
	
	public void StartLevel ()
	{
		Application.LoadLevel ("aa");
	}
	
	public void startPCGSeed ()
	{
		Application.LoadLevel ("ad");
	}

	//TODO
	private void composeLevel ()
	{
		ArrayList lp = new ArrayList ();		
		ArrayList lc = new ArrayList ();
		ArrayList le = new ArrayList ();
		int bestPos = getHighestRated ();
		int pairPos = (int)Random.Range (0, levelList.Count - 1);
		Level best = levelList [bestPos];
		while (pairPos == bestPos)
			pairPos = (int)Random.Range (0, levelList.Count - 1);
		Level pair = levelList [pairPos];
		int[] playerPos;
		int[] endPos;



		for (int i = 0; i<30; i++) {
			if (getFromBest ())
				lp.Add (best.getLine () [i]);
			else
				lp.Add (pair.getLine () [i]);
		
			if (getFromBest ())
				lc.Add (best.getCollum () [i]);
			else
				lc.Add (pair.getCollum () [i]);
		
			if(i%3==0){

				if (getFromBest ())
					le.Add (best.getEnemies () [i]);
				else
					le.Add (pair.getEnemies () [i]);
			}
		}
		Level newLevel = new Level ();

		newLevel.setLines (lp,lc);
		newLevel.setEnemies (le);

		if (getFromBest ())
			playerPos = best.getPlayerPos ();
		else
			playerPos = pair.getPlayerPos ();

		if (getFromBest ())
			endPos = best.getFinishPos ();
		else
			endPos = pair.getFinishPos ();

		newLevel.setPositions (playerPos,endPos);

		sentLevel = newLevel;

	}

	private void lowerAll ()
	{
		foreach (Level l in levelList) 
			l.negativeEvaluation ();		
	}

	private bool getFromBest ()
	{
		return Random.value > 0.3;
	}






}
