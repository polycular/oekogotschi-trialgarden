using UnityEngine;
using System.Collections;

public class DieByParent : MonoBehaviour {


    public GameObject Parent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Parent == null)
        {
            GameObject.Destroy(this.gameObject);
        }
	
	}
}
