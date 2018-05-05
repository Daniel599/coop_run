using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerPlatformDrop : Challenge
{

    //Chain m_UpperChain;
    int m_ChainOfUpperLayerId;
    public Transform m_ResetLocation;

    // Use this for initialization
    void Start()
    {
        //m_UpperChain = GetComponentInChildren<Chain>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnSwitchTriggered()
    {
        Debug.Log("switch triggered");
        base.OnSwitchTriggered();

    }

    public override void OnTrapTriggered(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //UnityEngine.Time.timeScale = 0.1f;
            //return;
            PlatformerCharacter2D player = collision.gameObject.GetComponent<PlatformerCharacter2D>();
            player.Reset(m_ResetLocation.position + new Vector3(0, 1.5f));
            player.Pause();
        }
        base.OnTrapTriggered(collision);
    }
}
