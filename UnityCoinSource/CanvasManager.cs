using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance = null;

    private GameManager gameManager;
    private Animator animator;

    public TextMeshProUGUI informationText;
    public Dialogue dialogue;

    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoadedCM;
        SceneManager.sceneUnloaded += OnSceneClosedCM;
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
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnSceneLoadedCM(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) return;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogue.sentences[SceneManager.GetActiveScene().buildIndex - 1]));
    }

    void OnSceneClosedCM(Scene scene)
    {
    }
    

    IEnumerator TypeSentence (string sentence)
    {
        informationText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            informationText.text += letter;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        for (int i = 0; i < sentence.Length - 1; i++)
        {
            string temp = "";
            for (int j = i + 1; j < sentence.Length; j++)
            {
                temp += sentence[j];
            }
            informationText.text = temp;
            yield return null;
        }
        informationText.text = null;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

}
