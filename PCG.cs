using UnityEngine;
using System.Collections;

public class PCG : MonoBehaviour {

	[SerializeField]
	private GameObject wallprefab;
	[SerializeField]
	private int matrixSize = 100;


	void Start () {
		GameObject[,] matrix = new GameObject[matrixSize,matrixSize];
	
		for (int m = 0; m<matrixSize; m++) {
			for (int n = 0; n<matrixSize; n++) {
				bool wall = Random.value>=0.7;
				if(m==0 || m == matrixSize-1 || n==0 || n == matrixSize-1 )
					wall = true;


				if(wall){
				Instantiate (wallprefab,new Vector3(m*10,n*10,0),Quaternion.identity);	
				}
			
			}
		}
	}
	

}
