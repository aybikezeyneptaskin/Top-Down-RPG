using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    private Vector2 movementInput;
    private Rigidbody2D rb;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        //If movement input is not 0 try to move
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);
            //slide on edge of objects -> smoother movement ama olmadı sanki???
            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));
                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        // check for potential collisions
        int count = rb.Cast(
            movementInput, // x and y vals between -1 and 1 represents direction from the body to look for collisions
            movementFilter, // the settings that determine where a collision can occur on such as layers to collide with
            castCollisions, //list of collisions to store the fpund collisions into after the cast is finished
            moveSpeed * Time.fixedDeltaTime + collisionOffset); //the amount to cast equal to the movement plus an offset
        if (count == 0)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        else return false;
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}
