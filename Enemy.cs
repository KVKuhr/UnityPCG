using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {

	private Rigidbody2D rigid;

	[SerializeField]
	private float aSpeed = 1;
	private float cooldown;

	[SerializeField]
	private GameObject
		player;

	[SerializeField]
	private GameObject
		projectile;

	[SerializeField]
	private float
		forceProjectile;
	[SerializeField]
	private int
		hp;


	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");

		cooldown = aSpeed;
		rigid = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		cooldown -= Time.fixedDeltaTime;

		if (cooldown <= 0) {
			attackPlayer();
			cooldown = aSpeed;
		
		}
	}

	private void attackPlayer() {
		Vector3 aux = player.transform.position;
		Vector2 relativePos = aux - transform.position;
		Quaternion rotation = Quaternion.LookRotation (relativePos);
		rotation.x = 0;
		rotation.y = 0;		 

		GameObject clone = Instantiate (projectile, rigid.transform.position, rotation) as GameObject;

		clone.GetComponent<Projectile> ().setDirection (relativePos, forceProjectile);


	}
	void OnTriggerEnter2D (Collider2D other){


		if (other.tag.Equals ("Projectile")){
			gotHit();
			Debug.Log ("hit");
		}
	}

	private void gotHit ()
	{
		hp--;
		if (hp == 0)
			Destroy(gameObject);
		
	}


}
