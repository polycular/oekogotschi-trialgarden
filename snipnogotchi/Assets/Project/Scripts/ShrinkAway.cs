using UnityEngine;
using System.Collections;

public class ShrinkAway : MonoBehaviour {

    public float Duration = 1f;
    protected float m_TimeToDie = 0f;

    protected Vector3 m_ActualScale = Vector3.one;

	// Use this for initialization
	void Start () {
        this.m_TimeToDie = Time.time + Duration;
        m_ActualScale = this.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localScale = m_ActualScale * (1.0f - Mathf.InverseLerp(m_TimeToDie - Duration, m_TimeToDie, Time.time));
        if (Time.time > m_TimeToDie)
        {
            GameObject.Destroy(this.gameObject);
        }
	}
}
