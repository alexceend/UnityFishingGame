using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFell : MonoBehaviour
{
    Rigidbody rb;

    public Vector3 resetPosition = new Vector3(-10f, 3.75f, 10f);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.position.y < -10f || Input.GetKeyDown(KeyCode.Z))
        {
            rb.position = resetPosition;

            // Reset velocity to prevent unwanted movement
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
