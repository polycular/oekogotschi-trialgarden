using UnityEngine;
using System.Collections;

public class Bounce : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}



    void OnCollisionEnter(Collision collision)
    {

        AudioSource aus = this.GetComponents<AudioSource>()[0];
        if (aus)
        {
            aus.Play();
        }
    }
}
