using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CustomUI : MonoBehaviour {

	[SerializeField]
	private Text lifeBar;

	[SerializeField]
	private Canvas pauseMenu;

	[SerializeField]
	private Canvas evaluateCanvas;



	private int maxHealth;
	private int currentHealth;

	private PCGControler control;





	public void startUp(int max ){

		control = GameObject.FindGameObjectWithTag ("ControlHolder").GetComponent<PCGControler>();

		pauseMenu.enabled = false;
		evaluateCanvas.enabled = false;
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
		Time.timeScale = 1.0f;
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
	public void openEvaluate(){
		Time.timeScale = 0.0f;
		evaluateCanvas.enabled = true;
	}


	public void dislikeLevel(){

		control.dislikeLevel ();
		exitPress ();

	}

	public void likeLevel(){

		control.likeLevel ();
		exitPress ();
	}






}
