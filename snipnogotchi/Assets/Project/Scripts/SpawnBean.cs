using UnityEngine;
using System.Collections;

public class SpawnBean : MonoBehaviour
{

    public GameObject BeanPrefab = null;
    public float InstantiationDelay = 1.0f;

    float m_lastInstantiation = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Snippable snippable = this.GetComponentInChildren<Snippable>();

        if(snippable != null && snippable.isFlying)
        {
            snippable.gameObject.transform.parent = null;
            m_lastInstantiation = Time.timeSinceLevelLoad;
        }
        else if (snippable == null && Time.timeSinceLevelLoad - m_lastInstantiation >= InstantiationDelay)
        {
            GameObject go = GameObject.Instantiate(BeanPrefab, this.transform.position, this.transform.rotation) as GameObject;
            go.transform.parent = this.transform;
            m_lastInstantiation = Time.timeSinceLevelLoad;
        }
	}
}
