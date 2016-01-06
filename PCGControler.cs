using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PCGControler : MonoBehaviour
{


	private PCGLevelMaker pcg;
	private List<Level> levelList;
	public static Level sentLevel;


	// Use this for initialization
	void Start ()
	{
		GameObject.DontDestroyOnLoad (gameObject);

		if (FindObjectsOfType (GetType ()).Length > 1) {
			Destroy (gameObject);
		}




		pcg = gameObject.GetComponent<PCGLevelMaker> ();

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
	
	public void startLevel ()
	{
		Application.LoadLevel ("aa");
	}
	
	public void startPCGSeed ()
	{
		SaveLoad.Load ();
		levelList = SaveLoad.savedLevels;

		int l = getHighestRated ();

		sentLevel = levelList [l];
		Application.LoadLevel ("ad");
	}

	private void lowerAll ()
	{
		foreach (Level l in levelList) 
			l.negativeEvaluation ();		
	}

	//TODO
	private void generateNew ()
	{

	
	}


	public void makeList ()
	{
		levelList = pcg.createNewList (5);
		Debug.Log (levelList.Count);
		SaveLoad.savedLevels = levelList;
		SaveLoad.Save ();
	}

	public void likeLevel ()
	{
		sentLevel.positiveEvaluation ();
		saveLevel ();

	
	}

	private int getOther(int n){
		while(true){
			int i = Mathf.FloorToInt(Random.Range(0,levelList.Count-1));
			if(i!=n)
				return i;
		}
	}








}
