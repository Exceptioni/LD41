using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    public static GameUI Instance;
    
    public Transform Deck;
    public Transform Hand;
    public Transform Discard;
    public Transform Field;
    public Transform CardDump;

    void Awake()
    {
        Instance = this;
    }

    public RectTransform ManaBar;
    public Text ManaText;
    public RectTransform HealthBar;
    public Text HealthText;

    public Text EnemiesLeftText;

    public GameObject RewardsPanel;

    public GameCardUI Reward1;
    public GameCardUI Reward2;
    public GameCardUI Reward3;

    public GameObject HoverPanel;
    public Text HoverName;
    public Text HoverDesc;
    public Text DeckCount;
    public Text DiscardCount;

    public Text WaveText;

    public void ShowRewards(Card rw1, Card rw2, Card rw3)
    {
        RewardsPanel.SetActive(true);
        Reward1.Card = rw1;
        Reward2.Card = rw2;
        Reward3.Card = rw3;
    }

    private void Update()
    {
        ManaBar.sizeDelta = new Vector2((GameManager.Instance.Player.Mana / GameManager.Instance.Player.MaxMana) * 200, 30);
        ManaText.text = GameManager.Instance.Player.Mana.ToString("N1") + " / " + GameManager.Instance.Player.MaxMana.ToString("N0");
        HealthBar.sizeDelta = new Vector2(((float)GameManager.Instance.Player.Health / (float)GameManager.Instance.Player.MaxHealth) * 200, 30);
        HealthText.text = GameManager.Instance.Player.Health.ToString("N0") + " / " + GameManager.Instance.Player.MaxHealth.ToString("N0");
        EnemiesLeftText.text = GameManager.Instance.EnemiesKilledThisLevel + " / " + (GameManager.Instance.EnemiesToSpawnThisLevel);
        WaveText.text = "Wave: " + GameManager.Instance.Level + (GameManager.Instance.EndlessMode ? "" : "/10");

        EndlessButton.interactable = GameManager.Instance.CampaignBeat;

        HoverPanel.SetActive(hovering);
        hovering = false;

        DeckCount.text = "Deck: " + GameManager.Instance.Player.Deck.Count;
        DiscardCount.text = "Discard: " + GameManager.Instance.Player.DiscardPile.Count;
    }


    public GameObject MainMenuPanel;
    public GameObject ControlsPanel;

    public Button EndlessButton;
    public void ShowMenu()
    {
        MainMenuPanel.SetActive(true);
        ControlsPanel.SetActive(false);
        VictoryPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    public void HideMenu()
    {
        MainMenuPanel.SetActive(false);
        ControlsPanel.SetActive(false);
    }

    public void Play()
    {
        GameManager.Instance.EndlessMode = false;
        GameManager.Instance.StartNewGame();
        HideMenu();
    }

    public void Endless()
    {
        GameManager.Instance.EndlessMode = true;
        GameManager.Instance.StartNewGame();
        HideMenu();
    }

    public void ShowControls()
    {
        MainMenuPanel.SetActive(false);
        ControlsPanel.SetActive(true);
    }

    private bool hovering;
    public void Hover(string name, string text)
    {
        HoverName.text = name;
        HoverDesc.text = text;
        hovering = true;
    }

    public GameObject VictoryPanel;

    public void ShowVictory()
    {
        VictoryPanel.SetActive(true);
    }

    public GameObject GameOverPanel;

    public void ShowGameOver()
    {
        GameOverPanel.SetActive(true);
    }
}
