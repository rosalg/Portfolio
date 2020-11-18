using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    public float destroyAfter = 5f;
    public float bulletForce = 1000f;
    public Rigidbody rb;
    public int PlayerNumber;
    public GameObject deathParticles;
    private GameManager gm;

    public Enemy.Color_t m_Color;

    public override void OnStartServer()
    {
        Invoke( nameof( DestroySelf ), destroyAfter );
    }

    // Destroy for everyone on the server
    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy( gameObject.transform.parent.gameObject );
    }

    // set velocity for server and client. this way we don't have to sync the
    // position, because both the server and the client simulate it.
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        rb.AddForce( transform.forward * bulletForce );
    }

    [ServerCallback]
    private void OnCollisionEnter( Collision collision )
    {
        if ( collision.gameObject.tag == "Enemy" && collision.gameObject.GetComponent<Enemy>().TargetPlayerNumber == PlayerNumber )
        {
            if ( collision.gameObject.GetComponent<Enemy>().color == m_Color )
            {
                gm.m_currentWave.ActiveEnemies--;
                NetworkServer.Destroy( collision.gameObject );
            }
        }
        NetworkServer.Destroy( gameObject.transform.parent.gameObject );
        GameObject particles = Instantiate( deathParticles, transform.position, transform.rotation );
        NetworkServer.Spawn( particles );
    }
}
