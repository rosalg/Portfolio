using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NobleConnect.Mirror;
using UnityEngine.UI;
using Mirror;

public class OurNetworkManager : NobleNetworkManager
{
    public Transform leftPlayerSpawn;
    public Transform rightPlayerSpawn;
    public Button button;
    public static bool readyForWaves;
    public static GameObject[] players = new GameObject[2];
    public static Dictionary<GameObject, NetworkConnection> playerNetworkConnections =
        new Dictionary<GameObject, NetworkConnection>();


    public override void OnServerConnect( NetworkConnection conn )
    {
        base.OnServerConnect( conn );
    }

    public override void OnServerAddPlayer( NetworkConnection conn )
    {
        // add player at correct spawn position
        Transform start = numPlayers == 0 ? leftPlayerSpawn : rightPlayerSpawn;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        player.GetComponent<Player>().PlayerNumber = numPlayers == 0 ? 0 : 1;
        player.GetComponent<Player>().playerOne = numPlayers == 0 ? true : false;
        Debug.Log( numPlayers );
        players[numPlayers] = player;
        playerNetworkConnections.Add( player, conn );

        NetworkServer.AddPlayerForConnection( conn, player );
        if ( numPlayers == 2 ) button.gameObject.SetActive( true );
    }

    public override void OnServerDisconnect( NetworkConnection conn )
    {
        foreach ( GameObject enemy in GameObject.FindGameObjectsWithTag( "Enemy" ) ) NetworkServer.Destroy( enemy );
        GameManager gm = FindObjectOfType<GameManager>();
        if ( gm != null && gm.m_spawningCoroutine != null ) StopCoroutine( gm.m_spawningCoroutine );
        NetworkManager.Destroy( players[0] );
        if ( !GameManager.Testing ) NetworkManager.Destroy( players[1] );


        if ( gm != null ) gm.m_WaveInProgress = false;
        readyForWaves = false;

        // call base functionality (actually destroys the player)
        button.gameObject.SetActive( false );
        base.OnServerDisconnect( conn );
    }

    public void StartGame()
    {
        if ( numPlayers == 2 || GameManager.Testing )
        {
            button.gameObject.SetActive( false );
            readyForWaves = true;
        }
    }


}
