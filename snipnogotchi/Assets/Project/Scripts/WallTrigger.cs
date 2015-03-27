using UnityEngine;
using System.Collections;

public class WallTrigger : MonoBehaviour {

    public bool HyperEffect = false;
	public int BonusPoints = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other){

        if (other.gameObject.CompareTag("Bean"))
        {
            BonusEffects effects = other.gameObject.GetComponent<BonusEffects>();
            BonusFactorCarrier bonus = other.gameObject.GetComponent<BonusFactorCarrier>();
            if (effects != null)
            {
                if (HyperEffect)
                {
                    bonus.addMultiplierB();
                    effects.HyperEffect.SetActive(true);
                }
                else
                {
                    bonus.addMultiplierA();
                    effects.SuperEffect.SetActive(true);
                }

                AudioSource aus = other.GetComponents<AudioSource>()[0];
                if (aus)
                {
                    aus.Play();
                }
            }
        }
	}
}
