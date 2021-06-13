using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.Events;

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
    public GameObject spawnPoint0;
    public GameObject spawnPoint1;
    public int playerMaxHealth = 3;
    List<GameObject> _lightZones = new List<GameObject>();
    public GameObject lightZonePrefab;

    private void Start()
    {
        TimerTimeText.text = "Game Starts in: " + timeRemaining;
        GameEvents.current.onPlayerHit += OnPlayerHit;
        DeployLightZones();
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
            DisableDragPhase();
        }
        else
        {
            EnableDragPhase();
        }
    }

   public void OnPlayerHit(PlayerController player)
    {
        //remove hp und reset position
        player.health -= 1;
        if(player.health == 0)
        {
            TimerTimeText.text = "Game Over";
            player0.health = playerMaxHealth;
            player1.health = playerMaxHealth;
        }
        DestroyLightZones();
        DeployLightZones();
        player0.rb.velocity =  new Vector2(0,0);
        player1.rb.velocity = new Vector2(0, 0);
        player0.transform.position = spawnPoint0.transform.position;
        player1.transform.position = spawnPoint1.transform.position;
        DisableDragPhase();
        timeRemaining = 15;
    }

    private void EnableDragPhase()
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

    private void DisableDragPhase()
    {
        DragPhase = false;
        _blockPlayerMovement = false;
        player0.movementFrozen = _blockPlayerMovement;
        player1.movementFrozen = _blockPlayerMovement;
        timeRemaining = maxWalkPhaseTimeInSeconds;
    }

    private void DeployLightZones()
    {
        int toAdd = UnityEngine.Random.Range(4, 7);
         
        for (int i = 0; i < toAdd; i++)
        {
            var zone = Instantiate(lightZonePrefab);
            float localScale = UnityEngine.Random.Range(2, 5);
            zone.transform.localScale = new Vector3(localScale,localScale,localScale);
            zone.transform.position = new Vector2(UnityEngine.Random.Range(-13, 13), UnityEngine.Random.Range(-5, 6));
            _lightZones.Add(zone);
        }
    }
    private void DestroyLightZones()
    {
       foreach(var zone in _lightZones)
        {
            zone.SetActive(false);
            Destroy(zone);
        }
        _lightZones.Clear();
    }
}