using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public Rigidbody2D rb;

    Vector2 movementDirection;

    public int playerIndex;

    bool playerIsBeginDragged;
    Vector2 dragDirection;
    public float dragForce;
    public float dragDuration;

    private void Start()
    {
        if (playerIndex < 0 || playerIndex > 1)
        {
            Debug.Log("PlayeIndex has to be 1 or 0 for the Players to move");
        }
    }

    // Update is called once per frame
    void Update()
    {
        processInputs();
    }

    void FixedUpdate()
    {
        if (!GameManager.movementFrozen)
        {
            move(movementDirection);
        }else if (playerIsBeginDragged)
        {
            drag(dragDirection);
        }
    }

    void processInputs()
    {
        if (playerIndex == 0)
        {
            movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }else if (playerIndex == 1)
        {
            movementDirection = new Vector2(Input.GetAxisRaw("HorizontalAlt"), Input.GetAxisRaw("VerticalAlt")).normalized;
        }
    }

    void move(Vector2 direction)
    {
        rb.MovePosition(rb.position + direction * movementSpeed * Time.deltaTime);
    }

    void drag(Vector2 vDragDirection)
    {
        rb.AddForce(vDragDirection * dragForce);
        StartCoroutine(DraggingThePlayer());
    }

    public void DragMeARiver(Vector2 vDragDirection)
    {
        playerIsBeginDragged = true;
        dragDirection = vDragDirection.normalized;
    }

    IEnumerator DraggingThePlayer()
    {
        yield return new WaitForSecondsRealtime(dragDuration);
        playerIsBeginDragged = false;
    }
}
