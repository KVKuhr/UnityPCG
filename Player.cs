using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

	[SerializeField]
	private float
		mspeed = 10;
	[SerializeField]
	private float
		accel = 10;
	[SerializeField]
	private Transform[]
		groundPoints = new Transform[3];
	private bool isJumping;
	private bool isGrounded;
	[SerializeField]
	private float
		jumpForce;
	private bool attack;
	private Rigidbody2D rigid;
	private bool facingRight;
	private Animator myAnimator;
	[SerializeField]
	private float
		groundRadius;
	[SerializeField]
	private LayerMask
		whatIsGround;
	[SerializeField]
	private GameObject
		projectile;
	[SerializeField]
	private float
		forceProjectile;
	[SerializeField]
	private int
		hp;
	[SerializeField]
	private float
		aSpeed = 1;
	private float cooldown;
	[SerializeField]
	private GameObject
		shield;



	void Start ()
	{
		facingRight = true;
		rigid = GetComponent<Rigidbody2D> ();
		myAnimator = GetComponent<Animator> ();
		cooldown = aSpeed;

	}

	void FixedUpdate ()
	{
		cooldown -= Time.fixedDeltaTime;

		isGrounded = isItGrounded ();

		HandleInput ();
		float horizontal = Input.GetAxis ("Horizontal");
		handleMovement (horizontal);
		
	}

	private void handleMovement (float horizontal)
	{
	
		if (isGrounded && isJumping) {
			isJumping = false;
			rigid.AddForce (new Vector2 (0, jumpForce));	
		}
		if (horizontal * rigid.velocity.x < mspeed)
			rigid.AddForce (Vector2.right * horizontal * accel);
		if (Mathf.Abs (rigid.velocity.x) > mspeed)
			rigid.velocity = new Vector2 (Mathf.Sign (rigid.velocity.x) * mspeed, rigid.velocity.y);	               

		flip (horizontal);
		myAnimator.SetFloat ("speed", Mathf.Abs (horizontal));

	}

	private void flip (float horizontal)
	{
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
		
			facingRight = !facingRight;
			Vector2 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		
		}
	}

	//Called by FixedUpdate
	private void HandleInput ()
	{
		//When the Jump Axis is beeing pressed during update.
		if (Input.GetAxis ("Jump") == 1 && isGrounded) {
			isJumping = true;		
		}
		//When the Fire1 Axis is beeing pressed during update.
		if (Input.GetAxis ("Fire1") == 1 && cooldown <= 0) {
			handleAttack ();
			cooldown = aSpeed;
		}
	
	}

	//clamado pelo handle input
	private void handleAttack ()
	{
		Vector3 aux = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 relativePos = aux - transform.position;
		Quaternion rotation = Quaternion.LookRotation (relativePos);
		rotation.x = 0;
		rotation.y = 0;		 

		GameObject clone = Instantiate (projectile, rigid.transform.position, rotation) as GameObject;
		clone.GetComponent<Projectile> ().setDirection (relativePos, forceProjectile);


	}

	//Called by HandleMovement.
	private bool isItGrounded ()
	{
		if (rigid.velocity.y <= 0) {
			foreach (Transform point in groundPoints) {
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);
				for (int i = 0; i<colliders.Length; i++) {
					if (colliders [i].gameObject != gameObject && colliders [i].gameObject != shield)
						return true;
				}
			}
		}
		return false;
	}
	//Called when a TriggerBoxCollider is touched.
	void OnTriggerEnter2D (Collider2D other)
	{		
		string otherTag = other.tag;
	
		if (otherTag.Equals ("EnemyProjectile")) {
			Vector2 relativePos = other.gameObject.transform.position - gameObject.transform.position;
			OnHit (relativePos);
		}
		if (otherTag.Equals ("Finish"))
			Finish (false);
	}

	//Called by On Triger enter in case of EnemyProjectile Hit.
	private void OnHit (Vector2 relativePos)
	{
		RaycastHit2D hit = Physics2D.Raycast (gameObject.transform.position,
		                                      relativePos);
		if (hit.collider.gameObject.name.Equals ("Body"))
			gotHit ();
	}

	//Called by OnHit.
	private void gotHit ()
	{
		hp--;
		if (hp == 0)
			Finish (true);

	}
	//Called when the level is reloaded / the next one is loaded.
	private void Finish (bool reset)
	{
		if (reset)
			Application.LoadLevel (Application.loadedLevel);
		else {
			if(Application.loadedLevelName.Equals("ac"))
				Application.LoadLevel("ac");
			if(Application.loadedLevelName.Equals("aa"))
				Application.LoadLevel("ab");
			else
				Application.LoadLevel("aa");		
		}


	}
		
}























