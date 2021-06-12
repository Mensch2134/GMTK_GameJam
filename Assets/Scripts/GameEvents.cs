using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameEvents current;
    private void Awake()
    {
        current = this;
    }

    public event Action<PlayerController> onPlayerHit;
    public void PlayerHit(PlayerController player)
    {
        if (onPlayerHit != null)
        {
            onPlayerHit(player);
        }
    }
}
