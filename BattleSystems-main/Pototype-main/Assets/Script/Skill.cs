using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public Character.Element element;
    public string skillName;
    public int MPCost, ID;

    public Skill(Character.Element element, int MPCost, string skillName = "null")
    {
        this.element = element;
        this.MPCost = MPCost;
        this.skillName = skillName;
        ID = getSkillID(skillName);
    }

    public int getSkillID(string skillName)
    {
        switch (skillName)
        {
            case "Dodge":
                return 0;
            case "AgainstTheCurrent":
                return 1;
            case "Fireball":
                return 2;
            case "Explosion":
                return 3;
            case "Heal":
                return 4;
            case "Flush":
                return 5;
            case "Shield":
                return 6;
            case "Strand":
                return 7;
            case "DoubleDagger":
                return 8;
            case "LastShock":
                return 9;
        }
        return 0;
    }
}
