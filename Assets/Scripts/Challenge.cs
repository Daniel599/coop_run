using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : MonoBehaviour
{
    protected bool SwitchTriggered { get; private set; }
 
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void OnSwitchTriggered()
    {
        SwitchTriggered = true;
    }

    public virtual void OnTrapTriggered(Collider2D collision)
    {

    }

    public virtual void Reset()
    {
        SwitchTriggered = false;
    }
}
