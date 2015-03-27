using UnityEngine;
using System.Collections;

public class DestroyOutOfWorld : MonoBehaviour {


    public float lowY = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (this.transform.position.y <= lowY)
        {
            GameObject.Destroy(this.gameObject);
        }
	}
}
