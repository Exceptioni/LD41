﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_SpellFireball : Card
{

    #region Base Data

    public override string Name { get { return "Fireball"; } }

    public override int ManaCost { get { return 1; } }

    public override CardType Type { get { return CardType.Spell; } }

    public override string EffectText { get { return "Deal " + GetPowerText(0) + " damage"; } }

    public override List<int> BasePower { get { return new List<int>() { 3 }; } }

    #endregion

    
    public override void OnHitEnemy(Enemy e)
    {
        base.OnHitEnemy(e);
        GC.StartEffect(GetAfterEffect(), EffectType.After);
        e.TakeDamage(Power[0] + BonusPower);
    }
    
}