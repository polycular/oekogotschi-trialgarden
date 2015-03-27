using UnityEngine;
using System.Collections;

public class BonusFactorCarrier : MonoBehaviour {


    public float MultiplierA = 5.0f;
    public float MultiplierB = 10.0f;
    public GameObject MultiplierAEffectPrefab = null;
    public GameObject MultiplierBEffectPrefab = null;

    protected float m_Multiplicator = 1;
    public float Multiplicator
    {
        get
        {
            return m_Multiplicator;
        }
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addMultiplierA()
    {
        m_Multiplicator *= MultiplierA;
        GameObject g = GameObject.Instantiate(MultiplierAEffectPrefab, this.transform.position, Quaternion.identity) as GameObject;
        g.GetComponent<AlwaysFaceObject>().FacingTarget = GameObject.FindObjectOfType<QCARBehaviour>().gameObject;
    }

    public void addMultiplierB()
    {
        m_Multiplicator *= MultiplierB;
        GameObject g = GameObject.Instantiate(MultiplierBEffectPrefab, this.transform.position, Quaternion.identity) as GameObject;
        g.GetComponent<AlwaysFaceObject>().FacingTarget = GameObject.FindObjectOfType<QCARBehaviour>().gameObject;
    }
}
