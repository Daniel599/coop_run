using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeChecker : MonoBehaviour {

    Collider2D m_collider;
    PlatformerCharacter2D m_char;
	// Use this for initialization
	void Start () {
        m_char = GetComponentInParent<PlatformerCharacter2D>();
        m_collider = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void SetCollider(Collider2D col)
    {
        Debug.Log("setting");
        m_collider = col;
    }

    public void ClearCollider()
    {
        m_collider = null;
    }

    public bool IsColliderStillAround()
    {
        return m_collider != null;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == m_collider)
        {
            //Time.timeScale = 0;
            Debug.Log("leaving rope");
            m_collider = null;
        }
    }
}
