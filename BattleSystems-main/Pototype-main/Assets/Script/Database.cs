using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public bool isHandling = true, isSelectedOption = false, isAllySelected = false;
    public int selectedState = 0, selectedIndex = 0, selector = 0, selectedItem = 0, currentWave = 0, mapLerpingNumber = 0, totalWave = 0, level = 0, coin = 1000;
    public GameObject c, s, i, m;
    public GameObject[] characterSprites;
    public List<GameObject> allyDetails, enemyDetails;
    public List<Character> waitingEnemies;
    public List<Item> inventory;
    public Sprite[] backgroundMap, foregroundMap;
    public MoveForeground moveMap;

    private BattleMenu battleMenu;

    public void AddCharacterToAllyList(int maxHP, int maxMP, int defense, int dodgeRate, int speed, int attackDamage, Character.Element element, int ID)
    {
        GameObject cloner = Instantiate(c);
        cloner.GetComponent<Character>().SetCharacter(maxHP, maxMP, defense, dodgeRate, speed, attackDamage, element, ID);
        cloner.GetComponent<Character>().isAlly = true;
        cloner.name = ID.ToString();
        cloner.transform.SetParent(transform);
        allyDetails.Add(cloner);
    }

    public void AddCharacterToEnemyList(Character characterStats)
    {
        GameObject cloner = Instantiate(c);
        cloner.GetComponent<Character>().SetCharacter(characterStats.maxHP, characterStats.maxMP, characterStats.defense, characterStats.dodgeRate, characterStats.speed, characterStats.attackDamage, characterStats.element, characterStats.ID, characterStats.wave);
        Character tempCharacter = cloner.GetComponent<Character>();
        tempCharacter.isAlly = false;
        cloner.name = characterStats.ID.ToString();
        cloner.transform.SetParent(transform);
        enemyDetails.Add(cloner);
    }

    public void CreateAlly()
    {
        for (int i = 0; i < allyDetails.Count; i++)
        {
            GameObject cloner = Instantiate(s, new Vector2(-2 + i * -2, -2), Quaternion.identity);
            SceneCharacter temp = cloner.GetComponent<SceneCharacter>();
            temp.characterStats = allyDetails[i].GetComponent<Character>();
            temp.database = this;
            temp.isBarCharacter = false;
            allyDetails[i].GetComponent<Character>().sceneCharacter = temp;
            cloner.tag = "Ally";
        }
    }

    public void CreateEnemy(int maxHP, int maxMP, int defense, int dodgeRate, int speed, int attackDamage, Character.Element element, int ID)
    {
        AddCharacterToEnemyList(new Character( maxHP,  maxMP,  defense, dodgeRate, speed, attackDamage, element, ID, currentWave));

        GameObject cloner = Instantiate(s, new Vector2(2 + (enemyDetails.Count - 1) * 2, -2), Quaternion.identity);
        SceneCharacter temp = cloner.GetComponent<SceneCharacter>();
        temp.characterStats = enemyDetails[enemyDetails.Count - 1].GetComponent<Character>();
        temp.database = this;
        temp.isBarCharacter = false;
        enemyDetails[enemyDetails.Count - 1].GetComponent<Character>().sceneCharacter = temp;
        cloner.tag = "Enemy";
    }

    public void CreateEnemy()
    {
        int t = 0;
        for (int i = 0; i < waitingEnemies.Count; i++)
        {
            if (waitingEnemies[i].wave == currentWave)
            {
                AddCharacterToEnemyList(waitingEnemies[i]);
                t++;
            }
        }

        for (int i = 0; i < waitingEnemies.Count; i++)
        {
            if (t > 0)
            {
                waitingEnemies.RemoveAt(t - 1);
            }
            t--;
        }
        for (int i = 0; i < enemyDetails.Count; i++)
        {
            GameObject cloner = Instantiate(s, new Vector2(2 + i * 2, -2), Quaternion.identity);
            SceneCharacter temp = cloner.GetComponent<SceneCharacter>();
            temp.characterStats = enemyDetails[i].GetComponent<Character>();
            temp.database = this;
            temp.isBarCharacter = false;
            enemyDetails[i].GetComponent<Character>().sceneCharacter = temp;
            cloner.tag = "Enemy";
        }
        isHandling = false;
    }

    public void AddItemToInventory(string name, int amount)
    {
        bool isExist = false;
        int existingIndex = 0;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (name == inventory[i].itemName)
            {
                isExist = true;
                existingIndex = i;
            }
        }
        if (isExist == true)
        {
            inventory[existingIndex].itemAmount += amount;
        }
        else
        {
            inventory.Add(new Item(name, amount));
        }
    }

    public void CreateMap(int sceneNumber)
    {
        GameObject tempBackground = Instantiate(m);
        tempBackground.transform.position = new Vector3(0, 0, 0);
        SpriteRenderer backgroundSR = tempBackground.GetComponent<SpriteRenderer>();
        backgroundSR.sprite = backgroundMap[sceneNumber];
        backgroundSR.sortingLayerName = "background";
        tempBackground.transform.localScale = new Vector3(2.5f, 2.5f, 0);

        GameObject tempForeground = Instantiate(m);
        tempForeground.transform.position = new Vector3(0, 0, 0);
        SpriteRenderer foregroundSR = tempForeground.GetComponent<SpriteRenderer>();
        foregroundSR.sprite = foregroundMap[sceneNumber];
        foregroundSR.sortingLayerName = "foreground";
        tempForeground.transform.localScale = new Vector3(2.5f, 2.5f, 0);
        moveMap = tempForeground.GetComponent<MoveForeground>();
        moveMap.isForeground = true;
        moveMap.database = this;
    }

    public void AddEnemyDetailsToWaitingEnemies(int maxHP, int maxMP, int defense, int dodgeRate, int speed, int attackDamage, Character.Element element, int ID, int wave)
    {
        waitingEnemies.Add(new Character(maxHP, maxMP, defense, dodgeRate, speed, attackDamage, element, ID, wave));
    }

    public void SetUp()
    {
        AddCharacterToAllyList(50, 100, 10, 5, 15, 15, Character.Element.wildfire, 0);
        AddCharacterToAllyList(100, 100, 10, 5, 4, 15, Character.Element.water, 0);
        AddCharacterToAllyList(100, 100, 10, 5, 4, 15, Character.Element.earth, 0);

        allyDetails[0].GetComponent<Character>().AddSkill(Character.Element.wind, 10, "Dodge");
        allyDetails[0].GetComponent<Character>().AddSkill(Character.Element.wind, 10, "AgainstTheCurrent");
        allyDetails[0].GetComponent<Character>().AddSkill(Character.Element.fire, 10, "Fireball");
        allyDetails[0].GetComponent<Character>().AddSkill(Character.Element.fire, 10, "Explosion");
    }

    public void Initialize()
    {
        battleMenu = GameObject.Find("BattleMenu").GetComponent<BattleMenu>();
        battleMenu.sr = battleMenu.GetComponent<SpriteRenderer>();
        battleMenu.sr.enabled = false;
        battleMenu.database = this;
        
        switch (level)
        {
            case 0:
                totalWave = 1;
                CreateMap(0);
                AddEnemyDetailsToWaitingEnemies(1, 100, 10, 5, 5, 15, Character.Element.fire, 0, 0);
                AddEnemyDetailsToWaitingEnemies(1, 100, 10, 5, 6, 15, Character.Element.water, 0, 1);
                AddEnemyDetailsToWaitingEnemies(1, 100, 10, 5, 5, 15, Character.Element.earth, 0, 1);
                break;
            case 1:
                totalWave = 1;
                CreateMap(1);
                AddEnemyDetailsToWaitingEnemies(1, 100, 10, 5, 5, 15, Character.Element.fire, 0, 0);
                AddEnemyDetailsToWaitingEnemies(1, 100, 10, 5, 7, 15, Character.Element.earth, 0, 1);
                break;
            case 2:
                Debug.Log("Boss Level");
                totalWave = 1;
                CreateMap(2);
                AddEnemyDetailsToWaitingEnemies(270, 100, 10, 6, 12, 30, Character.Element.water, 1, 0);
                //AddEnemyDetailsToWaitingEnemies(1, 100, 10, 5, 7, 15, Character.Element.earth, 0, 1);
                break;
            case 4:
                totalWave = 2;
                CreateMap(3);
                AddEnemyDetailsToWaitingEnemies(1, 100, 10, 5, 5, 15, Character.Element.fire, 0, 0);
                AddEnemyDetailsToWaitingEnemies(1, 100, 10, 5, 7, 15, Character.Element.earth, 0, 1);
                break;
            default:
                totalWave = 1;
                CreateMap(1);
                AddEnemyDetailsToWaitingEnemies(1, 100, 10, 5, 5, 15, Character.Element.fire, 0, 0);
                break;
        }

        CreateAlly();
        CreateEnemy();
    }
}
