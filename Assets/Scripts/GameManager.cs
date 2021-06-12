using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static bool DragPhase = false;
    public PlayerController player0;
    public PlayerController player1;
    public bool _blockPlayerMovement = false;
    public float maxPullPhaseTimeInSeconds = 1;
    public float maxWalkPhaseTimeInSeconds = 8;
    public float timeRemaining = 20;
    public Text TimerTimeText;


    private void Start()
    {
        TimerTimeText.text = "Game Starts in: " + timeRemaining;
    }
    // Update is called once per frame
    void Update()
    {

        UpdatePhaseTimer();
        if (timeRemaining <= 0)
        {
            SwitchPhase();
        }

        if (Input.GetKeyUp(KeyCode.E) && !DragPhase)
        {
            SwitchPhase();
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

    //IEnumerator TimerForMe(float timerTime)
    //{
    //    yield return new WaitForSecondsRealtime(timerTime);
    //    DragPhase = false;
    //    _blockPlayerMovement = false;
    //    player0.movementFrozen = _blockPlayerMovement;
    //    player1.movementFrozen = _blockPlayerMovement;
    //}

    void UpdatePhaseTimer()
    {
        
        timeRemaining -= Time.deltaTime;
        if (!DragPhase)
        {
            TimerTimeText.text = Math.Floor(timeRemaining).ToString();
        }
        else
        {
            TimerTimeText.text = "Survive!";
        }
    }
    void SwitchPhase()
    {
        if (DragPhase)
        {
            DragPhase = false;
            _blockPlayerMovement = false;
            player0.movementFrozen = _blockPlayerMovement;
            player1.movementFrozen = _blockPlayerMovement;
            timeRemaining = maxWalkPhaseTimeInSeconds;
        }
        else
        {
            DragPhase = true;
            player0.rb.velocity = Vector2.zero;
            player1.rb.velocity = Vector2.zero;
            DragPlayers();
            _blockPlayerMovement = true;
            player0.movementFrozen = _blockPlayerMovement;
            player1.movementFrozen = _blockPlayerMovement;
            timeRemaining = maxPullPhaseTimeInSeconds;
        }
    }
}