using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_SpellShadowsurge : Card
{

    #region Base Data

    public override string Name { get { return "Shadowsurge"; } }

    public override int ManaCost { get { return 1; } }

    public override CardType Type { get { return CardType.Spell; } }

    public override string EffectText { get { return "Deal " + GetPowerText(0) + " damage. Runes effect this card twice."; } }

    public override List<int> BasePower { get { return new List<int>() { 4 }; } }

    #endregion

    
    public override void OnHitEnemy(Enemy e)
    {
        base.OnHitEnemy(e);
        GC.StartEffect(GetAfterEffect(), EffectType.After);
        e.TakeDamage(Power[0] + BonusPower);
    }
    
}
