using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grammar : MonoBehaviour
{

	private LevelGrammar currentLevel;
	[SerializeField]
	private Room[]
		normalRoomList;
	[SerializeField]
	private Room[]
		specialRoomList;
	private int[,]
		levelMap;
	private List<Room> iRooms = new List<Room> ();
	private int[] endRoom;
	private int[] startRoom ;

	//Djikstra Attributes
	private List<int[]> vRooms = new List<int[]> ();
	private List<int[]> tRooms = new List<int[]> ();

	void Start ()
	{
		createRandomLevel ();
		makeUsable ();
		convertToObjects ();

	}

	private void convertToObjects ()
	{
		int x = 0;
		int y = 0;
		//levelMap = currentLevel.getMap ();
		for (int m = 0; m<levelMap.GetLength(0); m++) {
			for (int n = 0; n<levelMap.GetLength(1); n++) {
				Vector3 place = new Vector3 (x, y, 0);

				if (levelMap [m, n] > 1) {
					iRooms.Add (GameObject.Instantiate (normalRoomList [levelMap [m, n] - 2], place, Quaternion.identity) as Room);
				} else {
					if (levelMap [m, n] > -1)
					if (levelMap [m, n] == 0) {
						iRooms.Add (GameObject.Instantiate (specialRoomList [levelMap [m, n]], place, Quaternion.identity) as Room);

					} else {
						iRooms.Add (GameObject.Instantiate (specialRoomList [levelMap [m, n]], place, Quaternion.identity) as Room);

					}
				}
				x += 4;
			}
			x = 0;
			y += 4;
		}
		Debug.Log (levelMap.GetLength (0));
	}

	private void createRandomLevel ()
	{

		int[,] newlevelMap = createNewMap (10, 10);

		//LevelGrammar newLevel = new LevelGrammar (newlevelMap);

		levelMap = newlevelMap;
		setEndFin ();

	}

	private void setEndFin ()
	{
		for (int m = 0; m<levelMap.GetLength(0); m++) {
			for (int n = 0; n<levelMap.GetLength(1); n++) {
				if (levelMap [m, n] == 0)
					startRoom = new int[2] {m,n};

				if (levelMap [m, n] == 1)
					endRoom = new int[2] {m,n};

			}
		}
	}

	private int[,] createNewMap (int m, int n)
	{
		int[,] ret = new int[m, n];
		ret = addline (ret, 0, createStartLine (n));
		ret = addline (ret, m - 1, createFinishLine (n));

		for (int i=1; i<m-1; i++) {
			ret = addline (ret, i, createRandomLine (n));
		}

		return ret;
	}

	private int[] createRandomLine (int size)
	{
		int[] ret = new int[size];

		for (int i = 0; i<size; i++)
			ret [i] = getRandomBlock ();
		return ret;

	}

	private int getRandomBlock ()
	{
		return Mathf.FloorToInt (Random.Range (2, 12.999999f));
	}

	private int[] createStartLine (int size)
	{
		int[] ret = new int[size];
		ret [0] = 0;
		for (int i = 1; i<size; i++)
			ret [i] = 2;
		return ret;
	}

	private int[] createFinishLine (int size)
	{
		int[] ret = new int[size];
		ret [9] = 1;
		for (int i = 0; i<size-1; i++)
			ret [i] = 2;
		return ret;
	}

	private int[,] addline (int[,] matrix, int index, int[] toAdd)
	{
		for (int i = 0; i<toAdd.Length; i++) {
			matrix [index, i] = toAdd [i];
		}
		return matrix;
	}
	//Start Djikstra

	private void makeUsable ()
	{
		bool notUsable = true;

		while (notUsable)
			if (djikstraStart ())
				notUsable = false;
	
	}

	private bool djikstraStart ()
	{

		tRooms.Add (startRoom);

		while (tRooms.Count != 0) {
			int[] closest = tRooms [findClosest ()];
			bool result = djikstraRun (closest);

			if (result) {
				return true;
			}
		}
		adaptRoom (vRooms [findClosestVisited ()]);
		return false;
		//Find closest Room
		//Turn into 0;
		//add to tRooms

	}

	private bool djikstraRun (int[] current)
	{
		vRooms.Add (current);
		tRooms.Remove (current);

		if (isFinish (current))
			return true;


		int[] aux = new int[2];

		aux [0] = current [0] + 1;
		aux [1] = current [1];

		if (aux [0] >= 0 && aux [0] < levelMap.GetLength (0) && aux [1] >= 0 && aux [1] < levelMap.GetLength (1) &&
			current [0] >= 0 && current [0] < levelMap.GetLength (0) && current [1] >= 0 && current [1] < levelMap.GetLength (1))
		if (ifHelper (current, aux))
			return true;

		aux [0] = current [0];
		aux [1] = current [1] + 1;

		if (aux [0] >= 0 && aux [0] < levelMap.GetLength (0) && aux [1] >= 0 && aux [1] < levelMap.GetLength (1) &&
			current [0] >= 0 && current [0] < levelMap.GetLength (0) && current [1] >= 0 && current [1] < levelMap.GetLength (1))
		if (ifHelper (current, aux))
			return true;
		
		aux [0] = current [0] - 1;
		aux [1] = current [1];

		if (aux [0] >= 0 && aux [0] < levelMap.GetLength (0) && aux [1] >= 0 && aux [1] < levelMap.GetLength (1) &&
			current [0] >= 0 && current [0] < levelMap.GetLength (0) && current [1] >= 0 && current [1] < levelMap.GetLength (1))
		if (ifHelper (current, aux))
			return true;
		
		aux [0] = current [0];
		aux [1] = current [1] - 1;

		if (aux [0] >= 0 && aux [0] < levelMap.GetLength (0) && aux [1] >= 0 && aux [1] < levelMap.GetLength (1) &&
			current [0] >= 0 && current [0] < levelMap.GetLength (0) && current [1] >= 0 && current [1] < levelMap.GetLength (1))
		if (ifHelper (current, aux))
			return true;
		
		return false;


	}

	private bool isFinish (int[] current)
	{	
		if (current == endRoom)
			return true;		
		return false;
		
	}

	private bool ifHelper (int[] current, int[] next)
	{

		int cNumber = levelMap [current [0], current [1]];

		int nNumber = levelMap [next [0], next [1]];

		if (isConnected (cNumber, nNumber)) {
			tRooms.Add (next);
			if (isFinish (next))
				return true;
		}
		return false;	
	}

	private int findClosest ()
	{
		int returned = 0;
		float aux = Mathf.Infinity;
		Vector2 finishpoint = new Vector2 (endRoom [0], endRoom [1]);
		for (int i = 0; i<tRooms.Count; i++) {
			Vector2 currentPoint = new Vector2 (tRooms [i] [0], tRooms [i] [1]);
			if (Vector2.Distance (currentPoint, finishpoint) < aux) {
				returned = i;
				aux = Vector2.Distance (currentPoint, finishpoint);
			}
		}
		return returned;	
	}

	private int findClosestVisited ()
	{
		int returned = 0;
		float aux = Mathf.Infinity;
		Vector2 finishpoint = new Vector2 (endRoom [0], endRoom [1]);
		for (int i = 0; i<vRooms.Count; i++) {
			Vector2 currentPoint = new Vector2 (vRooms [i] [0], vRooms [i] [1]);
			if (Vector2.Distance (currentPoint, finishpoint) < aux) {
				returned = i;
				aux = Vector2.Distance (currentPoint, finishpoint);
			}
		}
		return returned;	
	}
			
	private bool isConnected (int first, int second)
	{
		Room fRoom;
		Room sRoom;
		
		if (first > 1)
			fRoom = normalRoomList [first - 2];
		else
			fRoom = specialRoomList [first];
		
		
		if (second > 1)
			sRoom = normalRoomList [second - 2];
		else
			sRoom = specialRoomList [second];
		
		return isConnected (fRoom, sRoom);
	}
	
	private bool isConnected (Room first, Room second)
	{
		if (first.getExitList () [0] != null)
		if (second.getExitList () [2] != null)
			return true;
		if (first.getExitList () [1] != null)
		if (second.getExitList () [3] != null)
			return true;
		if (first.getExitList () [2] != null)
		if (second.getExitList () [0] != null)
			return true;
		if (first.getExitList () [3] != null)
		if (second.getExitList () [1] != null)
			return true;
		return false;	
	}
	
	private void adaptRoom (int[] toChange)
	{
		vRooms.Clear ();
		tRooms.Clear ();
		
		levelMap [toChange [0], toChange [1]] = 0;
	}


}
