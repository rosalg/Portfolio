using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public void FadeBackground()
    {
        StartCoroutine(FadeBackgroundHelper());
    }

    public void FadeTitle()
    {
        StartCoroutine(FadeTitleHelper());
    }

    public void FadePlayButton()
    {
        StartCoroutine(FadeButton());
    }

    private IEnumerator FadeButton()
    {
        float rate = GameObject.Find("MainMenuPlay").GetComponent<Image>().color.a / 10f;
        for (float ft = 1f; ft >= 0; ft -= 0.1f)
        {
            Color c = GameObject.Find("MainMenuPlay").GetComponent<Image>().color;
            c.a -= rate;
            GameObject.Find("MainMenuPlay").GetComponent<Image>().color = c;
            Color ct = GameObject.Find("MainMenuPlayText").GetComponent<TMP_Text>().color;
            c.a -= rate;
            GameObject.Find("MainMenuPlayText").GetComponent<TMP_Text>().color = c;
            yield return new WaitForSeconds(0.06f);
        }
        GameObject.Find("MainMenuPlay").SetActive(false);
        yield return new WaitForSeconds(0.00f);
    }

    private IEnumerator FadeTitleHelper()
    {
        float rate = GameObject.Find("Title").GetComponent<TMP_Text>().color.a / 10f;
        for (float ft = 1f; ft >= 0; ft -= 0.1f)
        {
            Color c = GameObject.Find("Title").GetComponent<TMP_Text>().color;
            c.a -= rate;
            GameObject.Find("Title").GetComponent<TMP_Text>().color = c;
            yield return new WaitForSeconds(0.06f);
        }
        GameObject.Find("Title").SetActive(false);
        GameObject.Find("Network Manager").GetComponent<NetworkHUD>().enabled = true;
        yield return new WaitForSeconds(0.00f);
    }

    private IEnumerator FadeBackgroundHelper()
    {
        float rate = GameObject.Find("Background").GetComponent<Image>().color.a / 10f;
        for (float ft = 1f; ft >= 0; ft -= 0.1f)
        {
            Color c = GameObject.Find("Background").GetComponent<Image>().color;
            c.a -= rate;
            GameObject.Find("Background").GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.06f);
        }
        GameObject.Find("Background").SetActive(false);
        yield return new WaitForSeconds(0.00f);
    }
}
