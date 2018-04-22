using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_SpellLavabeam : Card
{

    #region Base Data

    public override string Name { get { return "Lavabeam"; } }

    public override int ManaCost { get { return 4; } }

    public override CardType Type { get { return CardType.Spell; } }

    public override string EffectText { get { return "Deal " + GetPowerText(0) + " damage"; } }

    public override List<int> BasePower { get { return new List<int>() { 13 }; } }

    #endregion

    
    public override void OnHitEnemy(Enemy e)
    {
        base.OnHitEnemy(e);
        GC.StartEffect(GetAfterEffect(), EffectType.After);
        e.TakeDamage(Power[0] + BonusPower);
    }
    
}
