using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_RunePower : Card
{

    #region Base Data

    public override string Name { get { return "Power"; } }

    public override int ManaCost { get { return 2; } }

    public override CardType Type { get { return CardType.Rune; } }

    public override string EffectText { get { return "Add 6 power to a Spell."; } }

    #endregion

    public override void RuneEffect(Card c)
    {
        GC.StartEffect(GetAfterEffect(), EffectType.After);
        c.AddPower(6);
    }
    
}
