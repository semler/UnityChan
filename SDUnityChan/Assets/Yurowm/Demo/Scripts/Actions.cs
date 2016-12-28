using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class Actions : MonoBehaviour {

	private Animator animator;
	private CapsuleCollider col;
	private Vector3 velocity;

	const int countOfDamageAnimations = 3;
	int lastDamageAnimation = -1;

	void Awake () {
		animator = GetComponent<Animator> ();
	}

	void Update () {
		Aiming ();
//		animator.SetTrigger ("Attack");



		float h = Input.GetAxis ("Horizontal");				// 入力デバイスの水平軸をhで定義
		float v = Input.GetAxis ("Vertical");	

		velocity = new Vector3 (0, 0, v);	// 上下のキー入力からZ軸方向の移動量を取得
		velocity = transform.TransformDirection (velocity);

		if (v > 0) {
			velocity *= 2;		// 移動速度を掛ける
		} else if (v < 0) {
			velocity *= 1;	// 移動速度を掛ける
		}

		col = GetComponent<CapsuleCollider> ();

		// 上下のキー入力でキャラクターを移動させる
		transform.localPosition += velocity * Time.fixedDeltaTime;

		// 左右のキー入力でキャラクタをY軸で旋回させる
		transform.Rotate (0, h * 2, 0);	

		if (Input.GetButton ("Fire1")) {
			Aiming ();
			animator.SetTrigger ("Attack");
			
		}


	}

	public void Aiming () {
		animator.SetBool ("Squat", false);
		animator.SetFloat ("Speed", 0f);
		animator.SetBool("Aiming", true);
	}
}
