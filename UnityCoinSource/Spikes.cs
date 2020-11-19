using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager gm = GameManager.instance;
        if (collision.gameObject.name == "PlayerB" || collision.gameObject.name == "PlayerW")
        {
            gm.KillPlayers();
            gm.Invoke("Restart", 1.5f);
        }
    }
}
