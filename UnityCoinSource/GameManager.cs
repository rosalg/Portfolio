using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [HideInInspector]
    public GameObject exitB;
    [HideInInspector]
    public GameObject exitW;
    [HideInInspector]
    public IndivLevelData[] IndividualLevelData;
    [SerializeField]
    public Sprite[] LevelImages;
    [HideInInspector]
    public GameObject playerB;
    [HideInInspector]
    public GameObject playerW;
    [HideInInspector]
    public bool isLoading;

    public GameObject DeathParticleB;
    public GameObject DeathParticleW;
    public int NumLevels = 45;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        
        if (SaveSystem.LoadLevel() != null)
        {
            for (int i = 0; i < NumLevels; i++)
            {
                IndividualLevelData = SaveSystem.LoadLevel().LD;
            }
        }
        else
        {
            IndividualLevelData = new IndivLevelData[NumLevels];
            for (int i = 0; i < NumLevels; i++)
            {
                IndividualLevelData[i] = new IndivLevelData(false, i + 1.ToString());
            }
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;

        if (!CanvasManager.instance.gameObject.activeInHierarchy) CanvasManager.instance.gameObject.SetActive(true);

        exitB = GameObject.Find("BExit");
        exitW = GameObject.Find("WExit");

        if (!IndividualLevelData[Int32.Parse(scene.name)].IsUnlocked)
        {
            IndividualLevelData[Int32.Parse(scene.name)].IsUnlocked = true;
            SaveSystem.SaveLevel(this);
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        playerB = GameObject.Find("PlayerB");
        playerW = GameObject.Find("PlayerW");
        if (isLoading)
        {
            return;
        }

        if (playerB != null && playerW != null)
        {
            Collider2D colliderB = playerB.GetComponent<BoxCollider2D>();
            Collider2D colliderW = playerW.GetComponent<BoxCollider2D>();
            if (exitB == null) return;
            if (exitB.GetComponent<BoxCollider2D>().IsTouching(colliderB) && exitW.GetComponent<BoxCollider2D>().IsTouching(colliderW))
            {
                isLoading = true;
                AudioManager.instance.Play("VictorySound");
                StartCoroutine(NextLevel());
            }
        }
    }
    
    public void KillPlayers()
    {
        Instantiate(DeathParticleB, new Vector3(playerB.transform.position.x, playerB.transform.position.y, 0), DeathParticleB.transform.rotation);
        Destroy(playerB);
        Instantiate(DeathParticleW, new Vector3(playerW.transform.position.x, playerW.transform.position.y, 0), DeathParticleW.transform.rotation);
        Destroy(playerW);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator NextLevel()
    {
        Destroy(playerB);
        Destroy(playerW);
        yield return new WaitForSeconds(1f);
        isLoading = false;
        if (SceneManager.GetActiveScene().buildIndex + 1 <= NumLevels)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else
        {
            CanvasManager.instance.gameObject.SetActive(false);
            SceneManager.LoadScene(0);
        }
    }

}
