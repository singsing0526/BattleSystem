using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo2 : MonoBehaviour
{
    public Text PriceTxt;
    public int SkillID;
    public Text Skilldis;
    public GameObject ShopManager;
    private ShopManagerScript SMS2;

    private void Start()
    {
        SMS2 = ShopManager.GetComponent<ShopManagerScript>();
        updateText2();
    }

    // Update is called once per frame
    public void updateText2()
    {
        PriceTxt.text = "$" + SMS2.shopSkills[2, SkillID].ToString();
    }
}