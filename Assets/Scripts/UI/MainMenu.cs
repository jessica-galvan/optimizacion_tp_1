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
        playButton.button.onClick.AddListener(SelectButtonSound);
        creditsButton.button.onClick.AddListener(SelectButtonSound);
        quitButton.button.onClick.AddListener(SelectButtonSound);
        goBackButton.button.onClick.AddListener(SelectButtonSound);

        playButton.button.onClick.AddListener(OnClickPlayHandler);
        creditsButton.button.onClick.AddListener(OnClickCreditsHandler);
        quitButton.button.onClick.AddListener(OnClickQuitHandler);
        goBackButton.button.onClick.AddListener(OnClickGoBackHandler);

        skipText.text = $"{skipKey} to skip";
    }

    private void Start()
    {
        AudioManager.instance.PlayMusic(AudioManager.instance.soundReferences.mainMenu);

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

    private void SelectButtonSound()
    {
        AudioManager.instance.PlaySFXSound(AudioManager.instance.soundReferences.selectButton);
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
        playButton.button.onClick.RemoveAllListeners();
        creditsButton.button.onClick.RemoveAllListeners();
        quitButton.button.onClick.RemoveAllListeners();
        goBackButton.button.onClick.RemoveAllListeners();
    }
}
