using UnityEngine;
using System.Collections;

public class AlwaysFaceObject : MonoBehaviour {

    public GameObject FacingTarget = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (FacingTarget)
        {
            this.gameObject.transform.LookAt(FacingTarget.transform);
        }
	
	}
}
