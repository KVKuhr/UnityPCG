using UnityEngine;
using System.Collections;

public class PCG : MonoBehaviour
{

	[SerializeField]
	private GameObject
		wallprefab;
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
	private int wallCurrentSize;

	private float cooldown = 5;

	void Start ()
	{
			
		createOuterBounds ();

		createHorizontal ();
		createVertical ();



	}

	private void createOuterBounds ()
	{
		for (int m = 0; m<matrixSize; m++) {
			Instantiate (wallprefab, new Vector3 (m * 10, 0, 0), Quaternion.identity);		
			Instantiate (wallprefab, new Vector3 (m * 10, 10 * matrixSize - 1, 0), Quaternion.identity);
			Instantiate (wallprefab, new Vector3 (0, m * 10, 0), Quaternion.identity);		
			Instantiate (wallprefab, new Vector3 (10 * matrixSize - 1, m * 10, 0), Quaternion.identity);
		
		}
	}

	private void getNewSize ()
	{
		int wallsize = (int)Random.Range (wallMinSize, wallMaxSize);
		wallCurrentSize = wallsize;	
	}

	private void buildingLine (int m, int n)
	{

		for (int l = m; l<matrixSize && wallCurrentSize > 0; l++) {
			Instantiate (wallprefab, new Vector3 (l * 10, n * 10, 0), Quaternion.identity);
			wallCurrentSize--;
		}			

	}

	private void buildingVertical (int m, int n)
	{	
		for (int l = n; l<matrixSize && wallCurrentSize > 0; l++) {
			Instantiate (wallprefab, new Vector3 (m * 10, l * 10, 0), Quaternion.identity);
			wallCurrentSize--;
		}			

	}

	private int[] getRandomBlock ()
	{
		int[] returning = {0,0};
		returning [0] = (int)Random.Range (1, matrixSize);
		returning [1] = (int)Random.Range (1, matrixSize);	
		return returning;
	}

	private void createHorizontal(){
		for (int h = 0; h<horizontalWalls; h++) {
			getNewSize ();
			int [] aux = getRandomBlock (); 
			buildingLine (aux [0], aux [1]);				
		}
	
	
	}
	private void createVertical(){
		for (int v = 0; v<verticalWalls; v++) {
			getNewSize ();
			int [] aux = getRandomBlock (); 
			buildingVertical (aux [0], aux [1]);				
		}
		
		
	}


	void FixedUpdate ()
	{
		cooldown -= 0.02f;
		HandleInput ();
	}

	void HandleInput ()
	{
		if (Input.GetAxis ("Jump") == 1)
		if (cooldown < 0)
			Application.LoadLevel (Application.loadedLevel);
	}





}
