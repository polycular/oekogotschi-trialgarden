using UnityEngine;
using System.Collections;

public class ShadowThrower : MonoBehaviour {

    public Vector3 ShadowDirection = Vector3.zero;
    public Vector3 Scale0 = Vector3.one;
    public Vector3 Scale1 = Vector3.zero;
    public float Scale1Distance = 1.0f;
    public GameObject ShadowQuadPrefab = null;

    protected GameObject ShadowQuad = null;

	// Use this for initialization
	void Start () {
        ShadowQuad = GameObject.Instantiate(ShadowQuadPrefab) as GameObject;
        DieByParent dbp = ShadowQuad.GetComponent<DieByParent>();
        if (dbp)
        {
            //Link the shadow to be detsroyed when this GO is destroyed
            dbp.Parent = this.gameObject;
        }
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit rc = new RaycastHit();
        if (Physics.Raycast(this.transform.position, ShadowDirection, out rc))
        {
            ShadowQuad.SetActive(true);
            ShadowQuad.transform.localPosition = rc.point;
            Vector3 scaleFactor = Vector3.Lerp(Scale0, Scale1, Mathf.InverseLerp(0, Scale1Distance, rc.distance));
            Vector3 scale = ShadowQuadPrefab.transform.localScale;
            scale.x *= scaleFactor.x;
            scale.y *= scaleFactor.y;
            scale.z *= scaleFactor.z;
            ShadowQuad.transform.localScale = scale;
        }
        else
        {
            ShadowQuad.SetActive(false);
        }
	}
}
