using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerate : MonoBehaviour
{
	private Rules[] ruleset;
	[SerializeField]
	private Room[]
		normalRoomList;
	[SerializeField]
	private Room[]
		specialRoomList;

	//This list contains all the rooms in the main path.
	private List<Room> roomList;

	//nph = number of times rules will be applied between the start and finish.
	private int nph;

	public LevelGrammar generateNew (Rules[] pRules)
	{
		ruleset = pRules;


		roomList = new List<Room> ();

		roomList.Add (specialRoomList [0]);
		nph = 4;

		return null;
	}

	private bool exertRules ()
	{
		while (nph > 0) {







			nph--;
		}
		return false;
	}

	/*
	Como fazer:
	guardar o array nextRoom do quarto a ser modificado em um array novo.
	se o quarto em questao fizer parte da lista roomlist, ou seja, eh parte do caminho principal, 

	 */
	private int getCurrentEnd ()
	{
		return roomList [roomList.Count - 1].getInt ();
	}

	private int[] getUsableExits (Room r)
	{
		int[] ret = {0,1,2,3};
		foreach (int i in ret) {
			if (r.getRoom (i) != null || r.getRoom (i).getExitList () [i] == null)
				ret [i] = -1;
		}

		return ret;
	}

	private int getRandomFreeExit (int[] elist)
	{
		int tsize = 0;

		foreach (int i in elist)
			if (i != -1)
				tsize++;
		

		if (tsize > 0) {
			int[] ret = new int[tsize];
			int aux = 0;
			foreach (int i in elist)
				if (i != -1)
					ret [aux++] = i;
			return ret[Random.Range(0,tsize)];
			
		}
		return -1;
	
	}
	
	private Rules[] thatApply (int room,int e)
	{
		List<Rules> aux = new List<Rules>();
		foreach (Rules r in ruleset)
			if (r.isOrigin (room) && r.isExit(e)) {
			aux.Add(r);		
		}				
		return aux.ToArray();
	}
	private Rules getOneRule(Rules[] l){
		return l [Random.Range (0, l.Length)];
	}

	private Room getRoomFromInt(int x){

		if(x<2)
			return specialRoomList[x];
		return normalRoomList [x - 2];	
	}


	/// <summary>
	/// This method applies the rule r   	
	/// </summary>
	/// <param name="r">Number of the rooms to be added. </param>
	/// <param name="c">The room that will recieve the rooms added. </param>
	/// <param name="e">The exit to be used on the c room. </param>


	private bool applyRule (Room[] r, Room c, int e, bool endP)
	{
		if (roomList.Contains (c)) {
			int x = roomList.IndexOf(c);

			roomList[x].setRoom(e,r[0]);

			if(endP)
				roomList.AddRange(r);
		}



		return false;
	}






}
