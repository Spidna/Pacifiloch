using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventParent : MonoBehaviour
{
    // I think onStart is redundant cuz Nothing can be added beforehand
    //public System.Action onStart;
    public System.Action onDestroy;
    public System.Action onUpdate;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //onStart?.Invoke();
        onDestroy += clearUpdate;
    }

    // Called at a fixed interval in ballpark of framerate
    protected virtual void FixedUpdate()
    {
        onUpdate?.Invoke();
    }

    protected virtual void OnDestroy()
    {
        onDestroy?.Invoke();
    }
    protected virtual void clearUpdate()
    {
        onUpdate = null;
    }


}
