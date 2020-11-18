using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [HideInInspector]
    public int colorSelected = -1;
    public int PlayerNumber;
    public Color playerOneColor;
    public Color playerTwoColor;
    public float movSpeed = 10f;
    public float bulletForce = 1000f;
    public GameObject[] bulletPrefabs;
    public GameObject[] pingPrefabs;
    public int maxBullets = 3;
    public float reloadTime = 1.5f;
    private Animator reloadAnimator;
    [SyncVar]
    public bool playerOne;

    public List<GameObject> enemiesTargeting = new List<GameObject>();
    private int bullets;


    // Start is called before the first frame update
    void Start()
    {
        colorSelected = 0;
        bullets = maxBullets;
        reloadAnimator = GameObject.Find("ReloadText").GetComponent<Animator>();


        if (playerOne)
        {
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
            {
                renderer.material.SetColor("_BaseColor", playerOneColor);
            }
        }
        else
        {
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
            {
                renderer.material.SetColor("_BaseColor", playerTwoColor);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( !base.hasAuthority )
        {
            return;
        }

        if ( Input.GetKeyDown( "1" ) )
        {
            colorSelected = 0;
        }
        else if ( Input.GetKeyDown( "2" ) )
        {
            colorSelected = 1;
        }
        else if ( Input.GetKeyDown( "3" ) )
        {
            colorSelected = 2;
        }

        // Player movement with wasd and rotation follows mouse 
        transform.Translate( Input.GetAxisRaw( "Horizontal" ) * movSpeed * Time.deltaTime, 0, Input.GetAxisRaw( "Vertical" ) * movSpeed * Time.deltaTime, Space.World );
        transform.forward = Camera.main.ScreenToWorldPoint( new Vector3( Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y - transform.position.y ) ) - transform.position;


        // Shooting system
        if ( Input.GetMouseButtonDown( 0 ) && bullets > 0 )
        {
            // Have to pass in color selected here because can't leave that to the server
            // @bbarnett (6/3) TODO: This seems to sometimes cause a crash when the other player has disconnected.
            CmdShoot( colorSelected );
            bullets--;
            if ( bullets <= 0 )
            {
                StartCoroutine( Reload() );
            }
        }

        if ( Input.GetMouseButtonDown( 1 ) )
        {
            Vector3 pingLocation = Camera.main.ScreenToWorldPoint( new Vector3( Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y - 0.2f ) );
            CmdPing( colorSelected, pingLocation.x, pingLocation.y, pingLocation.z );
        }

        /* TODO: Ping system and shooting system.
         * 
         * Detect when player clicks
         * 
         * If player clicks on their opponent's side:
         *      instantiate a ping particle prefab corresponding to `colorSelected` at that location. (E.g. BluePingParticles)
         * 
         * If player clicks on their own side:
         *      Shoot a bullet of color `colorSelected` at that location.
         *      Notes: 
         *          There's already a Projectile prefab, but color needs to be changed. 
         *          A simpler idea would be to just check if an enemy is present at the location and kill instantly; no projectiles needed
         * 
         */
    }

    IEnumerator Reload()
    {
        reloadAnimator.SetBool( "Reloading", true );
        yield return new WaitForSeconds( reloadTime );
        reloadAnimator.SetBool( "Reloading", false );
        bullets = maxBullets;
    }

    [ClientRpc]
    void RpcPlayShootingSound(int colorSelected) {
        if (colorSelected == 0)
        {
            AudioManager.instance.Play("RedShot");
        }
        else if (colorSelected == 1)
        {
            AudioManager.instance.Play("GreenShot");
        }
        else
        {
            AudioManager.instance.Play("BlueShot");
        }
    }

    [Command]
    private void CmdShoot( int colorSelected )
    {
        if ( colorSelected < bulletPrefabs.Length )
        {
            RpcPlayShootingSound(colorSelected);
            GameObject bullet = Instantiate(bulletPrefabs[colorSelected], transform.position, transform.rotation);
            bullet.GetComponentInChildren<Bullet>().PlayerNumber = PlayerNumber;
            NetworkServer.Spawn( bullet );
        }
    }

    [ClientRpc]
    void RpcPlayPingSound(int colorSelected) {
        if (colorSelected == 0)
        {
            AudioManager.instance.Play("RedPing");
        }
        else if (colorSelected == 1)
        {
            AudioManager.instance.Play("GreenPing");
        }
        else
        {
            AudioManager.instance.Play("BluePing");
        }
    }

    [Command]
    private void CmdPing( int colorSelected, float x, float y, float z )
    {
        RpcPlayPingSound(colorSelected);
        Vector3 location = new Vector3(x, y, z);
        GameObject ping = Instantiate(pingPrefabs[PlayerNumber], location, Quaternion.Euler(-90f, 0f, 0f));
        NetworkServer.Spawn( ping );
    }
}
