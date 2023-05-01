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
    private const string LEVE_SCENE = "Level";

    [Header("Screens")]
    public GameObject menu;
    public GameObject credits;

    [Header("Skip")]
    public KeyCode skipKey = KeyCode.F1;
    public TextMeshProUGUI skipText;
    public float skipTextAnimationTime = 3f;

    [Header("Buttons")]
    public MenuButton playButton;
    public MenuButton creditsButton;
    public MenuButton quitButton;
    public MenuButton goBackButton;
    public KeyCode goBackKey = KeyCode.Escape;

    private bool notInMainMenu;
    private MenuButton currentSelectedButton;

    void Awake()
    {
        playButton.button.onClick.AddListener(OnClickPlayHandler);
        creditsButton.button.onClick.AddListener(OnClickCreditsHandler);
        quitButton.button.onClick.AddListener(OnClickQuitHandler);
        goBackButton.button.onClick.AddListener(OnClickGoBackHandler);

        skipText.text = $"{skipKey} to skip";

        StartCoroutine(SkipButtonAnimation(skipTextAnimationTime));

        currentSelectedButton = playButton;
        GoBack();
    }

    private void GoBack()
    {
        menu.SetActive(true);
        credits.SetActive(false);
        notInMainMenu = false;
        SelectButton(currentSelectedButton);
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
    }

    private void OnClickPlayHandler()
    {
        SceneManager.LoadScene(LEVE_SCENE);
    }

    private void OnClickCreditsHandler()
    {
        currentSelectedButton = creditsButton;
        notInMainMenu = true;
        menu.SetActive(false);
        credits.SetActive(true);
        SelectButton(goBackButton);
    }

    private void SelectButton(MenuButton button)
    {
        button.button.Select();
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
        playButton.button.onClick.RemoveListener(OnClickPlayHandler);
        creditsButton.button.onClick.RemoveListener(OnClickCreditsHandler);
        quitButton.button.onClick.RemoveListener(OnClickQuitHandler);
        goBackButton.button.onClick.RemoveListener(OnClickGoBackHandler);
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
