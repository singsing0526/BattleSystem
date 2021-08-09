using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatsEffect
{
    public int round, extraDefense, extraDodgeRate, extraSpeed, extraAttackDamage;
    public Character characterStats;

    public StatsEffect(int round, Character characterStats, int extraDefense, int extraDodgeRate, int extraSpeed, int extraAttackDamage)
    {
        this.round = round;
        this.characterStats = characterStats;
        this.extraAttackDamage = extraAttackDamage;
        this.extraDefense = extraDefense;
        this.extraDodgeRate = extraDodgeRate;
        this.extraSpeed = extraSpeed;

        characterStats.extraAttackDamage += this.extraAttackDamage;
        characterStats.extraDefense += this.extraDefense;
        characterStats.extraDodgeRate += this.extraDodgeRate;
        characterStats.extraSpeed += this.extraSpeed;
    }

    public void clearStats()
    {
        characterStats.extraAttackDamage -= extraAttackDamage;
        characterStats.extraDefense -= extraDefense;
        characterStats.extraDodgeRate -= extraDodgeRate;
        characterStats.extraSpeed -= extraSpeed;
    }

    public void FinsihOneRound()
    {
        round--;
        if (round <= 0)
        {
            clearStats();
            characterStats.statsEffects.Remove(this);
        }
    }

}
