using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed = 5f;
    public Rigidbody2D rb;

    [Header("PlayerStats")]
    public int playerIndex;

    [Header("Drag")]
    public float dragForce;
    public float distanceInfluence;

    Vector2 movementDirection;

    float BASE_DRAG_FORCE = 1;
    private Vector2 dragDirection;
    private float distanceFactor;
    public bool movementFrozen = false;

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
        if (!GameManager.isBeingDragged && !movementFrozen)
        {
            move(movementDirection);
        }
        if (GameManager.isBeingDragged && movementFrozen)
        {
            rb.AddForce(dragDirection * BASE_DRAG_FORCE * dragForce * distanceFactor);
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

    public void DragMeARiver(Vector2 vDragDirection, float distance)
    {
        dragDirection = vDragDirection.normalized;
        //Debug.Log(this.name + dragDirection * dragForce);
        distanceFactor = distance * distanceInfluence;
        movementFrozen = true;
    }
}
