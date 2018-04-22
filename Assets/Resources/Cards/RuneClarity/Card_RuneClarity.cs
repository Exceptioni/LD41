using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_RuneClarity : Card
{

    #region Base Data

    public override string Name { get { return "Clarity"; } }

    public override int ManaCost { get { return 1; } }

    public override CardType Type { get { return CardType.Rune; } }

    public override string EffectText { get { return "Whenever a buffed Spell hits an enemy, restore 1 mana."; } }

    #endregion

    public override void RuneEffect(Card c)
    {
        GC.StartEffect(GetAfterEffect(), EffectType.After);
        c.ManaRestoredOnHit += 1;
    }
    
}
