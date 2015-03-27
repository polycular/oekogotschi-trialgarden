using UnityEngine;
using System.Collections;

public class StartForce : MonoBehaviour {


    public Vector3 Force = Vector3.zero;

	// Use this for initialization
	void Start () {
        Rigidbody r = this.GetComponent<Rigidbody>();
        if (r != null)
        {
            r.AddForce(Force);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
