using UnityEngine;
using System.Collections;

public class LevelGrammar : MonoBehaviour
{

	[SerializeField]
	private int[,]
		levelMap;

	public LevelGrammar (int[,] levelMap)
	{
		this.levelMap = levelMap;
	}

	public void setMap (int[,] pLevelMap)
	{
		levelMap = pLevelMap;
	}

	public int[,] getMap ()
	{
		return levelMap;
	}
}
