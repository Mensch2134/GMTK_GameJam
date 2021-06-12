using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed = 5f;
    public float targetMoveSpeed;
    public Rigidbody2D rb;

    [Header("PlayerStats")]
    public int playerIndex;
    public int health;

    [Header("Drag")]
    public float dragForce;
    public float distanceInfluence;

    Vector2 movementDirection;

    float BASE_DRAG_FORCE = 1;
    private Vector2 dragDirection;
    private float distanceFactor;
    public bool movementFrozen = false;

    public UnityEvent _playerHit;


    private void Start()
    {
        _playerHit = new UnityEvent();
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
        if (GameManager.DragPhase)
        {
            rb.AddForce(dragDirection * BASE_DRAG_FORCE * dragForce * distanceFactor);
        }
        if (!movementFrozen)
        {
            move(movementDirection);
        }
    }

    void processInputs()
    {
        if (playerIndex == 0)
        {
            movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }else if (playerIndex == 1)
        {
            movementDirection = new Vector2(Input.GetAxisRaw("HorizontalAlt"), Input.GetAxisRaw("VerticalAlt"));
        }
    }

    void move(Vector2 direction)
    {
        //if (direction.magnitude > 1)
        //{
        //    direction = direction.normalized;
        //}
        //rb.velocity = direction * movementSpeed;
        if (rb.velocity.magnitude < targetMoveSpeed)
        {

        }
        rb.AddForce(direction * movementSpeed, ForceMode2D.Force);
    }

    public void CalculateDragDirection(Vector2 vDragDirection, float distance)
    {
        dragDirection = vDragDirection.normalized;
        //Debug.Log(this.name + dragDirection * dragForce);
        distanceFactor = distance * distanceInfluence;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        GameEvents.current.PlayerHit(this);
    }
}
