using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;

public class LevelGenerator : MonoBehaviour
{
    public Button button;
    public Sprite padlock;
    UnityEventQueueSystem m_MyEvent;

    // Start is called before the first frame update
    void Start()
    {
        // Generate buttons
        GenerateButtons();
    }
 

    private void GenerateButtons()
    {
        GameObject c = GameObject.Find("Content");
        GridLayoutGroup gridLG = c.GetComponent<GridLayoutGroup>();
        RectTransform cr = c.GetComponent<RectTransform>();
        Rect sv = GameObject.Find("Scroll View").GetComponent<RectTransform>().rect;
        gridLG.cellSize = new Vector2(Screen.width/8f, Screen.width/16f);
        gridLG.spacing = new Vector2((-5f/44f)*Screen.width + 3200f/11f, 160);
        cr.sizeDelta = new Vector2(cr.sizeDelta.x, (gridLG.cellSize.y + gridLG.spacing.y) * (GameManager.instance.NumLevels / gridLG.constraintCount) + gridLG.padding.top);

        for (int i = 0; i < GameManager.instance.NumLevels; i++)
        {
            //Debug.Log(i);
            Button currButton = Instantiate(button);
            currButton.transform.SetParent(c.transform);
            // currButton.GetComponentInChildren<TMP_Text>().text = GameManager.instance.IndividualLevelData[i].LevelName;
            currButton.GetComponentInChildren<TMP_Text>().text = "";
            currButton.GetComponent<Image>().sprite = padlock;
            if (GameManager.instance.IndividualLevelData[i].IsUnlocked)
                currButton.GetComponent<Image>().sprite = GameManager.instance.LevelImages[i];
            string LevelNum = i.ToString();
            currButton.onClick.AddListener(delegate { FindObjectOfType<Menu>().GetComponent<Menu>().LoadLevel(LevelNum); });
        }
    }


}
