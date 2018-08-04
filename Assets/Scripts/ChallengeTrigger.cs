using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeTrigger : MonoBehaviour {

    public int m_challenge_type;
    public Transform m_UpperRobot;
    public Transform m_LowerRobot;
    public List<Transform> targets;
    private Challenge m_challenge;

    // Use this for initialization
    void Start () {
        m_challenge = GetComponentInParent<Challenge>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
