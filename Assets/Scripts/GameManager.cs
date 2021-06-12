using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class GameManager : MonoBehaviour
{
    public static bool isBeingDragged = false;
    public PlayerController player0;
    public PlayerController player1;
    public bool _blockPlayerMovement = false;
    public float timerLength;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.E) && !isBeingDragged)
        {
            isBeingDragged = true;

            _blockPlayerMovement = true;
            player0.movementFrozen = _blockPlayerMovement;
            player1.movementFrozen = _blockPlayerMovement;

            DragPlayers();
            StartCoroutine(TimerForMe(timerLength));
            player0.movementFrozen = true;
            player1.movementFrozen = true;
        }
    }

    void DragPlayers()
    {
        Vector2 player0Direction = player1.gameObject.transform.position - player0.gameObject.transform.position;
        Vector2 player1Direction = player0Direction * -1f;
        float distance = player0Direction.magnitude;

        player0.CalculateDragDirection(player0Direction, distance);
        player1.CalculateDragDirection(player1Direction, distance);
    }

    IEnumerator TimerForMe(float timerTime)
    {
        yield return new WaitForSecondsRealtime(timerTime);
        isBeingDragged = false;
        _blockPlayerMovement = false;
        player0.movementFrozen = _blockPlayerMovement;
        player1.movementFrozen = _blockPlayerMovement;
    }
}