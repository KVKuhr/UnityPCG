using UnityEngine;
using System.Collections;

public class PixelPerfectCamera : MonoBehaviour {

	public static float p2u = 1.5f;
	public static float scale = 1f;
	private float originalSize;

	public Vector2 nres = new Vector2(240,160);

	void Awake () {


		var camera = GetComponent<Camera> ();
		originalSize = camera.orthographicSize;
		if (camera.orthographic) {
			scale = Screen.height / nres.y;
			p2u = scale;
			camera.orthographicSize = (Screen.height / 2.0f) / p2u;

		
		}
	}
	void OnDestroy(){
		var camera = GetComponent<Camera> ();
		camera.orthographicSize = originalSize;
		
		
	}
}
