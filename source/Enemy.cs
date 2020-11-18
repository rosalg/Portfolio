using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Enemy : NetworkBehaviour
{
    [HideInInspector]
    public Transform player;
    private Rigidbody m_Rigidbody;
    private Vector3 moveDirection;

    [HideInInspector]
    public int TargetPlayerNumber;

    [SyncVar]
    public Color_t color = Color_t.NONE;
    public enum Color_t {NONE = -1, RED = 1, BLUE = 2, GREEN = 3};
    public Material[] mats = new Material[4];
    [HideInInspector]
    public float moveForce;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        float temp = Random.Range(1.0f, 3.9f);
        color = (Color_t)Mathf.FloorToInt(Random.Range(1.0f, 3.9f));
    }

    [TargetRpc]
    public void TargetRenderColor(NetworkConnection target) {
        Renderer rend = GetComponent<Renderer>();
        if (color == Color_t.RED)
        {
            rend.material = mats[0];
        }
        else if (color == Color_t.GREEN)
        {
            rend.material = mats[1];
        }
        else if (color == Color_t.BLUE)
        {
            rend.material = mats[2];
        }
        else
        {
            rend.material = mats[3];
        }
    }

    void Update()
    {
        if (player != null)
        {
            
            moveDirection = Vector3.zero;
            Vector3 direction = player.position - this.transform.position;
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
            moveDirection = direction.normalized;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Do we want to be adding force on each update? The enemy's speed should prob be constant towards the player
        m_Rigidbody.AddForce(moveDirection * moveForce);
        m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z);
        /* TODO: Enemy behavior and player death detection
            * 1. Use colliders to detect when an enemy reaches a player. If a player gets touched, the game ends; notify the network manager
            */
    }

    [ServerCallback]
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.transform.CompareTag("Player") && TargetPlayerNumber == collision.gameObject.GetComponentInParent<Player>().PlayerNumber)
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) NetworkServer.Destroy(enemy);
            GameManager gm = FindObjectOfType<GameManager>();
            OurNetworkManager nm = FindObjectOfType<OurNetworkManager>();
            StopCoroutine(gm.m_spawningCoroutine);
            OurNetworkManager.players[0].transform.position = nm.leftPlayerSpawn.position;
            if (!GameManager.Testing)
            {
                OurNetworkManager.players[1].transform.position = nm.rightPlayerSpawn.position;
            }
            gm.CreateWaves();
            gm.m_WaveInProgress = false;
            OurNetworkManager.readyForWaves = false;
            nm.button.gameObject.SetActive(true);
        }
    }
}
