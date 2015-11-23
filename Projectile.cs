using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {

	private float pSpeed;
	private Rigidbody2D pRigid;
	private Vector2 direction;


	public void setDirection(Vector2 target, float force){
		pRigid = GetComponent<Rigidbody2D>();
		direction = target;
		pSpeed = force;
		handleTarget ();
	}




	void OnBecameInvisible(){
		Destroy (gameObject);
	}
	private void handleTarget(){
			if(direction.x<0)
			pRigid.velocity = -transform.right * pSpeed;
			else
			pRigid.velocity = transform.right * pSpeed;
	}

	void OnTriggerEnter2D (Collider2D other){
		if (tag.Equals("Projectile")) {
			if (!other.tag.Equals ("Player"))
				Destroy (gameObject);
		} else {			    
			if (!other.tag.Equals ("Enemy"))
				Destroy (gameObject);
		}
	}




}
