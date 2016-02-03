using UnityEngine;
using System.Collections;

public class ExitScript : MonoBehaviour
{



	void OnTriggerEnter2D (Collider2D other)
	{	
		if (other.tag.Equals ("Exit")) {
			GameObject.Destroy (other.gameObject);
			GameObject.Destroy (gameObject);
		}

	}

}