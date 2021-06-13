using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.Events;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static bool DragPhase = false;
    public static bool GameRunning;
    public PlayerController player0;
    public PlayerController player1;
    public bool _blockPlayerMovement = false;
    public float maxPullPhaseTimeInSeconds = 1;
    public float maxWalkPhaseTimeInSeconds = 8;
    public float timeRemaining = 20;
    public Text _timerTimeText;
    public GameObject spawnPoint0;
    public GameObject spawnPoint1;
    public static int playerMaxHealth = 4;
    List<GameObject> _lightZones = new List<GameObject>();
    public GameObject lightZonePrefab;
    public GameObject _gameOverPanel;
    public Text _playerWinsMessage;
    

    private void Start()
    {
        _gameOverPanel.SetActive(false);
        _timerTimeText.text = "Game Starts in: " + timeRemaining;
        GameEvents.current.onPlayerHit += OnPlayerHit;
        _playerWinsMessage.text = string.Empty;
        StartGame();
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameRunning && Input.GetKeyUp(KeyCode.Space))
        {
            StartGame();
        }
        
        if(GameRunning)
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
            _timerTimeText.text = Math.Floor(timeRemaining).ToString();
        }
        else
        {
            _timerTimeText.text = "Survive!";
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
    private bool blockPlayerHits = false;
    public void OnPlayerHit(PlayerController player)
    {
        if (DragPhase && !blockPlayerHits)
        {
            blockPlayerHits = true;
            
            //remove hp und reset position
            StartCoroutine(delay(player));
            
        }
    }


    IEnumerator delay(PlayerController player)
    {
        Time.timeScale = 0;
        player.health -= 1;
        player.transform.Rotate(0, 0, -90);
        SetBlockPlayerInput(true);
        yield return new WaitForSecondsRealtime(0.5f);
        if (player.health == 0)
        {
            ExecuteGameOver(player);
        }
        DestroyLightZones();
        DeployLightZones();
        player0.rb.velocity = new Vector2(0, 0);
        player1.rb.velocity = new Vector2(0, 0);
        player0.transform.position = spawnPoint0.transform.position;
        player1.transform.position = spawnPoint1.transform.position;
        DisableDragPhase();
        timeRemaining = 15;
        SetBlockPlayerInput(false);
        player.transform.Rotate(0, 0, 90);
        Time.timeScale = 1;
        blockPlayerHits = false;
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
        DestroyLightZones();
        DeployLightZones();
    }

    private void SetBlockPlayerInput(bool b)
    {
        _blockPlayerMovement = b;
        player0.movementFrozen = _blockPlayerMovement;
        player1.movementFrozen = _blockPlayerMovement;
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

    private void ExecuteGameOver(PlayerController loosingPlayer)
    {
        GameRunning = false;
        DragPhase = false;
        _timerTimeText.text = string.Empty;
        player0.movementFrozen = _blockPlayerMovement;
        player0.movementFrozen = _blockPlayerMovement;
        string winner;
        if (loosingPlayer == player0)
        {
            winner = "Red Ghost Wins!";
        }
        else
        {
            winner = "Blue Ghost Wins!";
        }
        _playerWinsMessage.text = winner;
        _gameOverPanel.SetActive(true);

    }

    private void StartGame()
    {
        player0.health = playerMaxHealth;
        player1.health = playerMaxHealth;
        GameRunning = true;
        DestroyLightZones();
        DeployLightZones();
        DisableDragPhase();
        _gameOverPanel.SetActive(false);
    }
}