using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour
{
	[SerializeField]
	private Canvas
		quitMenu;
	[SerializeField]
	private Button
		startButton;
	[SerializeField]
	private Button
		exitButton;
	[SerializeField]
	private GameObject
		controlHolder;
	[SerializeField]
	private Canvas
		levelCanvas;
	private PCGControler control;
	private string errorMsg = "";
	private int l1;
	private int l2;
	private int remove;
	private int choice;
	[SerializeField]
	private Text[]
		levelStats;
	[SerializeField]
	private Text
		chosenText;
	[SerializeField]
	private Text
		statsText;

	[SerializeField]
	private Text
		createButtonText;


	private bool isManual = false;

	private bool createNew = true;
	[SerializeField]
	private Graphic[] createList;
	[SerializeField]
	private Toggle autoToggle;

	// Use this for initialization
	void Start ()
	{
		control = controlHolder.GetComponent<PCGControler> ();
		quitMenu = quitMenu.GetComponent<Canvas> ();
		startButton = startButton.GetComponent<Button> ();
		exitButton = exitButton.GetComponent<Button> ();
		quitMenu.enabled = false;
		levelCanvas.enabled = false;
		setStatsTexts ();
		clearValues ();
		openCloseCreate ();
	}
	
	public void exitPress ()
	{
		quitMenu.enabled = true;
		startButton.enabled = false;
		exitButton.enabled = false;
	}

	public void noPress ()
	{
		quitMenu.enabled = false;
		startButton.enabled = true;
		exitButton.enabled = true;
	}

	public void yesPress ()
	{
		Application.Quit ();
	
	}

	//Starts level AC
	public void startPCGLevel ()
	{
		control.startPCGLevel ();
	
	}
	//Starts level AA
	public void StartLevel ()
	{
		control.startLevel ();
	}
	//Enables the level Canvas
	public void openLevelCanvas ()
	{
		setStatsTexts ();
		updateHelperText ();
		updateChosenText ();
		levelCanvas.enabled = !levelCanvas.enabled;
	}

	//Starts AD
	public void choseLevel ()
	{
		if (choice > -1) {
			errorMsg = "";
			control.startPCGSeed (choice-1);
		} else
			errorMsg = "You must first choose a level.";
	}

	private void displayError ()
	{
	}

	public void createNewLevel ()
	{
		if (l1 != -1 && l2 != -1 && remove != -1)
			control.createNew (l1-1, l2-1, remove-1, isManual);
	}

	private void updateChosenText ()
	{
		string t1;

		if (choice != -1)
			t1 = choice + "";
		else
			t1 = "none";

		chosenText.text = "Chosen: " + t1 + ".";	
	}

	private void updateHelperText ()
	{
		string t1;
		string t2;
		string rem;

		if (l1 != -1)
			t1 = l1 + "";
		else
			t1 = "none";

		
		if (l2 != -1)
			t2 = l2 + "";
		else
			t2 = "none";
		
		if (remove != -1)
			rem = remove + "";
		else
			rem = "none";

		statsText.text = "Primary: " + t1 + " Pair: " + t2 + " Remove: " + rem + ".";
	}

	public void clearValues ()
	{		
		l1 = -1;
		l2 = -1;
		remove = -1;
		updateHelperText ();
	}

	private void setStatsTexts ()
	{
		if (!control.isListEmpty ())
			for (int i = 0; i<5; i++) {
				levelStats [i].text = control.getStat (i);		
			}
	}

	public void button1IsPressed(){
		manageSelections (1);
	}
	public void button2IsPressed(){
		manageSelections (2);
	}
	public void button3IsPressed(){
		manageSelections (3);
	}
	public void button4IsPressed(){
		manageSelections (4);
	}
	public void button5IsPressed(){
		manageSelections (5);
	}






	private void manageSelections (int x)
	{
		if (createNew) {
			if (l1 == -1)
				l1 = x;
			else {
				if (l2 == -1)
					l2 = x;
				else
					remove = x;		
			}
			updateHelperText ();
		} else {
			choice = x;
			updateChosenText();
		}
	}
	public void changeMode(){
		isManual = !isManual;
	}

	public void openCloseCreate(){
		autoToggle.enabled = !autoToggle.enabled;
		createNew = !createNew;
		foreach(Graphic g in createList){
			g.enabled = !g.enabled;	}
		if (createNew)
			createButtonText.text = "Close Create";
		else
			createButtonText.text = "Create New";

	}

	public void makeList(){
		control.makeList ();
	}



}
