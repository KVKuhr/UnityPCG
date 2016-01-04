using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CustomUI : MonoBehaviour {

	[SerializeField]
	private Text lifeBar;

	[SerializeField]
	private Canvas pauseMenu;


	private int maxHealth;
	private int currentHealth;


	public void startUp(int max ){


		pauseMenu.enabled = false;
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

	public void exitPress(){

		Application.LoadLevel ("menu");
	}

	public void openPause(){
		Time.timeScale = 0.0f;
		pauseMenu.enabled = true;
	}
	public void closePause(){


		Time.timeScale = 1.0f;
		pauseMenu.enabled = false;

	}
	public void escPressed()
	{
		if(pauseMenu.isActiveAndEnabled)
				closePause();
			else
				openPause();
	}





}
