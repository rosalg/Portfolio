using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager gm = GameManager.instance;
        if (collision.gameObject.name == "PlayerB" || collision.gameObject.name == "PlayerW")
        {
            gm.KillPlayers();
            gm.Invoke("Restart", 1.5f);
            Destroy(gameObject);
        } else if (collision.gameObject.GetComponent<Tilemap>() != null)
        {
            Destroy(gameObject);
        }
    }
}
