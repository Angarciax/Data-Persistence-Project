using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        int size = gameManager.GetComponent<GameManager>().ballSize;
        switch( size )
        {
            case 0:
                transform.localScale *= 0.5f;
                break;
            //            case 1:
            // transform.localScale *= 2.0f;
            //                break;
            case 2:
                transform.localScale *= 2.0f;
                break;
        }
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    
    private void OnCollisionExit(Collision other)
    {
        var velocity = m_Rigidbody.velocity;
        
        //after a collision we accelerate a bit
        velocity += velocity.normalized * 0.01f;
        
        //check if we are not going totally vertically as this would lead to being stuck, we add a little vertical force
        if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.1f)
        {
            velocity += velocity.y > 0 ? Vector3.up * 0.5f : Vector3.down * 0.5f;
        }

        //max velocity
        if (velocity.magnitude > 3.0f)
        {
            velocity = velocity.normalized * 3.0f;
        }

        m_Rigidbody.velocity = velocity;
    }
}
