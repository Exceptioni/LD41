using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player {

    public float Mana;
    public float BaseManaRegen = 1f;
    private float ManaRegen
    {
        get
        {
            return BaseManaRegen;
        }
    }
    public int Health;
    public int MaxHealth;

    public List<Card> BaseDeck = new List<Card>();
    public List<Card> AllCards = new List<Card>();
    public List<Card> Deck = new List<Card>();

    public List<Card> DiscardPile
    {
        get
        {
            return AllCards.Where(m => m.Position == CardPosition.Discard).ToList();
        }
    }

    public void TakeDamage(int strength)
    {
        Health -= strength;

        if(Health <= 0)
        {
            GameManager.Instance.GameOver();
            Health = 0;
        }
    }

    public void RestoreHealth(int v)
    {
        Health += v;

        if (Health >= 25)
        {
            Health = 25;
        }
    }

    public List<Card> Hand
    {
        get
        {
            return AllCards.Where(m => m.Position == CardPosition.Hand).ToList();
        }
    }

    public List<Card> Field
    {
        get
        {
            return AllCards.Where(m => m.Position == CardPosition.Field).ToList();
        }
    }

    public float MaxMana;

    public Player()
    {
        BaseDeck = CardDatabase.Instance.GetBaseDeck();

        MaxMana = 10;
        Mana = 10;

        foreach (Card c in BaseDeck)
        {
            CardDatabase.Instance.SpawnNewCard(c);
        }
    }

    public void Draw(int amt)
    {
        for (int i = 0; i < amt; i++)
        {
            Draw();
        }
    }

    public void Draw()
    {
        if (Deck.Count == 0)
        {
            ReshuffleDeck();
        }

        if(Deck.Count > 0)
        {
            Deck[0].SendTo(CardPosition.Hand);
        }
    }

    public void ReshuffleDeck()
    {
        while (DiscardPile.Count > 0)
        {
            DiscardPile[0].SendTo(CardPosition.Deck);
        }
        RefreshDeck();
    }

    public void AddCardToDeck(Card gameCard)
    {
        BaseDeck.Add(gameCard);
        CardDatabase.Instance.SpawnNewCard(gameCard);
    }

    public void Reset()
    {
        AllCards = BaseDeck;
        AllCards.ForEach(m => m.SendTo(CardPosition.Deck));
        Health = 25;
        MaxHealth = 25;
        RefreshDeck();
    }

    public void Refresh()
    {
        AllCards.ForEach(m => m.SendTo(CardPosition.Deck));

        Mana = MaxMana;

        RefreshDeck();
        Deck.Shuffle();
    }

    public void GenerateMana()
    {
        Mana += ManaRegen * Time.deltaTime;
        Mana = Mathf.Clamp(Mana, 0, MaxMana);
    }

    public void SpendMana(int amt)
    {
        Mana -= amt;
        Mana = Mathf.Clamp(Mana, 0, MaxMana);
    }

    private void ShuffleDeck()
    {
        Deck.Shuffle();
    }

    public void RefreshDeck()
    {
        Deck = AllCards.Where(m => m.Position == CardPosition.Deck).ToList();
        ShuffleDeck();
    }

    public void RestoreMana(int manaRestored)
    {
        Mana += manaRestored;
        Mana = Mathf.Clamp(Mana, 0, MaxMana);
    }
}
