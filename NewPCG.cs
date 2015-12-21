using UnityEngine;
using System.Collections;


public class NewPCG : MonoBehaviour
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
		matrixSize = 100;
	[SerializeField]
	private int
		horizontalWalls = 10;
	[SerializeField]
	private int
		verticalWalls = 10;
	[SerializeField]
	private int
		wallMaxSize = 10;
	[SerializeField]
	private int
		wallMinSize = 5;

	//End of Generator V2.
	
	//Start of Generator V3.
	[SerializeField]
	private GameObject
		player;
	[SerializeField]
	private GameObject
		enemy;
	[SerializeField]
	private float
		distanceToPlayer = 50f;
	[SerializeField]
	private int
		numberOfEnemies;
	[SerializeField]
	private GameObject
		finish;
	//End of Generator V3.
	
	private ArrayList toVisit = new ArrayList ();
	private ArrayList visited = new ArrayList ();
	

	//Level for Loading
	private Level currentLevel;


	
	
	
	//Auxiliary, for easier testing.
	private float cooldown = 1;
	
	void Start ()
	{	
		//Calls the function that oversees terrain creation. Generator V2.
		createTerrain ();
		
		createEntities (true);
		
		makeUsable ();
		
	}
	
	void reset ()
	{
		Application.LoadLevel (Application.loadedLevel);
	}
	
	//Start of V2.
	
	//Creates the Terrain for the Level. Generator V2.
	private void createTerrain ()
	{
		findSeed ();

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
	private int[] getRandomBlock ()
	{
		int[] returning = {0,0};
		returning [0] = (int)Random.Range (1, matrixSize - 2);
		returning [1] = (int)Random.Range (1, matrixSize - 2);	
		return returning;
	}
	
	//Oversees the creation of HORIZONTAL walls.
	private void createHorizontal ()
	{
		ArrayList lineStarts = currentLevel.getLine ();
		for (int h = 0; h<lineStarts.Count; h++) {

			int [] aux = (int[])lineStarts[h]; 
			buildingLine (aux [0], aux [1],aux [2]);				
		}	
	}
	
	//Oversees the creation of VERTICAL walls.
	private void createVertical ()
	{
		ArrayList verticalStarts = currentLevel.getCollum();
		for (int v = 0; v<verticalStarts.Count; v++) {

			int [] aux = (int[]) verticalStarts[v]; 
			buildingVertical (aux [0], aux [1],aux[2]);				
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
		int[] pos = findSuitable (currentLevel.getPlayerPos());
		player = Instantiate (player, new Vector3 (pos [0], pos [1], 0), Quaternion.identity) as GameObject;	
	}
	
	private void placeEnemies ()
	{
		ArrayList aux = currentLevel.getEnemies ();


		for (int i = 0; i<aux.Count; i++) {
			int[] pos = findEmptyPos ((int[])aux[i]);
			Instantiate (enemy, new Vector3 (pos [0]*10, pos [1]*10, 0), Quaternion.identity);
		}	
	}
	
	private void placeFinish ()
	{		

		int[] pos = findSuitable (currentLevel.getFinishPos());
		Vector2 finalPos = new Vector2 (pos [0], pos [1]);		
		finish = Instantiate (finish, finalPos, Quaternion.identity) as GameObject;
	}
	
	//Finds a SUITABLE LOCATION for the PLAYER and the FINISH.
	private int[] findSuitable (int[] center)
	{
		int[] returning = {0,0};
		//Calls all the ground prefabs in the game
		GameObject[] walls = findPositionSeed (center);
		bool unsuitable = true;


		while (unsuitable) {
			if (walls.Length==0)
				reset();
			//Gets a RANDOM ground tile.
			int aux = (int)Random.Range (0, walls.Length);
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
	}
	
	
	
	//End of V3
	
	//Start of V4
	private void makeUsable ()
	{
		bool isUsable = true;
		while (isUsable) {
			if (!startDijkstra ()) {
				Debug.Log ("nope");
				reset ();			
			} else
				isUsable = false;
			isUsable = false;
		}
		
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
	
	//End of V4

	private bool isEmpty(int[] currentCenter){
		Vector2 current = new Vector2 (currentCenter[0]*10,currentCenter[1]*10);


		RaycastHit2D hit = Physics2D.Raycast (current, Vector2.right, 5f);
		if (hit.collider == null)
			return true;
		hit = Physics2D.Raycast (current, Vector2.left, 5f);
		if (hit.collider == null)
			return true;
		return false;	
	
	
	}


	
	
	
	
	//TODO
	private void findSeed ()
	{
		Level chosen = new Level ();
		ArrayList lp = new ArrayList ();		
		ArrayList lc = new ArrayList ();
		ArrayList le = new ArrayList ();


		for(int i = 0; i<30;i++){
			int[] randomBlock = getRandomBlock();
			int[] aux = {randomBlock[0],randomBlock[1],40};
			lp.Add(aux);

			randomBlock = getRandomBlock();
			int[] aux1 = {randomBlock[0],randomBlock[1],15};
			lc.Add(aux1);

			randomBlock = getRandomBlock();
			if(i%3==0){
				int[] aux2 = {randomBlock[0],randomBlock[1]};
				le.Add(aux2);
			
			}


		}
		chosen.setPositions (getRandomBlock(),getRandomBlock());
		chosen.setEnemies (le);

		chosen.setLines (lp,lc);

		currentLevel = chosen;

	}








	/*
	//Oversees creation of Horizontal lines, using the seed.
	private void createHorizontalSeed (ArrayList horizontals)
	{
		for (int i = 0; i < horizontals.Count; i +=2) {
			wallCurrentSize = (int)horizontals [i + 1];
			Vector2 newLine = (Vector2)horizontals [i];
			int[] xy = {(int)newLine.x,(int)newLine.y};
			buildingLine (xy [0], xy [1]);
		}
		
		
	}
	//Oversees creation of Vertical lines, using the seed.
	private void createVerticalSeed (ArrayList verticals)
	{
		for (int i = 0; i < verticals.Count; i +=2) {
			wallCurrentSize = (int)verticals [i + 1];
			Vector2 newLine = (Vector2)verticals [i];
			int[] xy = {(int)newLine.x,(int)newLine.y};
			buildingVertical (xy [0], xy [1]);
		}		
	}
	*/

	
	private GameObject[] findPositionSeed (int[] centerPos)
	{
		ArrayList auxReturn = new ArrayList ();
		
		GameObject[] walls = GameObject.FindGameObjectsWithTag ("Terrain");

		for (int i = 0; i < walls.Length; i ++) {
			int cX = (int)walls [i].transform.position.x;
			int cY = (int)walls [i].transform.position.y;
			if (cX < centerPos [0]*10 + 300 && cX > centerPos [0]*10 - 300)
				if (cY < centerPos [1]*10 + 300 && cY > centerPos [1]*10 - 300)
					auxReturn.Add (walls [i]);		
		}

		GameObject[] returning = new GameObject[auxReturn.Count];

		for (int i = 0; i < auxReturn.Count; i ++) {
			returning[i] = (GameObject)auxReturn[i];
		}

		return returning;			
	}
	private int[] findEmptyPos(int[] center){
		if (isEmpty (center))
			return center;
		int[] returning = {0,0};
		int counter = 1;
		bool unsuitable = true;


		while (unsuitable) {
			switch(counter % 8)
			{
			case 0 :    returning[0] = center[0]+10*counter; 
				    	returning[1]  = center[1]; 
				if(isEmpty(returning))
					return returning;
				break;
				
			case 1 :
				returning[0] = center[0]; 
				returning[1]  = center[1]+10*counter; 
				if(isEmpty(returning))
					return returning;
				break;
				
			case 2 :
				returning[0] = center[0]-10*counter; 
				returning[1]  = center[1]; 
				if(isEmpty(returning))
					return returning;
				break;
				
			case 3 :
				returning[0] = center[0]; 
				returning[1]  = center[1]-10*counter; 
				if(isEmpty(returning))
					return returning;
				break;
				
			case 4 :
				returning[0] = center[0]+10*counter; 
				returning[1]  = center[1]-10*counter; 
				if(isEmpty(returning))
					return returning;
				break;
				
			case 5 :
				returning[0] = center[0]-10*counter; 
				returning[1]  = center[1]+10*counter; 
				if(isEmpty(returning))
					return returning;
				break;
				
			case 6 :
				returning[0] = center[0]-10*counter; 
				returning[1]  = center[1]-10*counter; 
				if(isEmpty(returning))
					return returning;
				break;
				
			case 7 :
				returning[0] = center[0]+10*counter; 
				returning[1]  = center[1]+10*counter; 
				if(isEmpty(returning))
					return returning;
				break;
			}
			Debug.Log(returning);
		
			counter++;
		}

		return null;
	}

	
	
	
	
	
	//End of V5
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	void FixedUpdate ()
	{
		
		
	
		Vector3 newPos = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
		transform.position = Vector3.Lerp (transform.position, newPos, Time.deltaTime * 100);
		
		
		
		cooldown -= 0.02f;
		HandleInput ();
	}
	
	void HandleInput ()
	{
		if (Input.GetAxis ("Fire2") == 1)
			if (cooldown < 0)
				reset ();
	}
	
	
	
	
	
	
}
