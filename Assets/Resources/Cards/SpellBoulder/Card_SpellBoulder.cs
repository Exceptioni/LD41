using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_SpellBoulder : Card
{

    #region Base Data

    public override string Name { get { return "Boulder"; } }

    public override int ManaCost { get { return 3; } }

    public override CardType Type { get { return CardType.Spell; } }

    public override string EffectText { get { return "Deal " + GetPowerText(0) + " damage. Has a larger hitbox and travels slower."; } }

    public override List<int> BasePower { get { return new List<int>() { 7 }; } }

    public override float HitboxSize { get { return 0.4f; } }

    public override float SpeedMultiplier { get { return 0.25f; } }

    #endregion


    public override void OnHitEnemy(Enemy e)
    {
        base.OnHitEnemy(e);
        e.TakeDamage(Power[0] + BonusPower);
    }
    
}
