using UnityEngine;
using System.Collections;

public class InsideCupTrigger : MonoBehaviour {
	public GameObject SuccessTextPrefab;

	void Start(){
	}

	void OnTriggerEnter(Collider other) {

        Snippable snippable = other.GetComponent<Snippable>();
        if (snippable)
        {
            int score = 0;
            float rawScore = (Time.timeSinceLevelLoad - snippable.SnippedAt) * 100;
            BonusFactorCarrier bonus = other.GetComponent<BonusFactorCarrier>();

            GameObject go = GameObject.Instantiate(
                SuccessTextPrefab,
                this.transform.position + this.transform.up * 6,
                Quaternion.identity) as GameObject;

            if (bonus)
            {
                rawScore *= bonus.Multiplicator;
            }

            score = Mathf.CeilToInt(rawScore);

            TextMesh tm = go.GetComponentInChildren<TextMesh>();
            tm.text = score.ToString();
            go.GetComponent<AlwaysFaceObject>().FacingTarget = GameObject.FindObjectOfType<QCARBehaviour>().gameObject;


            AudioSource aus = this.GetComponents<AudioSource>()[0];
            if (aus)
            {
                aus.Play();
            }

            GameObject.Destroy(other.gameObject);
        }
	}
}
