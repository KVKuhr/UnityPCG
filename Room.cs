using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	[SerializeField]
	private int number;
	
	[SerializeField]
	private GameObject[] exitList;

	[SerializeField]
	private Room[] nextRoom = new Room[4];



	public Room getRoom(int i){
		return nextRoom [i];
	}
	public void setRoom(int i,Room next){
		nextRoom [i] = next;	
	}

	public int getInt(){
		return number;
	}
	public GameObject[] getExitList(){
		return exitList;
	}

	public void destroyExit(int n){
		Destroy(exitList[n]);
	}
}
