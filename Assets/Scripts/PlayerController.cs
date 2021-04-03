using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    private Rigidbody rb;
    private int count; // used for counting collectibles

    //used for movement vectors
    private float movementX;
    private float movementY;
    private float movementZ;

    //used for double jump mechanic
    private bool grounded;
    private bool doubleJump;
    private bool jumping;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grounded = true;
        doubleJump = true;
        jumping = false;

        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementZ = movementVector.y;
        movementY = 0.0f;
    }

    void OnJump(InputValue jumpValue)
    {
        Debug.Log(transform.position.y);
        if (grounded)
        {
            jumping = true;
            Debug.Log(transform.position.y);
            movementY = 25.0f;
            grounded = false;
        }
        else if (!grounded && doubleJump)
        {
            Debug.Log(transform.position.y);
            jumping = true;
            movementY = 35.0f;
            doubleJump = false;
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            winTextObject.SetActive(true);
        }
    }
    void FixedUpdate()
    {
        // will jump if movementY > 0
        if (movementY > 0.0f)
        {
            Vector3 movement = new Vector3(movementX, movementY, movementZ);
            rb.AddForce(movement * speed);
            movementY = 0.0f;
        }
        else // not jumping
        {
            Vector3 movement = new Vector3(movementX, 0.0f, movementZ);
            rb.AddForce(movement * speed);

            // check if ball is grounded
            if ((transform.position.y == .5) && (jumping == true))
            {
                doubleJump = true;
                grounded = true;
                jumping = false;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        {
            other.gameObject.SetActive(false); //disables game objects
            count += 1;

            SetCountText();

        }
    }
}
