using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeHook : MonoBehaviour {

    Chain m_LastChain;
    PlatformerCharacter2D m_char;
    LayerMask m_RopesToHook;
    LayerMask m_RopesToCut;

    public bool HasRope { get; private set; }

    // Use this for initialization
    void Start()
    {
        m_char = GetComponentInParent<PlatformerCharacter2D>();
        m_RopesToHook = m_char.RopeLayerMask;
        m_RopesToCut = m_char.RopesToCut;
        HasRope = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (HasRope)
        {
            return;
        }

        if (((1 << other.gameObject.layer) & m_RopesToHook.value) != 0)
        {
            Chain NewChain = other.gameObject.GetComponentInParent<Chain>();
            if (NewChain != m_LastChain)
            {
                m_char.ConnectToRopeLink(other);
                HasRope = true;
                m_LastChain = NewChain;
            }
        }
        else if (((1 << other.gameObject.layer) & m_RopesToCut.value) != 0)
        {
            Debug.Log("cutting");
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1f, 0), ForceMode2D.Impulse);
            Chain.CutMe(other.gameObject);
        }
    }

    public void LeaveRope()
    {
        HasRope = false;
    }

    public void Reset()
    {
        HasRope = false;
        m_LastChain = null;
    }

    public void ResetRopeDectection()
    {
        m_LastChain = null;
    }
}
