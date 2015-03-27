using UnityEngine;
using System.Collections;

public class SnippingForce : MonoBehaviour {
	private float speed;
	private Vector3 forceDirection;
	private Vector2 firstTouchPosition;
	private Vector2 lastTouchPosition;


	// Use this for initialization
	void Start () {
		speed = 1.0f;
		forceDirection = new Vector3(0f, 0f, 1.0f);
		this.rigidbody.AddForce(forceDirection*speed);
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 currPos = this.transform.position;
		this.transform.position = currPos + forceDirection * speed;
		speed = speed * 0.995f;

	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.name == "brickWall")
		{
			forceDirection = Vector3.forward * -1;
			speed = speed * 0.7f;
		}
	}
}
