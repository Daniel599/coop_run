using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperBalloonFly : Challenge
{

    Chain m_UpperChain;
    int m_LayerIdOfChainToRestore;
    public Transform m_ResetLocation;

    private void Awake()
    {
        m_UpperChain = GetComponentInChildren<Chain>();
        m_UpperChain.rebuildRope();
    }
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnSwitchTriggered()
    {
        Debug.Log("switch triggered");
        m_UpperChain.snapOffB();
        base.OnSwitchTriggered();

    }

    public override void OnTrapTriggered(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlatformerCharacter2D player = collision.gameObject.GetComponent<PlatformerCharacter2D>();
            player.Reset(m_ResetLocation.position + new Vector3(0, 1.5f));
            if (!SwitchTriggered)
            {
                player.Pause();
            }

            Vector3 reset_pos = m_UpperChain.A.transform.position + new Vector3(m_UpperChain.offsetA.x, m_UpperChain.offsetA.y);
            m_UpperChain.resetPositions(reset_pos, Vector3.down);
        }
        base.OnTrapTriggered(collision);
    }
}
