using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PCGLevelMaker : MonoBehaviour
{

	//Start of Generator V2.
	[SerializeField]
	private GameObject
		wallprefab;
	[SerializeField]
	private GameObject
		groundprefab;
	[SerializeField]
	private int
		matrixSize;
	[SerializeField]
	private int
		horizontalWalls;
	[SerializeField]
	private int
		verticalWalls;
	[SerializeField]
	private int
		wallMaxSize;
	[SerializeField]
	private int
		wallMinSize;
		
	//End of Generator V2.
		
	//Start of Generator V3.
	[SerializeField]
	private GameObject
		playerPrefab;
	[SerializeField]
	private GameObject
		enemy;
	[SerializeField]
	private float
		distanceToPlayer;
	[SerializeField]
	private int
		numberOfEnemies;
	[SerializeField]
	private GameObject
		finishPrefab;
	private GameObject player;
	private GameObject finish;

		
		
	//End of Generator V3.
		
	private ArrayList toVisit = new ArrayList ();
	private ArrayList visited = new ArrayList ();
		
		
	//Level for Loading
	private Level currentLevel;
		
	private void createLevel ()
	{
		//Calls the function that oversees terrain creation. Generator V2.
		createTerrain ();
			
		createEntities (true);
			
		if (!makeUsable ())
			createLevel ();

	}	
		

	//Start of V2.
		
	//Creates the Terrain for the Level. Generator V2.
	private void createTerrain ()
	{
		if (currentLevel == null)
			createSeed ();
			
		//Calls the function to fill the outer walls.
		createOuterBounds ();
			
		//Creates n = horizontalWalls horizontal Walls.
		createHorizontal ();
			
		//Creates n = verticalWalls vertical Walls.
		createVertical ();
			
			
	}
		
	//Creates the outer bounds for the map, fills out the line and collumn 0 and matrixSize with wallPrefabs.
	private void createOuterBounds ()
	{
		for (int m = 0; m<matrixSize; m++) {
			Instantiate (groundprefab, new Vector3 (m * 10, 0, 0), Quaternion.identity);		
			Instantiate (wallprefab, new Vector3 (m * 10, 10 * matrixSize - 1, 0), Quaternion.identity);
			Instantiate (wallprefab, new Vector3 (0, m * 10, 0), Quaternion.identity);		
			Instantiate (wallprefab, new Vector3 (10 * matrixSize - 1, m * 10, 0), Quaternion.identity);		
		}
	}
		
		
		
	//Takes a point in the pseudomatrix and keeps building HORIZONTALLY to the LEFT
	//until either it reaches the outer wall or the wallCurrentSize becomes 0
	private void buildingLine (int x, int y, int n)
	{
			
		for (int l = x; l<matrixSize && n > 0; l++) {
			Instantiate (groundprefab, new Vector3 (l * 10, y * 10, 0), Quaternion.identity);
			n--;
		}			
			
	}
		
	//Takes a point in the pseudomatrix and keeps building DOWN 
	//until either it reaches the outer wall or the wallCurrentSize becomes 0
	private void buildingVertical (int x, int y, int n)
	{	
		for (int l = y; l<matrixSize && n > 0; l++) {
			Instantiate (wallprefab, new Vector3 (x * 10, l * 10, 0), Quaternion.identity);
			n--;
		}			
			
	}
		
	//Finds a random point in the pseudomatrix.
	public int[] getRandomBlock ()
	{
		int[] returning = {0,0};
		returning [0] = (int)Random.Range (1, matrixSize - 2);
		returning [1] = (int)Random.Range (1, matrixSize - 2);	
		return returning;
	}

	public int[] getRandomBlockStart ()
	{

		int[] returning = {0,0,0};
		returning [0] = (int)Random.Range (1, matrixSize - 2);
		returning [1] = (int)Random.Range (1, matrixSize - 2);
		returning [2] = (int)Random.Range (wallMinSize, wallMaxSize);

		return returning;
	}
		
	//Oversees the creation of HORIZONTAL walls.
	private void createHorizontal ()
	{
		List<int[]> lineStarts = currentLevel.getLine ();
		for (int h = 0; h<lineStarts.Count; h++) {
				
			int [] aux = (int[])lineStarts [h]; 
			buildingLine (aux [0], aux [1], aux [2]);				
		}	
	}
		
	//Oversees the creation of VERTICAL walls.
	private void createVertical ()
	{
		List<int[]> verticalStarts = currentLevel.getCollum ();
		for (int v = 0; v<verticalStarts.Count; v++) {
				
			int [] aux = (int[])verticalStarts [v]; 
			buildingVertical (aux [0], aux [1], aux [2]);				
		}		
	}
	//End of V2
		
	//Start of V3
		
	//Sets the location of the player, enemies and finish.
	private void createEntities (bool notReset)
	{
		placePlayer ();	
		placeFinish ();	
		if (notReset)
			placeEnemies ();
			
	}
		
	//Places the Player in a suitable spot on the map.
	private void placePlayer ()
	{
		int[] pos = findSuitable (currentLevel.getPlayerPos ());
		player = Instantiate (playerPrefab, new Vector3 (pos [0], pos [1], 0), Quaternion.identity) as GameObject;	
	}
		
	private void placeEnemies ()
	{
		List<int[]> aux = currentLevel.getEnemies ();
			
			
		for (int i = 0; i<aux.Count; i++) {
			int[] pos = findEmptyPos ((int[])aux [i]);
			Instantiate (enemy, new Vector3 (pos [0] * 10, pos [1] * 10, 0), Quaternion.identity);
		}	
	}
		
	private void placeFinish ()
	{		
			
		int[] pos = findSuitable (currentLevel.getFinishPos ());
		Vector2 finalPos = new Vector2 (pos [0], pos [1]);		
		finish = Instantiate (finishPrefab, finalPos, Quaternion.identity) as GameObject;
	}
		
	//Finds a SUITABLE LOCATION for the PLAYER and the FINISH.
	private int[] findSuitable (int[] center)
	{
		int[] returning = {0,0};
		//Calls all the ground prefabs in the game
		GameObject[] walls = findPositionSeed (center);
		bool unsuitable = true;
			
			
		while (unsuitable) {
			if (walls.Length == 0) {
				return findSuitable (getRandomBlock ());			
			}
			//Gets a RANDOM ground tile.
			int aux = (int)Random.Range (0, walls.Length - 1);
			//Takes the aux`s position and creates a new variable exactly on top of it.
			Vector2 newPos = new Vector2 (walls [aux].transform.position.x, walls [aux].transform.position.y + 10);
				
			//Finds the closest collider to the right. If its 5, the new position is inside a wall.
			RaycastHit2D distanceToRightWall = Physics2D.Raycast (newPos, Vector2.right);
			bool isNotInside = distanceToRightWall.distance > 5.1;
			if (isNotInside) {
				unsuitable = false;		
				returning [0] = (int)newPos.x;
				returning [1] = (int)newPos.y;			
			}
		}	
			
			
		return returning;
	}
		
	private void clearPlayerFinish ()
	{
		Destroy (player);
		Destroy (finish);
		player = null;
		finish = null;
		Destroy (GameObject.FindGameObjectWithTag ("Player"));
		Destroy (GameObject.FindGameObjectWithTag ("Finish"));

			
	}

	private void clearEnemies ()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
			
		for (int i = 0; i<enemies.Length; i++) {
			Destroy (enemies [i]);			
		}	
	}

	private void clearWalls ()
	{
		GameObject[] ground = GameObject.FindGameObjectsWithTag ("Terrain");
		
		for (int i = 0; i<ground.Length; i++) {
			Destroy (ground [i]);			
		}
		GameObject[] terrain = GameObject.FindGameObjectsWithTag ("Wall");
		
		for (int i = 0; i<terrain.Length; i++) {
			Destroy (terrain [i]);			
		}

	}
		
	private void clearAll ()
	{
		clearPlayerFinish ();
		clearEnemies ();
		clearWalls ();	
	}
		
		
		
		
		
		
		
		
	//End of V3
	private void remakePositions ()
	{

		clearPlayerFinish ();
		clearEnemies ();
		createEntities (true);	
	}
		
		
		
		
		
		
		
		
	//Start of V4
	private bool makeUsable ()
	{
		int counter = -1;

		bool isUsable = true;
		while (isUsable) {
			if (!startDijkstra ()) {
				counter++;
				if (counter < 10)					
					remakePositions ();
				else {
					return false;
				}
			} else
				isUsable = false;
		}
		return true;
			
	}
		
	private bool startDijkstra ()
	{
			
			
		toVisit.Add (new Vector2 (player.transform.position.x, player.transform.position.y));
			
			
		while (toVisit.Count>0) {
				
			Vector2 aux = (Vector2)toVisit [findClosest ()];
			if (fillAdjacent (aux)) {
				toVisit.Clear ();
				return true;
			}
		}
		return false;
	}
		
	private bool fillAdjacent (Vector2 current)
	{
		visited.Add (current);
		toVisit.Remove (current);
			
		if (isFinish (current))
			return true;
			
		Vector2 aux = new Vector2 (current.x + 10, current.y);
			
		if (ifHelper (aux))
			return true;
			
		aux = new Vector2 (current.x, current.y + 10);
		if (ifHelper (aux))
			return true;
			
		aux = new Vector2 (current.x - 10, current.y);
		if (ifHelper (aux))
			return true;
			
		aux = new Vector2 (current.x, current.y - 10);
		if (ifHelper (aux))
			return true;
			
		return false;
			
	}
		
	private int findClosest ()
	{
		int returned = 0;
		float aux = Mathf.Infinity;
		Vector2 finishpoint = new Vector2 (finish.transform.position.x, finish.transform.position.y);
		for (int i = 0; i<toVisit.Count; i++) {
			if (Vector2.Distance ((Vector2)toVisit [i], finishpoint) < aux) {
				returned = i;
				aux = Vector2.Distance ((Vector2)toVisit [i], finishpoint);
			}
		}
		return returned;	
	}
		
	private bool ifHelper (Vector2 aux)
	{
		if (isEmpty (aux) && ! visited.Contains (aux) && !toVisit.Contains (aux) || isFinish (aux)) {
			if (isFinish (aux))
				return true;
			toVisit.Add (aux);
		}
		return false;
	}
		
	private bool isFinish (Vector2 current)
	{
		RaycastHit2D hit = Physics2D.Raycast (current, Vector2.right, 5f);
			
		if (isEmpty (current))
			return false;
			
		if (hit.collider.tag.Equals ("Finish"))
			return true;
			
		return false;
			
	}

	//End of V4
	private bool isEmpty (Vector2 current)
	{
		RaycastHit2D hit = Physics2D.Raycast (current, Vector2.right, 5f);
		if (hit.collider == null)
			return true;
		hit = Physics2D.Raycast (current, Vector2.left, 5f);
		if (hit.collider == null)
			return true;
		return false;		
	}

	private bool isEmpty (int[] currentCenter)
	{
		Vector2 current = new Vector2 (currentCenter [0] * 10, currentCenter [1] * 10);
			
			
		RaycastHit2D hit = Physics2D.Raycast (current, Vector2.right, 5f);
		if (hit.collider == null)
			return true;
		hit = Physics2D.Raycast (current, Vector2.left, 5f);
		if (hit.collider == null)
			return true;
		return false;	
			
			
	}
					
	//TODO
	private void createSeed ()
	{
		Level chosen = new Level ();
		List<int[]> lp = new List<int[]> ();		
		List<int[]> lc = new List<int[]> ();
		List<int[]> le = new List<int[]> ();			

		for (int i = 0; i<horizontalWalls; i++) {
		
			int[] randomBlock = getRandomBlock ();
			int wallsize = (int)Random.Range (wallMinSize, wallMaxSize);
			
			int[] aux = {randomBlock [0],randomBlock [1],wallsize};
			lp.Add (aux);		
		
		}

		for (int i = 0; i<verticalWalls; i++) {

				
			int wallsize = (int)Random.Range (wallMinSize, wallMaxSize);

			int[] randomBlock = getRandomBlock ();
			int[] aux1 = {randomBlock [0],randomBlock [1],wallsize};
			lc.Add (aux1);

		}

		for (int i = 0; i<numberOfEnemies; i++) {

			int[] randomBlock = getRandomBlock ();

			int[] aux2 = {randomBlock [0],randomBlock [1]};
			le.Add (aux2);
		}

		chosen.setPositions (getRandomBlock (), getRandomBlock ());

		chosen.setEnemies (le);
			
		chosen.setLines (lp, lc);
			
		currentLevel = chosen;
	}
		
	private GameObject[] findPositionSeed (int[] centerPos)
	{
		ArrayList auxReturn = new ArrayList ();
			
		GameObject[] walls = GameObject.FindGameObjectsWithTag ("Terrain");
			
		for (int i = 0; i < walls.Length; i ++) {
			int cX = (int)walls [i].transform.position.x;
			int cY = (int)walls [i].transform.position.y;
			if (cX < centerPos [0] * 10 + 300 && cX > centerPos [0] * 10 - 300)
			if (cY < centerPos [1] * 10 + 300 && cY > centerPos [1] * 10 - 300)
				auxReturn.Add (walls [i]);		
		}
			
		GameObject[] returning = new GameObject[auxReturn.Count];
			
		for (int i = 0; i < auxReturn.Count; i ++) {
			returning [i] = (GameObject)auxReturn [i];
		}
			
		return returning;			
	}

	private int[] findEmptyPos (int[] center)
	{
		if (isEmpty (center))
			return center;
		int[] returning = {0,0};
		int counter = 1;
		bool unsuitable = true;
			
			
		while (unsuitable) {
			switch (counter % 8) {
			case 0:
				returning [0] = center [0] + 10 * counter; 
				returning [1] = center [1]; 
				if (isEmpty (returning))
					return returning;
				break;
					
			case 1:
				returning [0] = center [0]; 
				returning [1] = center [1] + 10 * counter; 
				if (isEmpty (returning))
					return returning;
				break;
					
			case 2:
				returning [0] = center [0] - 10 * counter; 
				returning [1] = center [1]; 
				if (isEmpty (returning))
					return returning;
				break;
					
			case 3:
				returning [0] = center [0]; 
				returning [1] = center [1] - 10 * counter; 
				if (isEmpty (returning))
					return returning;
				break;
					
			case 4:
				returning [0] = center [0] + 10 * counter; 
				returning [1] = center [1] - 10 * counter; 
				if (isEmpty (returning))
					return returning;
				break;
					
			case 5:
				returning [0] = center [0] - 10 * counter; 
				returning [1] = center [1] + 10 * counter; 
				if (isEmpty (returning))
					return returning;
				break;
					
			case 6:
				returning [0] = center [0] - 10 * counter; 
				returning [1] = center [1] - 10 * counter; 
				if (isEmpty (returning))
					return returning;
				break;
					
			case 7:
				returning [0] = center [0] + 10 * counter; 
				returning [1] = center [1] + 10 * counter; 
				if (isEmpty (returning))
					return returning;
				break;
			}
				
			counter++;
		}
			
		return null;
	}
		
		
		
		
		
		
	//End of V5
		
	//Creation of List
	public List<Level> createNewList (int size)
	{
					
		List<Level> returning = new  List<Level> ();
			
		for (int i = 0; i<size; i++) {
			returning.Add (createAndTest ());			
		}
			
		return returning;	
	}
		
	private Level createAndTest ()
	{
		createLevel ();
		Level returning = currentLevel;
		currentLevel = null;
		clearAll ();
		return returning;
			
	}

	public Level createNew (Level best, Level pair)
	{
		
		Level next = new Level ();
		List<int[]> lp = new List<int[]> ();		
		List<int[]> lc = new List<int[]> ();
		List<int[]> le = new List<int[]> ();			
		int[] pPos;
		int[] fPos;



		int bestLineNumber = Mathf.FloorToInt(horizontalWalls * 0.7f);
		int pairLineNumber = Mathf.FloorToInt(horizontalWalls * 0.2f);
		int randomLineNumber = Mathf.FloorToInt(horizontalWalls * 0.1f);

		lp.AddRange (getNPositions (best.getLine (), bestLineNumber));
		lp.AddRange (getNPositions (pair.getLine (), pairLineNumber));
		lp.AddRange (getNPositonsRandom(randomLineNumber,true));



		int bestColumnNumber = Mathf.FloorToInt(verticalWalls * 0.7f);
		int pairColumnNumber = Mathf.FloorToInt(verticalWalls * 0.2f);
		int randomColumnNumber = Mathf.FloorToInt(verticalWalls * 0.1f);
		
		lc.AddRange (getNPositions (best.getCollum (), bestColumnNumber));
		lc.AddRange (getNPositions (pair.getCollum (), pairColumnNumber));
		lc.AddRange (getNPositonsRandom(randomColumnNumber,true));

		int bestEnemiesNumber = Mathf.FloorToInt(verticalWalls * 0.7f);
		int pairEnemiesNumber = Mathf.FloorToInt(verticalWalls * 0.2f);
		int randomEnemiesNumber = Mathf.FloorToInt(verticalWalls * 0.1f);
		
		le.AddRange (getNPositions (best.getEnemies (), bestEnemiesNumber));
		le.AddRange (getNPositions (pair.getEnemies (), pairEnemiesNumber));
		le.AddRange (getNPositonsRandom(randomEnemiesNumber,false));






		if (getFromBest ()) {			 
			pPos = best.getPlayerPos ();	
		} else
		if (getFromPair ()) {
			pPos = pair.getPlayerPos ();
		} else
			pPos = getRandomBlock ();

		if (getFromBest ()) {
			fPos = best.getFinishPos ();
		} else
		if (getFromPair ()) {
			fPos = pair.getFinishPos ();
		} else
			fPos = getRandomBlock ();


		
		next.setPositions (pPos, fPos);
		
		next.setEnemies (le);
		
		next.setLines (lp, lc);
		
		currentLevel = next;

		return createAndTest ();

	}

	private List<int[]> getNPositions (List<int[]> list, int n)
	{
	
		List<int[]> ret = new List<int[]> ();

		for (int i = 0; i<n; i++) {
			int num = (int)Random.Range (0, list.Count);

			int[] aux = list [num];

			if (ret.Contains (aux))
				i--;
			else
				ret.Add (aux);
		}
		return ret;	
	}

	// IF b, int[3], else int[2]
	private List<int[]> getNPositonsRandom (int n, bool b)
	{
		
		List<int[]> ret = new List<int[]> ();
		
		for (int i = 0; i<n; i++) {
			if (b) {
				ret.Add (getRandomBlockStart ());
			} else {
				ret.Add (getRandomBlock ());
			}
		}
		return ret;	
	}
	
	private bool getFromBest ()
	{
		return Random.value > 0.25;
	}
	
	private bool getFromPair ()
	{
		return Random.value > 0.25;
	}














}