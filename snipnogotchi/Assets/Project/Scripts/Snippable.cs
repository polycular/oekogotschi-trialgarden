using UnityEngine;
using System.Collections;

public class Snippable : MonoBehaviour {


    public float maxAngle = 45.0f;

    //Value in Screensize.Units. Means that 1/1 is a full screen width/height
    public float SniplengthMin = 0.2f;
    //Value in Screensize.Units. Means that 1/1 is a full screen width/height
    public float SniplengthMax = 0.5f;

    public float AscentMin = 20.0f;
    public float AscentMax = 600.0f;

    public float SnipTimeLimit = 1.0f;

    public float SnipspeedMin = 100.0f;
    public float SnipspeedMax = 900.0f;

    public bool activateSpin = false;
    public float MaxSpinFactor = 10f;
    public float VerticalSpinFactor = 0f;
    public float HorizontalSpinFactor = 0f;


    protected Vector2 m_snipStart = Vector2.zero;
    protected float m_snipStartTime = 0f;
    protected Vector2 m_snipEnd = Vector2.zero;
    protected float m_snipEndTime = 0f;

    protected float m_snippedAt = 0f;
    public float SnippedAt
    {
        get
        {
            return m_snippedAt;
        }
    }

    private bool m_isFlying = false;

    public bool isFlying
    {
        get { return m_isFlying; }
    }


	// Use this for initialization
    void Start()
    {
        m_snippedAt = Time.timeSinceLevelLoad;
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!m_isFlying)
        {
            Vector2 resolution = new Vector2(Screen.width, Screen.height);
            if (Input.GetMouseButtonDown(0))
            {
                m_snipStart = new Vector2(Input.mousePosition.x / resolution.x, Input.mousePosition.y / resolution.y);
                m_snipStartTime = Time.timeSinceLevelLoad;
            }
            if (Input.GetMouseButtonUp(0))
            {
                m_snipEnd = new Vector2(Input.mousePosition.x / resolution.x, Input.mousePosition.y / resolution.y);

                Vector2 snipLength = m_snipEnd - m_snipStart;
                if (snipLength.sqrMagnitude > (SniplengthMin * SniplengthMin) && snipLength.y > 0)
                {
                    m_snipEndTime = Time.timeSinceLevelLoad;

                    if (m_snipEndTime - m_snipStartTime < SnipTimeLimit)
                    {
                        this.snip(snipLength);
                    }
                }
            }
        }
	}

    void snip(Vector2 snipLength)
    {

        AudioSource aus = this.GetComponents<AudioSource>()[1];
        if (aus)
        {
            aus.Play();
        }

        m_snippedAt = Time.timeSinceLevelLoad;
        m_isFlying = true;
        Rigidbody body = this.GetComponent<Rigidbody>();
        body.useGravity = true;
        body.isKinematic = false;

        //time taken to snip [0f,1f]
        float t = Mathf.InverseLerp(0, SnipTimeLimit, Mathf.Max(SnipTimeLimit - (m_snipEndTime - m_snipStartTime), 0.0f));
        //length of the snipping gesture
        float l = Mathf.InverseLerp(SniplengthMin, SniplengthMax, Mathf.Min(snipLength.magnitude, SniplengthMax));
        
        Vector2 snipDir = snipLength.normalized;


        float angle = Vector2.Angle(new Vector2(0, 1), snipDir);

        float invAngle = Mathf.InverseLerp(0, 90, angle);

        float newAngle = Mathf.Lerp(0, maxAngle, invAngle);
        Vector2 newSnipDir = new Vector2(0, 1);

        if (snipDir.x > 0)
        {
            newAngle *= -1;
        }

        Quaternion q = Quaternion.AngleAxis(newAngle, new Vector3(0, 0, 1));
        newSnipDir = q * newSnipDir;

        snipDir = newSnipDir.normalized;


        Vector3 finalForce = new Vector3(snipDir.x, 0f, snipDir.y);

        float snipSpeed = Mathf.Lerp(SnipspeedMin, SnipspeedMax, t);
        float snipAscent = Mathf.Lerp(AscentMin, AscentMax, l);

        finalForce *= snipSpeed;
        finalForce.y = snipAscent;

        finalForce = this.transform.rotation * finalForce;

        if (activateSpin)
        {
            body.maxAngularVelocity = MaxSpinFactor;
            body.AddTorque(
                transform.right * snipLength.y * VerticalSpinFactor * snipSpeed
                +
                transform.up * -snipLength.x * HorizontalSpinFactor * snipSpeed
                );
        }
        body.AddForce(finalForce);
    }

    void FixedUpdate()
    {

    }
}
