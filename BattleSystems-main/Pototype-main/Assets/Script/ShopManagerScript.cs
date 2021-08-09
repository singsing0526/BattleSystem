using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManagerScript : MonoBehaviour
{
    public int[,] shopItems = new int[6,6];
    public int[,] shopSkills = new int[6, 6];
    [HideInInspector] public Text ConinsTXT;
    [HideInInspector] public int currentSelection = 0;
    public GameObject selectionPrefab, selectionHolder;
    [HideInInspector] public Database database;
    [HideInInspector] public CrossSceneManagement CSM;
    private TMPro.TextMeshProUGUI textHolder;
    private bool isItemScene = true;

    public GameObject Item1;
    public GameObject Item2;
    public GameObject Item3;
    public GameObject Item4;
    public GameObject Item5;

    public GameObject Skill1;
    public GameObject Skill2;
    public GameObject Skill3;
    public GameObject Skill4;
    public GameObject Skill5;

    public GameObject NextButton;
    public GameObject BackButton;

    public Text title;

    public void Next()
    {
        isItemScene = false;
        currentSelection = 0;
        selectionHolder.transform.position = GetSkillPosition(currentSelection);

        Item1.SetActive(false);
        Item2.SetActive(false);
        Item3.SetActive(false);
        Item4.SetActive(false);
        Item5.SetActive(false);

        Skill1.SetActive(true);
        Skill2.SetActive(true);
        Skill3.SetActive(true);
        Skill4.SetActive(true);
        Skill5.SetActive(true);

        title.text = "Skill";

        NextButton.SetActive(false);
        BackButton.SetActive(true);

    }

    public void back()
    {
        isItemScene = true;
        currentSelection = 0;
        selectionHolder.transform.position = GetItemPosition(currentSelection);

        Item1.SetActive(true);
        Item2.SetActive(true);
        Item3.SetActive(true);
        Item4.SetActive(true);
        Item5.SetActive(true);

        Skill1.SetActive(false);
        Skill2.SetActive(false);
        Skill3.SetActive(false);
        Skill4.SetActive(false);
        Skill5.SetActive(false);

        title.text = "Item";

        NextButton.SetActive(true);
        BackButton.SetActive(false);
    }

    void Awake()
    {
        CSM = GameObject.Find("CrossSceneManager").GetComponent<CrossSceneManagement>();
        database = CSM.transform.GetChild(0).GetComponent<Database>();
        textHolder = Instantiate(database.i).GetComponent<TMPro.TextMeshProUGUI>();
        textHolder.text = "[A] [D] to select, [Z] to buy, [X] to exit";
        textHolder.transform.SetParent(transform.GetChild(0).GetChild(0));
        ConinsTXT.text = "Coins: $" + database.coin.ToString();

        //IDs
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;
        shopItems[1, 5] = 5;

        //price
        shopItems[2, 1] = 100;
        shopItems[2, 2] = 100;
        shopItems[2, 3] = 150;
        shopItems[2, 4] = 150;
        shopItems[2, 5] = 300;

        //quantity
        SetItemAmount("HP Potion", 1);
        SetItemAmount("MP Potion", 2);
        SetItemAmount("Strength Potion", 3);
        SetItemAmount("Speed Potion", 4);
        SetItemAmount("Revive Potion", 5);

        //SkillIDs
        shopSkills[1, 1] = 1;
        shopSkills[1, 2] = 2;
        shopSkills[1, 3] = 3;
        shopSkills[1, 4] = 4;
        shopSkills[1, 5] = 5;


        //Skillprice
        shopSkills[2, 1] = 1000;
        shopSkills[2, 2] = 1000;
        shopSkills[2, 3] = 1500;
        shopSkills[2, 4] = 1500;
        shopSkills[2, 5] = 3000;


        //Skillquantity
        shopSkills[3, 1] = 0;
        shopSkills[3, 2] = 0;
        shopSkills[3, 3] = 0;
        shopSkills[3, 4] = 0;
        shopSkills[3, 5] = 0;

        selectionHolder = Instantiate(selectionPrefab);
        selectionHolder.transform.position = GetItemPosition(currentSelection);
    }

    private void SetItemAmount(string itemName, int ID)
    {
        bool isFound = false;
        for (int i = 0; i < database.inventory.Count; i++)
        {
            if (database.inventory[i].itemName == itemName)
            {
                isFound = true;
                shopItems[3, ID] = database.inventory[i].itemAmount;
            }
        }
        if (isFound == false)
        {
            shopItems[3, ID] = 0;
        }
    }

    public Vector2 GetItemPosition(int itemIndex)
    {
            selectionHolder.transform.SetParent(transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(itemIndex).GetChild(2));
            return transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(itemIndex).GetChild(3).position;
    }

    public Vector2 GetSkillPosition(int itemIndex)
    {
            selectionHolder.transform.SetParent(transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(itemIndex + 5).GetChild(2));
            return transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(itemIndex + 5).GetChild(2).position;
    }

    public void BuyItem(int selectedIndex = -1)
    {
        GameObject ButtonRef;
        if (selectedIndex == -1)
        {
            ButtonRef = transform.parent.GetComponent<EventSystem>().currentSelectedGameObject;
            Debug.Log(ButtonRef);
        }
        else
        {
            ButtonRef = transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(selectedIndex).gameObject;

        }
        ButtonInfo tempButton = ButtonRef.GetComponent<ButtonInfo>();

        if (database.coin >= shopItems[2, tempButton.ItemID])
        {
            database.coin -= shopItems[2, tempButton.ItemID];
            shopItems[3, tempButton.ItemID]++;
            database.AddItemToInventory(tempButton.Itemdis.text, 1);
            ConinsTXT.text = "Coins: $" + database.coin.ToString();
            ButtonRef.GetComponent<ButtonInfo>().QuantityTxt.text = shopItems[3, tempButton.ItemID].ToString();
            ButtonRef.GetComponent<ButtonInfo>().updateText();
        }
    }

    public void BuySkill(int selectedIndex = -1)
    {
        GameObject ButtonRef2;
        if (selectedIndex == -1)
        {
            ButtonRef2 = transform.parent.GetComponent<EventSystem>().currentSelectedGameObject;
        }
        else
        {
            ButtonRef2 = transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(selectedIndex + 5).gameObject;

        }
        ButtonInfo2 tempButton2 = ButtonRef2.GetComponent<ButtonInfo2>();

        if (database.coin >= shopSkills[2, tempButton2.SkillID])
        {
            database.coin -= shopSkills[2, tempButton2.SkillID];
            shopItems[3, tempButton2.SkillID]++;
            //database.AddItemToInventory(tempButton2.Skilldis.text, 1);
            ConinsTXT.text = "Coins: $" + database.coin.ToString();
            ButtonRef2.GetComponent<ButtonInfo2>().updateText2();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (currentSelection - 1 >= 0)
            {
                currentSelection--;
                if (isItemScene == true)
                     selectionHolder.transform.position = GetItemPosition(currentSelection);
                else
                    selectionHolder.transform.position = GetSkillPosition(currentSelection);
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (isItemScene == true)
            {
                if (currentSelection + 1 < 5)
                {
                    currentSelection++;
                    selectionHolder.transform.position = GetItemPosition(currentSelection);
                }
            }
            else
            {
                if (currentSelection + 1 < 5)
                {
                    currentSelection++;
                    selectionHolder.transform.position = GetSkillPosition(currentSelection);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            CSM.LoadScene("BigMap");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isItemScene == true)
                BuyItem(currentSelection);
            else
                BuySkill(currentSelection);
        }
    }


}
