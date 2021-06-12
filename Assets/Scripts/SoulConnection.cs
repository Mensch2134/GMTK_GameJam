using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulConnection : MonoBehaviour
{
    public LineRenderer connection;

    public Transform player0Pos;
    public Transform player1Pos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        connection.SetPosition(0, player0Pos.position);
        connection.SetPosition(1, player1Pos.position);
    }
}
