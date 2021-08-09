using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int maxHP, currentHP, maxMP, currentMP, defense, dodgeRate, speed, attackDamage, ID, wave;
    public int extraDefense, extraDodgeRate, extraSpeed, extraAttackDamage, shieldPoint;
    public enum Element {none, fire, water, wind, earth, electricity, ice, rain, stone, wildfire, chaos};
    public Element element;
    public bool isAlly, isDead = false;
    public SceneCharacter sceneCharacter;
    public List<Skill> skills;
    public GameObject elementPrefab;
    public List<GameObject> effects;
    public GameObject textPrefab;
    public List<StatsEffect> statsEffects;

    public Character(int maxHP, int maxMP, int defense, int dodgeRate, int speed, int attackDamage, Element element, int ID, int wave)
    {

        this.maxHP = maxHP;
        currentHP = maxHP;
        this.maxMP = maxMP;
        currentMP = 0;
        this.defense = defense;
        this.dodgeRate = dodgeRate;
        this.speed = speed;
        this.attackDamage = attackDamage;
        this.element = element;
        this.ID = ID;
        this.wave = wave;
    }

    public void SetCharacter(int maxHP, int maxMP, int defense, int dodgeRate, int speed, int attackDamage, Element element, int ID, int wave = 0)
    {
        this.maxHP = maxHP;
        currentHP = maxHP;
        this.maxMP = maxMP;
        currentMP = 0;
        this.defense = defense;
        this.dodgeRate = dodgeRate;
        this.speed = speed;
        this.attackDamage = attackDamage;
        this.element = element;
        this.ID = ID;
        this.wave = wave;
    }

    public void AddSkill(Element element, int MPCost, string skillName = "null")
    {
        skills.Add(new Skill(element, MPCost, skillName));
    }

    public void AddEffect(int round, Element element)
    {
        if (element != Element.none)
        {
            GameObject temp = Instantiate(elementPrefab);
            temp.GetComponent<ElementEffect>().setValue(round, element, this);
            effects.Add(temp);
        }
    }

    public void AddStatsEffect(int round, int extraDefense, int extraDodgeRate, int extraSpeed, int extraAttackDamage)
    {
        statsEffects.Add(new StatsEffect(round, this, extraDefense, extraDodgeRate, extraSpeed, extraAttackDamage));
    }

}
