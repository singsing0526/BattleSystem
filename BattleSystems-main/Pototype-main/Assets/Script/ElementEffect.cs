using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementEffect : MonoBehaviour
{
    public Character.Element element;
    public int round;
    public Character characterStats;
    public Sprite[] elementImages;
    public TMPro.TextMeshProUGUI roundIndicatorHolder;

    private int previousRepeatRate, extraDefense = 0, extraDodgeRate = 0, extraSpeed = 0, extraAttackDamage = 0;
    private SpriteRenderer sr;
    private bool isDeleted = false, isDeleteUp = false;

    private int getSpriteIndex(Character.Element element)
    {
        switch (element)
        {
            case Character.Element.chaos:
                return 0;
            case Character.Element.earth:
                return 1;
            case Character.Element.electricity:
                return 2;
            case Character.Element.fire:
                return 3;
            case Character.Element.ice:
                return 4;
            case Character.Element.rain:
                return 5;
            case Character.Element.stone:
                return 6;
            case Character.Element.water:
                return 7;
            case Character.Element.wildfire:
                return 8;
            case Character.Element.wind:
                return 9;
            default:
                return 0;
        }
    }

    public void setValue(int round, Character.Element element, Character characterStats)
    {

        this.round = round;
        this.element = element;
        this.characterStats = characterStats;
        sr = GetComponent<SpriteRenderer>();

        for (int i = 0; i < characterStats.effects.Count; i++)
        {
            ElementEffect tempElememt = characterStats.effects[i].GetComponent<ElementEffect>();
            if (getMixableElement(element, tempElememt.element) != Character.Element.none)
            {
                this.element = getMixableElement(element, tempElememt.element);
                tempElememt.isDeleted = true;
                tempElememt.isDeleteUp = true;
                tempElememt.StartCoroutine("SlideDelete");
            }
            else if (tempElememt.element == element)
            {
                this.round = tempElememt.round + round;
                extraAttackDamage = tempElememt.extraAttackDamage;
                extraDefense = tempElememt.extraDefense;
                extraDodgeRate = tempElememt.extraDodgeRate;
                extraSpeed = tempElememt.extraSpeed;
                characterStats.extraAttackDamage -= extraAttackDamage;
                characterStats.extraDefense -= extraDefense;
                characterStats.extraDodgeRate -= extraDodgeRate;
                characterStats.extraSpeed -= extraSpeed;
                tempElememt.isDeleted = true;
                tempElememt.isDeleteUp = true;
                tempElememt.StartCoroutine("SlideDelete");
            }
        }

        sr.sprite = elementImages[getSpriteIndex(this.element)];
        previousRepeatRate = characterStats.sceneCharacter.barCharacter.repeatRate;
        roundIndicatorHolder = Instantiate(characterStats.textPrefab).GetComponent<TMPro.TextMeshProUGUI>();
        roundIndicatorHolder.text = round.ToString();
        roundIndicatorHolder.transform.SetParent(GameObject.Find("Canvas").transform);
        executeEffects();
    }

    public IEnumerator SlideDelete()
    {
        ClearExtra();
        if (roundIndicatorHolder != null)
        {
            Destroy(roundIndicatorHolder.gameObject);
        }
        characterStats.effects.Remove(gameObject);
        if (isDeleteUp == true)
        {
            for (int i = 0; i < 10; i++)
            {
                transform.position += new Vector3(0, 0.1f, 0);
                yield return null;
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                transform.position += new Vector3(0.1f, 0, 0);
                yield return null;
            }
        }
        Destroy(gameObject);
    }

    public Character.Element getMixableElement(Character.Element currentElement, Character.Element existingElement)
    {
        if (currentElement == Character.Element.earth && existingElement == Character.Element.fire || currentElement == Character.Element.fire && existingElement == Character.Element.earth)
            return Character.Element.stone;
        if (currentElement == Character.Element.water && existingElement == Character.Element.electricity || currentElement == Character.Element.water && existingElement == Character.Element.electricity)
            return Character.Element.rain;
        if (currentElement == Character.Element.fire && existingElement == Character.Element.wind || currentElement == Character.Element.wind && existingElement == Character.Element.fire)
            return Character.Element.wildfire;
        if (currentElement == Character.Element.water && existingElement == Character.Element.wind || currentElement == Character.Element.wind && existingElement == Character.Element.water)
            return Character.Element.ice;
        if (currentElement == Character.Element.earth && existingElement == Character.Element.electricity || currentElement == Character.Element.electricity && existingElement == Character.Element.earth)
            return Character.Element.chaos;
        if (currentElement == Character.Element.fire && existingElement == Character.Element.water || currentElement == Character.Element.water && existingElement == Character.Element.fire)
            return Character.Element.chaos;
        return Character.Element.none;

    }

    public int getFInalAttackDamage(int attackDamage, int denfense)
    {
        if (attackDamage - denfense <= 0)
        {
            return 1;
        }
        return attackDamage - denfense;
    }

    public void executeEffects()
    {
        switch (element)
        {
            case Character.Element.fire:
                characterStats.currentHP -= getFInalAttackDamage(10, characterStats.defense + characterStats.extraAttackDamage);
                break;
            case Character.Element.wind:
                characterStats.extraSpeed -= 1;
                extraSpeed -= 1;
                break;
            case Character.Element.water:
                characterStats.extraAttackDamage -= 5;
                extraAttackDamage -= 5;
                break;
            case Character.Element.earth:
                characterStats.extraDefense -= 5;
                extraDefense -= 5;
                break;
            case Character.Element.electricity:
                for(int i = 0; i < characterStats.effects.Count; i++)
                {
                    if (characterStats.effects[i].GetComponent<ElementEffect>().element == Character.Element.water)
                    {
                        characterStats.currentHP -= getFInalAttackDamage(30, characterStats.defense + characterStats.extraAttackDamage);
                    }
                }
                break;
            case Character.Element.rain:
                characterStats.extraDodgeRate -= 5;
                characterStats.currentHP -= getFInalAttackDamage(10, characterStats.defense + characterStats.extraDefense);
                if (characterStats.isAlly == true)
                {
                    for (int i = 0; i < characterStats.sceneCharacter.database.allyDetails.Count; i++)
                    {
                        characterStats.sceneCharacter.database.allyDetails[i].GetComponent<Character>().AddEffect(1, Character.Element.water);
                    }
                }
                else
                {
                    for (int i = 0; i < characterStats.sceneCharacter.database.enemyDetails.Count; i++)
                    {
                        characterStats.sceneCharacter.database.enemyDetails[i].GetComponent<Character>().AddEffect(1, Character.Element.water);
                    }
                }
                break;
            case Character.Element.ice:
                characterStats.extraSpeed -= 4;
                extraSpeed -= 4;
                break;
            case Character.Element.stone:
                characterStats.sceneCharacter.barCharacter.progress = -10 * round;
                round = 0;
                roundIndicatorHolder.text = "0";
                break;
            case Character.Element.wildfire:
                if (characterStats.isAlly == true)
                {
                    for (int i = 0; i < characterStats.sceneCharacter.database.allyDetails.Count; i++)
                    {
                        characterStats.sceneCharacter.database.allyDetails[i].GetComponent<Character>().AddEffect(1, Character.Element.fire);
                    }
                }
                else
                {
                    for (int i = 0; i < characterStats.sceneCharacter.database.enemyDetails.Count; i++)
                    {
                        characterStats.sceneCharacter.database.enemyDetails[i].GetComponent<Character>().AddEffect(1, Character.Element.fire);
                    }
                }
                break;
            case Character.Element.chaos:
                if (round >= 2)
                {
                    round -= 2;
                    characterStats.currentHP -= getFInalAttackDamage(30, characterStats.defense + characterStats.extraAttackDamage);
                }
                break;
            default:
                Debug.Log("Unspecificied Element.");
                break;
        }
    }

    public void ClearExtra()
    {
        characterStats.extraAttackDamage -= extraAttackDamage;
        characterStats.extraDefense -= extraDefense;
        characterStats.extraDodgeRate -= extraDodgeRate;
        characterStats.extraSpeed -= extraSpeed;
    }

    private void LateUpdate()
    {
        if (isDeleted == false)
        {
            if (characterStats.sceneCharacter != null)
            {
                transform.position = characterStats.sceneCharacter.transform.position;
                transform.position += new Vector3(1, characterStats.effects.IndexOf(gameObject), 0);
                if (roundIndicatorHolder != null)
                {
                    roundIndicatorHolder.transform.position = transform.position;
                    roundIndicatorHolder.transform.position += new Vector3(0.35f, -0.2f, 0);
                }
                if (round <= 0 || characterStats.isDead == true)
                {
                    isDeleted = true;
                    StartCoroutine("SlideDelete");
                }
                if (previousRepeatRate != characterStats.sceneCharacter.barCharacter.repeatRate)
                {
                    round--;
                    roundIndicatorHolder.text = round.ToString();
                    previousRepeatRate = characterStats.sceneCharacter.barCharacter.repeatRate;
                    executeEffects();
                }
            }
            else
            {
                if (isDeleted == false)
                {
                    isDeleted = true;
                    StartCoroutine("SlideDelete");
                }
            }
        }
    }
}
