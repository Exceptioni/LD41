using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Player Player;

    public Transform PlayerTransform;

    public static GameManager Instance;

    public GameState State = GameState.Menu;

    public int Level;
    public int EnemiesSpawnedThisLevel;

    public int BonusPower
    {
        get
        {
            return Player.Field.Count > 0 ? Player.Field.Sum(m => m.ProvideBonusPower) : 0;
        }
    }

    public int EnemiesKilledThisLevel;

    public const float CastRange = 3f;

    public List<Card> ActiveBuffs;

    void Awake () {
        Instance = this;
        CampaignBeat = PlayerPrefs.GetInt("CampaignBeat", 0) == 1;
    }

    private void Start()
    {
        Player = new Player();
        Player.Reset();

        Player.Draw(4);
        Level = 1;
        SetState(GameState.Menu);
    }

    private float SpawnTimer;

    public void RemoveEnemy(GameEnemy e)
    {
        Enemies.Remove(e);

        EnemiesKilledThisLevel++;
    }

    public int EnemiesToSpawnThisLevel
    {
        get
        {
            return (Level * 10) + 10;
        }
    }

    public bool EndlessMode { get; internal set; }
    public bool CampaignBeat { get; internal set; }

    private float SpawnRate = 1.75f;
    void Update ()
    {
        if (State == GameState.Playing)
        {
            for (int i = 0; i < Player.Hand.Count(m => !m.Casting); i++)
            {
                Vector3 targetPos = new Vector3(i - (Player.Hand.Count(m => !m.Casting) * 0.5f), 0, 0);
                Player.Hand.Where(m => !m.Casting).ToList()[i].GC.gameObject.transform.localPosition = Vector3.Lerp(Player.Hand.Where(m => !m.Casting).ToList()[i].GC.gameObject.transform.localPosition, targetPos, 20f * Time.deltaTime);
            }

            if (Player.Hand.Count < 4)
            {
                Player.Draw();
            }

            Player.GenerateMana();

            if (EnemiesSpawnedThisLevel < EnemiesToSpawnThisLevel)
            {
                SpawnTimer += Time.deltaTime;
                if (SpawnTimer >= SpawnRate)
                {
                    SpawnTimer -= SpawnRate;

                    SpawnEnemy();
                    EnemiesSpawnedThisLevel++;
                }
            }
            // ;)
            // if (Input.GetKeyDown(KeyCode.K)) { EnemiesKilledThisLevel = EnemiesToSpawnThisLevel; }
            if (EnemiesKilledThisLevel == EnemiesToSpawnThisLevel)
            {
                if (!EndlessMode)
                {
                    if(Level == 10)
                    {
                        CampaignBeat = true;
                        PlayerPrefs.SetInt("CampaignBeat", 1);

                        ShowVictory();
                    }
                    else
                    {
                        ShowRewards();
                    }
                }
                else
                {
                    ShowRewards();
                }
            }
        }
	}

    private void ShowVictory()
    {
        SetState(GameState.Victory);

        GameUI.Instance.ShowVictory();
    }

    private void ShowRewards()
    {
        SetState(GameState.Reward);
        Rewards.Clear();
        Card c1 = CardDatabase.Instance.GetRandomCardForLoot();
        Card c2 = CardDatabase.Instance.GetRandomCardForLoot();
        Card c3 = CardDatabase.Instance.GetRandomCardForLoot();
        Rewards.Add(c1);
        Rewards.Add(c2);
        Rewards.Add(c3);

        GameUI.Instance.ShowRewards(c1, c2, c3);
    }

    public List<Card> Rewards = new List<Card>();
    public void ChooseReward(int i)
    {
        Player.AddCardToDeck(Rewards[i]);
        GameUI.Instance.RewardsPanel.SetActive(false);

        StartNextWave();
    }

    public void StartNewGame()
    {
        Level = 0;

        foreach(Card c in Player.AllCards)
        {
            Destroy(c.GC.gameObject);
        }

        Player = new Player();
        Player.Reset();

        Player.Draw(4);

        StartNextWave();
    }

    private void StartNextWave()
    {
        Level++;

        EnemiesKilledThisLevel = 0;
        EnemiesSpawnedThisLevel = 0;

        Player.Refresh();

        SetState(GameState.Playing);
    }

    public void GameOver()
    {
        if (State != GameState.Playing) return;
        SetState(GameState.GameOver);

        GameUI.Instance.ShowGameOver();
    }

    public void SetState(GameState st)
    {
        State = st;

        if(st == GameState.Menu)
        {

            GameUI.Instance.ShowMenu();
        }
        else { GameUI.Instance.HideMenu(); }
    }

    private List<GameEnemy> Enemies = new List<GameEnemy>();
    public GameObject GameEnemy;
    public void SpawnEnemy()
    {
        GameObject nc = Instantiate(GameEnemy) as GameObject;

        GameEnemy ge = nc.GetComponent<GameEnemy>();
        ge.SetEnemy(new Enemy_Orc());
        ge.Enemy.MaxHealth *= Level;
        ge.Enemy.Health = ge.Enemy.MaxHealth;

        nc.transform.position = new Vector3(UnityEngine.Random.Range(-25f, -15f), 20, 0);

        Enemies.Add(ge);
    }
}

public enum GameState
{
    Menu,
    Playing,
    Reward,
    GameOver,
    Victory
}
