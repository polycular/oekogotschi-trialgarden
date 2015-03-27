using UnityEngine;
using System.Collections;

public class AutoFocusFix : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}

    void OnResume()
    {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
