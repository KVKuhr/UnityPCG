using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {
	[SerializeField]
	private Canvas quitMenu;
	[SerializeField]
	private Button startButton;
	[SerializeField]
	private Button exitButton;
	[SerializeField]
	private GameObject controlHolder;

	private PCGControler control;

	// Use this for initialization
	void Start () {
		control = controlHolder.GetComponent<PCGControler>();
		quitMenu = quitMenu.GetComponent<Canvas>();
		startButton = startButton.GetComponent<Button>();
		exitButton = exitButton.GetComponent<Button>();
		quitMenu.enabled = false;

	}
	
	public void exitPress(){
		quitMenu.enabled = true;
		startButton.enabled = false;
		exitButton.enabled = false;
	}

	public void noPress(){
		quitMenu.enabled = false;
		startButton.enabled = true;
		exitButton.enabled = true;
	}
	public void yesPress(){
		Application.Quit();
	
	}

	public void startPCGLevel(){
		control.startPCGLevel ();
	
	}

	public void StartLevel(){
		control.startLevel();
	}

	public void startPCGSeed(){
		control.startPCGSeed ();
	}
}
