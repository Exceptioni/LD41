using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card {
    
    protected Player Player { get { return GameManager.Instance.Player; } }

    public List<int> Power = new List<int>();

    public float RuneTimer;

    public GameCard GC { get; internal set; }

    public int BonusPower { get { return GameManager.Instance.BonusPower; } }

    public bool Casting = false;

    private CardPosition _position;
    public int ManaRestoredOnHit = 0;

    public CardPosition Position
    {
        get
        {
            return _position;
        }
        set
        {
            bool refreshDeck = false;
            if (_position == CardPosition.Deck || value == CardPosition.Deck)
            {
                refreshDeck = true;
            }
            _position = value;

            if (refreshDeck)
            {
                Player.RefreshDeck();
            }
        }
    }

    #region Base Data

    public virtual bool IncludeInGame { get { return true; } }

    public virtual string Name { get { return "Invalid"; } }

    public virtual CardType Type { get { return CardType.INVALID; } }

    public virtual CardElement Element { get { return CardElement.INVALID; } }

    public virtual int ManaCost { get { return 0; } }

    public virtual string EffectText { get { return "This card is Invalid"; } }

    public virtual List<int> BasePower { get { return new List<int>(); } }

    public virtual int ProvideBonusPower
    {
        get { return 0; }
    }

    public virtual float HitboxSize { get { return 0.2f; } }

    public virtual float SpeedMultiplier { get { return 1f; } }

    public Vector3 RunePosition { get; internal set; }

    #endregion

    public Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Cards/" + Type.ToString() + Name.Replace(" ", "") + "/" + Type.ToString() + Name.Replace(" ", "") + "_Sprite") as Sprite;
    }

    public GameObject GetCastEffect()
    {
        Debug.Log("Cards / " + Type.ToString() + Name.Replace(" ", "") + " / " + Type.ToString() + Name.Replace(" ", "") + "_CastEffect");
        return Resources.Load<GameObject>("Cards/" + Type.ToString() + Name.Replace(" ", "") + "/" + Type.ToString() + Name.Replace(" ", "") + "_CastEffect") as GameObject;
    }

    public GameObject GetTravelEffect()
    {
        return Resources.Load<GameObject>("Cards/" + Type.ToString() + Name.Replace(" ", "") + "/" + Type.ToString() + Name.Replace(" ", "") + "_TravelEffect") as GameObject;
    }

    public GameObject GetAfterEffect()
    {
        return Resources.Load<GameObject>("Cards/" + Type.ToString() + Name.Replace(" ", "") + "/" + Type.ToString() + Name.Replace(" ", "") + "_AfterEffect") as GameObject;
    }

    public Card()
    {
        Reset();
    }

    public void SendTo(CardPosition pos)
    {
        Position = pos;
        if(pos == CardPosition.Deck || pos == CardPosition.Discard)
        {
            Reset();
        }
    }

    public void Reset()
    {
        Power = BasePower;
        ManaRestoredOnHit = 0;
    }

    public void AddPower(int amt)
    {
        for (int i = 0; i < Power.Count; i++)
        {
            Power[i] += amt;
        }
    }

    public void SetPower(int str)
    {
        for (int i = 0; i < Power.Count; i++)
        {
            Power[i] = str;
        }
    }

    /// <summary>
    /// This is only for Runes
    /// </summary>
    public virtual void HitSpell()
    {

    }

    public virtual void RuneEffect(Card c)
    {

    }

    public virtual void OnHitEnemy(Enemy e)
    {
        Player.RestoreMana(ManaRestoredOnHit);
    }

    public string GetPowerText(int index)
    {
        int basestr = BasePower[index];
        int str = Power[index] + BonusPower;

        // Sort color

        return str.ToString();
    }

    public void Cast()
    {
        if (!CanCast()) { return; }

        switch (Type)
        {
            case CardType.Spell:
                SendTo(CardPosition.Travelling);
                break;

            case CardType.Rune:
                SendTo(CardPosition.Field);
                RuneTimer = 10f;
                RunePosition = GC.transform.position;
                break;
        }
        Player.SpendMana(ManaCost);
        Casting = false;
    }

    public bool CanCast()
    {
        return Player.Mana >= ManaCost;
    }
}

public enum CardType
{
    INVALID = 0,
    Spell = 1,
    Rune = 2,
    Buff = 3,
}

public enum CardElement
{
    INVALID = 0,
    Fire = 1,
    Earth = 2,
    Air = 3,
    Water = 4,
}

public enum CardPosition
{
    Removed = 0,
    Deck = 1,
    Hand = 2,
    Discard = 3,
    Field = 4,
    Travelling = 5
}