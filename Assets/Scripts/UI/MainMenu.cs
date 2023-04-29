using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public string levelScene = "TestScene";

    [Header("Screens")]
    public GameObject menu;
    public GameObject credits;
    public GameObject splashArt;
    public float splashArtTime = 2f;

    [Header("Skip")]
    public KeyCode skipKey = KeyCode.F1;
    public TextMeshProUGUI skipText;
    public float skipTextAnimationTime = 3f;

    [Header("Buttons")]
    public Button playButton;
    public Button creditsButton;
    public Button quitButton;
    public Button goBackButton;
    public KeyCode goBackKey = KeyCode.Escape;

    private bool notInMainMenu;
    private bool activeSplashArt;
    private float splashTimer;

    void Awake()
    {
        playButton.onClick.AddListener(OnClickPlayHandler);
        creditsButton.onClick.AddListener(OnClickCreditsHandler);
        quitButton.onClick.AddListener(OnClickQuitHandler);
        goBackButton.onClick.AddListener(OnClickGoBackHandler);

        skipText.text = $"Press {skipKey} to skip";

        activeSplashArt = true;
        splashArt.gameObject.SetActive(true);
        menu.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);

        StartCoroutine(SkipButtonAnimation(skipTextAnimationTime));
    }

    private void GoBack()
    {
        menu.SetActive(true);
        credits.SetActive(false);
        notInMainMenu = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(skipKey))
        {
            OnClickPlayHandler();
        }
        else if (notInMainMenu && Input.GetKeyUp(goBackKey))
        {
            GoBack();
        }

        if (activeSplashArt)
        {
            splashTimer += Time.deltaTime;
            if(splashTimer > splashArtTime)
            {
                splashArt.gameObject.SetActive(false);
                menu.gameObject.SetActive(true);
                activeSplashArt = false;
            }
        }
    }

    private void OnClickPlayHandler()
    {
        SceneManager.LoadScene(levelScene);
    }

    private void OnClickCreditsHandler()
    {
        notInMainMenu = true;
        menu.SetActive(false);
        credits.SetActive(true);
    }

    private void OnClickGoBackHandler()
    {
        GoBack();
    }

    private void OnClickQuitHandler()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnClickPlayHandler);
        creditsButton.onClick.RemoveListener(OnClickCreditsHandler);
        quitButton.onClick.RemoveListener(OnClickQuitHandler);
        goBackButton.onClick.RemoveListener(OnClickGoBackHandler);
    }

    private IEnumerator SkipButtonAnimation(float time)
    {
        Color originalColor = skipText.color;

        float t = 0f;
        while (t < 1f)
        {
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, t));
            skipText.color = newColor;
            t += Time.deltaTime / time;
            yield return null;
        }
        t = 1f;
    }
}
