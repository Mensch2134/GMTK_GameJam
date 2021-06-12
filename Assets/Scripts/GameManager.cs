using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool movementFrozen = false;

    public PlayerController player0;
    public PlayerController player1;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.E))
        {
            movementFrozen = !movementFrozen;
            DragPlayers();
        }
    }

    void DragPlayers()
    {
        Vector2 player0Direction = player1.gameObject.transform.position - player0.gameObject.transform.position;
        Vector2 player1Direction = player0Direction * -1f;
        float distance = player0Direction.magnitude;

        player0.DragMeARiver(player0Direction, distance);
        player1.DragMeARiver(player1Direction, distance);
    }
}
