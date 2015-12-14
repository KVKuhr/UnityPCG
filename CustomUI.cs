using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CustomUI : MonoBehaviour {

	[SerializeField]
	private Text lifeBar;

	private int maxHealth;
	private int currentHealth;


	public void startUp(int max ){
		maxHealth = max;
		currentHealth = max;
		updateText ();
	}

	public void gotHit(){
		currentHealth--;
		updateText ();
	}

	private void updateText(){
		lifeBar.text = "Health : " + currentHealth + "/" + maxHealth;

	}

}
