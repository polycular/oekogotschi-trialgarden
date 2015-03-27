using UnityEngine;
using System.Collections;

public class ReactToTrackable : MonoBehaviour, ITrackableEventHandler 
{
    public TrackableBehaviour TargetTrackableBehaviour;


    void Start()
    {
        if (this.TargetTrackableBehaviour)
        {
            this.TargetTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    void OnTrackingFound()
    {
        this.gameObject.SetActive(true);
    }
    void OnTrackingLost()
    {
        this.gameObject.SetActive(false);
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }
}
