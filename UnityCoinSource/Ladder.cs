using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "PlayerB")
        {
            gm.playerB.GetComponent<PlayerMovement>().CanClimb = true;
        }
        if (collider.gameObject.name == "PlayerW")
        {
            gm.playerW.GetComponent<PlayerMovement>().CanClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "PlayerB")
        {
            gm.playerB.GetComponent<PlayerMovement>().CanClimb = false;
            gm.playerB.GetComponent<PlayerMovement>().IsClimbing = false;
        }
        if (collider.gameObject.name == "PlayerW")
        {
            gm.playerW.GetComponent<PlayerMovement>().CanClimb = false;
            gm.playerW.GetComponent<PlayerMovement>().IsClimbing = false;
        }
    }
}
