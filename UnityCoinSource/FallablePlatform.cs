using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallablePlatform : MonoBehaviour
{
    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }


    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.name == "PlayerB")
        {
            gm.playerB.GetComponent<PlayerMovement>().CanFallThroughPlatform = true;
        }
        if (collider.gameObject.name == "PlayerW")
        {
            gm.playerW.GetComponent<PlayerMovement>().CanFallThroughPlatform = true;
        }
    }
    
}
