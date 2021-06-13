using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulConnection : MonoBehaviour
{
    public LineRenderer connection;

    public Transform player0Pos;
    public Transform player1Pos;

    [SerializeField]
    private Texture[] textures;

    [SerializeField]
    private Texture dragTexture;

    private int animationStep;

    [SerializeField]
    private float fps = 30f;

    private float fpsCounter;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        connection.SetPosition(0, player0Pos.position);
        connection.SetPosition(1, player1Pos.position);

        if (GameManager.DragPhase)
        {
            fpsCounter = 0f;
            animationStep = 0;
            connection.material.SetTexture("_MainTex", dragTexture);
        } else
        {
            fpsCounter += Time.deltaTime;
            if (fpsCounter >= 1f / fps)
            {
                animationStep++;
                if (animationStep == textures.Length)
                {
                    animationStep = 0;
                }

                connection.material.SetTexture("_MainTex", textures[animationStep]);

                fpsCounter = 0f;
            }
        }       
    }
}
