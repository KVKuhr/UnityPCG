using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PCGControler : MonoBehaviour
{

	private string errorMsg = "";
	private PCGLevelMaker pcg;
	private List<Level> levelList;
	public static Level sentLevel;


	// Use this for initialization
	void Start ()
	{
		levelList = new List<Level> ();

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
		if (levelList.Contains(sentLevel) && sentLevel != null) {
			int index = levelList.IndexOf(sentLevel);
			levelList.RemoveAt(index);
			levelList.Insert(index,sentLevel);
		}


		SaveLoad.savedLevels = levelList;
		SaveLoad.Save ();
	}

	private void removeLevel (int n)
	{
		levelList.RemoveAt (n);
	}

	private int getLowestRated ()
	{
		float l = Mathf.Infinity;
		int returning = 0;
		for (int i = 0; i<levelList.Count; i++) {
			if (levelList [i].isPlayed ()) {
				if (l > levelList [i].getApproval ()) {
					l = levelList [i].getApproval ();
					returning = i;
				}
			}
		}
		return returning;
	}

	private int getHighestRated ()
	{
		float l = -Mathf.Infinity;
		int returning = 0;
		for (int i = 0; i<levelList.Count; i++) {
			if (levelList [i].isPlayed ()) {
				if (l < levelList [i].getApproval ()) {
					l = levelList [i].getApproval ();
					returning = i;
				}
			}
		}
		return returning;
	}

	private int getSecondHighestRated (int first)
	{
		float l = -Mathf.Infinity;
		int returning = 0;
		for (int i = 0; i<levelList.Count; i++) {
			if (levelList [i].isPlayed () && i != first) {
				if (l < levelList [i].getApproval ()) {
					l = levelList [i].getApproval ();
					returning = i;
				}
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
	
	public void startPCGSeed (int l)
	{
		SaveLoad.Load ();
		levelList = SaveLoad.savedLevels;
		levelList [l].wasPlayed ();
		sentLevel = levelList [l];
		saveLevel ();
		Application.LoadLevel ("ad");

	}


	//TODO
	private void generateNewManual (int l1, int l2, int remove)
	{

		sentLevel = null;

		Level newest = pcg.createNew (levelList [l1], levelList [l2]);
		int counting = 100;
		while (newest == null && counting > 0) {
			newest = pcg.createNew (levelList [l1], levelList [l2]);
			counting--;
		}
		if (newest != null) {
			levelList.Add (newest);
			setError ("...Done!");
		} else
			setError ("...Fail! Invalid Combo?");

		removeLevel (remove);

		saveLevel ();
	
	}

	private void generateNewAuto ()
	{
		int l1 = getHighestRated ();
		int l2 = getSecondHighestRated (l1);
		int remove = getLowestRated ();
					
		generateNewManual (l1, l2, remove);

	}

	public void makeList ()
	{
		levelList = pcg.createNewList (5);
		SaveLoad.savedLevels = levelList;
		SaveLoad.Save ();
	}

	public void likeLevel ()
	{
		sentLevel.positiveEvaluation ();
		saveLevel ();
	
	}

	public void dislikeLevel ()
	{
		sentLevel.negativeEvaluation ();
		saveLevel ();
	}

	private void setError (string error)
	{
		errorMsg = error;
	}

	public string getError ()
	{
		return errorMsg;
	}

	public void createNew (int l1, int l2, int remove,bool manual)
	{
		if(manual)
		generateNewManual (l1, l2, remove);
		else
			generateNewAuto ();

	}

	public string getStat (int x)
	{
		return levelList [x].getPlayed()+" / "+levelList [x].getApproval()*100+"%";
	}
	public bool isListEmpty(){
		loadLevels ();
		if (levelList.Count != 0)
			return false;
		return true;
	}

	/*
 			Falta no savelevel o sentlevel voltar pra lista. Feito isso, deve funfa susesegado tudo.



	 */



}
