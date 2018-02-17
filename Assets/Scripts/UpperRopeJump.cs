using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperRopeJump : Challenge {

    Chain m_UpperChain;
    int m_LayerIdOfChainToRestore;
    public Transform m_ResetLocation;

    // Use this for initialization
    void Start () {
        m_UpperChain = GetComponentInChildren<Chain>();
        m_UpperChain.SetColor(Color.black);
        m_UpperChain.SetLayer(LayerMask.NameToLayer("Default"));
        m_LayerIdOfChainToRestore = m_UpperChain.gameObject.layer;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnSwitchTriggered()
    {
        Debug.Log("switch triggered");
        m_UpperChain.snapOffB();
        m_UpperChain.SetColor(Color.white);
        m_UpperChain.SetLayer(m_LayerIdOfChainToRestore);
        base.OnSwitchTriggered();

    }

    public override void OnTrapTriggered(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlatformerCharacter2D player = collision.gameObject.GetComponent<PlatformerCharacter2D>();
            player.Reset(m_ResetLocation.position + new Vector3(0, 1.5f));
            player.Pause();

            m_UpperChain.resetPositions(m_UpperChain.A.transform.position, Vector3.down);
        }
        base.OnTrapTriggered(collision);
    }
}
