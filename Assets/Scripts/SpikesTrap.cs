﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesTrap : MonoBehaviour {

    Challenge m_challenge;

    // Use this for initialization
    void Start () {
        m_challenge = GetComponentInParent<Challenge>();
        if (m_challenge == null)
        {
            Debug.Log("SpikesTrap must be within Challenge");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_challenge.OnTrapTriggered(collision);
        }
        else if (collision.gameObject.tag == "Coin")
        {
            if (collision.gameObject.GetComponent<Coin>().PassedToOtherSide == true)
            {
                Destroy(collision.gameObject);
                return;
            }
        }
    }
}
