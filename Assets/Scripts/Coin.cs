using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    private Rigidbody2D m_body;
    private bool m_released = false;
    public LayerMask m_Floor;
    public Vector2 m_UpReleaseForce;
    public Vector2 m_DownReleaseForce;
    public bool PassedToOtherSide { get; protected set; }
    public bool OnTheFloor { get; protected set; }
    private GameObject m_FirstChar;

    private void Start()
    {
        OnTheFloor = false;
        m_body = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!m_released && collision.gameObject.tag == "Player")
        {
            m_FirstChar = collision.gameObject;
            Side side = m_FirstChar.GetComponent<Character>().Side;
            Vector2 applyForce;
            if (side == Side.Upper)
            {
                applyForce = m_DownReleaseForce;
            }
            else
            {
                applyForce = m_UpReleaseForce;
            }
            m_released = true;
            m_body.bodyType = RigidbodyType2D.Dynamic;
            m_body.AddForce(applyForce);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!PassedToOtherSide && collision.gameObject.layer == LayerMask.NameToLayer("Layout"))
        {
            PassedToOtherSide = true;
            GetComponent<CircleCollider2D>().isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!OnTheFloor &&
            (((1 <<collision.gameObject.layer) & m_Floor.value) != 0))
        {
            OnTheFloor = true;
            Debug.Log("On the Floor");
        }

        if ((collision.gameObject != m_FirstChar) &&
            (collision.gameObject.tag == "Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
