using UnityEngine;
using System.Collections;

public class Rules : MonoBehaviour
{
	private int[] origin;
	private int[] originExit;
	private Room[] gRule;
	private int chance;


	public Rules (Room[] initialRule, int pChance, int[] pOrigin, int[] pOriginExit)
	{
		originExit = pOriginExit;
		origin = pOrigin;
		gRule = initialRule;
		chance = pChance;

	}
	public int getChance(){
		return chance;
	}

	public Room[] getRule ()
	{
		return gRule;
	}
	public int[] getOrigins(){
		return origin;
	}
	public int[] getOriginsE(){
		return originExit;
	}
	public bool isOrigin(int r){
		foreach (int i in origin)
			if (i == r)
				return true;
		return false;
	}
	public bool isExit(int e){
		foreach (int i in originExit)
			if (i == e)
				return true;
		return false;
	}





}
