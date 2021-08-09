using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public int itemAmount = 0, ID;

    public Item(string name, int amount)
    {
        itemName = name;
        itemAmount = amount;
        ID = getID(name);
    }

    private int getID(string name)
    {
        switch (name)
        {
            case "HP Potion":
                return 0;
            case "MP Potion":
                return 1;
            case "Speed Potion":
                return 2;
            case "Strength Potion":
                return 3;
            case "Revive Potion":
                return 4;
        }
        return 0;
    }
}
