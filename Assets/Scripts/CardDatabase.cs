using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{

    public static CardDatabase Instance;

    public List<Card> Cards = new List<Card>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        List<Type> crds = FindSubClassesOf<Card>().ToList();
        foreach (Type t in crds)
        {
            Card instance = (Card)Activator.CreateInstance(t);
            if (instance.IncludeInGame)
            {
                Cards.Add(instance);
            }
        }
    }

    public GameObject GameCard;

    public Transform CardDump;

    public GameCard SpawnNewCard(Card c)
    {
        GameObject nc = Instantiate(GameCard) as GameObject;

        nc.transform.parent = CardDump;

        nc.GetComponent<GameCard>().SetCard(c);

        return nc.GetComponent<GameCard>();
    }

    public IEnumerable<Type> FindSubClassesOf<TBaseType>()
    {
        var baseType = typeof(TBaseType);
        var assembly = baseType.Assembly;

        return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
    }

    public Card GetRandomCardForLoot()
    {
        Card nc = (Card)Activator.CreateInstance(Cards.Where(m => m.Name != "Fireball").ToList()[UnityEngine.Random.Range(0, Cards.Count(m => m.Name != "Fireball"))].GetType());

        return nc;
    }

    public List<Card> GetBaseDeck()
    {
        List<Card> nc = new List<Card>()
        {
            new Card_SpellFireball(),
            new Card_SpellFireball(),
            new Card_SpellFireball(),
            new Card_SpellFireball(),
            new Card_SpellFireball(),
            new Card_SpellFireball(),
        };

        return nc;
    }
}
