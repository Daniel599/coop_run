using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour {
    public GameObject m_Roof;

    private void Start()
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == m_Roof)
        {
            //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<ConstantForce2D>().force = Vector2.up * 300; 
        }
    }
}
