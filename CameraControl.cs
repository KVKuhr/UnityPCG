﻿using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	[SerializeField]
	private GameObject target;
	[SerializeField]
	private float speed;


	void FixedUpdate () {



		Vector3 newPos = new Vector3(target.transform.position.x,target.transform.position.y,transform.position.z);
		transform.position = Vector3.Lerp (transform.position, newPos, Time.deltaTime *speed);
		


	}

}
