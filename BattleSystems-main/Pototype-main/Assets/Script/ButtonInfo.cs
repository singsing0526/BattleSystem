using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public int ItemID;
    public Text PriceTxt;
    public int SkillID;
    public Text QuantityTxt;
    public Text Itemdis;
    public Text Skilldis;
    public GameObject ShopManager;
    private ShopManagerScript SMS;

    private void Start()
    {
        SMS = ShopManager.GetComponent<ShopManagerScript>();
        updateText();
    }

    // Update is called once per frame
    public void updateText()
    {
        PriceTxt.text = "$" + SMS.shopItems[2, ItemID].ToString();
        QuantityTxt.text =  SMS.shopItems[3, ItemID].ToString();
    }
}
