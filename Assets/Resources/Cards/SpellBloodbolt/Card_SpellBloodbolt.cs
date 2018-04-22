using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_SpellBloodbolt : Card
{

    #region Base Data

    public override string Name { get { return "Bloodbolt"; } }

    public override int ManaCost { get { return 5; } }

    public override CardType Type { get { return CardType.Spell; } }

    public override string EffectText { get { return "Deal " + GetPowerText(0) + " damage. If this Spell kills an enemy, restore " + GetPowerText(1) + " health."; } }

    public override List<int> BasePower { get { return new List<int>() { 11, 1 }; } }

    #endregion

    
    public override void OnHitEnemy(Enemy e)
    {
        base.OnHitEnemy(e);
        GC.StartEffect(GetAfterEffect(), EffectType.After);
        if(e.TakeDamage(Power[0] + BonusPower))
        {
            Player.RestoreHealth(Power[0] + BonusPower);
        }
    }
    
}
