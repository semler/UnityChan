using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour {

	private CharacterController eCon;
	private Animator animator;
	private Vector3 destination;	//目的地
	private Vector3 randDestination;	//目的地
	private Vector3 velocity;        //速度×方向
	private Vector3 direction;       //移動方向
	private float waitTime;       
	private float currentTime;
	private GameObject unityChan;
							// キャラにアタッチされるアニメーターへの参照
	private AnimatorStateInfo stateInfo;

	// Use this for initialization
	void Start () {
		eCon = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		velocity = Vector3.zero;
		unityChan = GameObject.Find("SD_unitychan_humanoid");
		waitTime = 2.0f;
		currentTime = 0.0f;
		randDestination = RandDestination();
	}
	
	// Update is called once per frame
	void Update () {
		stateInfo = animator.GetCurrentAnimatorStateInfo (0);
		destination = new Vector3(unityChan.transform.position.x, unityChan.transform.position.y, unityChan.transform.position.z);

		if (Vector3.Distance (destination, transform.position) > 10) {
			animator.SetBool ("Attack", false);

			if (Vector3.Distance (randDestination, transform.position) < 0.5) {
				currentTime += Time.deltaTime;
				if (currentTime > waitTime) {
					animator.SetBool ("Stop", false);
					randDestination = RandDestination ();
					currentTime = 0.0f;
					Walk (randDestination, 0.5f);
				} else {
					animator.SetBool ("Stop", true);
				}
			} else {
				animator.SetBool ("Stop", false);
				Walk (randDestination, 0.5f);
			}
		} else if (Vector3.Distance (destination, transform.position) < 1.5) {
			animator.SetBool ("Attack", true);
			animator.SetBool ("Stop", false);

			direction = (destination - transform.position).normalized;
			transform.LookAt (new Vector3 (destination.x, transform.position.y, destination.z));

			if (stateInfo.IsName ("Attack")) {
				unityChan.SendMessage ("StartAttacked");
			}
		} else {
			animator.SetBool ("Attack", false);
			animator.SetBool ("Stop", false);

			Walk (destination, 1.0f);
		}
	}

	private void Walk (Vector3 destination, float speed) {
		if (eCon.isGrounded) {
			velocity = Vector3.zero;
			direction = (destination - transform.position).normalized;
			transform.LookAt (new Vector3 (destination.x, transform.position.y, destination.z));
			velocity = direction * speed;
		}
		velocity.y += Physics.gravity.y * Time.deltaTime;
		eCon.Move (velocity * Time.deltaTime);
	}

	private Vector3 RandDestination () {
		Vector3 random = Random.insideUnitCircle * 10;
		return transform.position + new Vector3(random.x, 0, random.z);
	}
}
