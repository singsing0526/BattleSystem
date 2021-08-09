using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenu : MonoBehaviour
{
    public SpriteRenderer sr;
    public BattleMenu battleMenu;
    public ItemMenu itemMenu;
    public Sprite[] itemSprites;
    public bool isDescription, isArrow;
    [HideInInspector]public GameObject description1, description2, arrowHolder;
    public GameObject itemAmountPrefab;
    public Sprite upArrow, downArrow;
    [HideInInspector]public TMPro.TextMeshProUGUI itemAmountHolder;
    public int descriptionID, scroller = 0;

    private void Start()
    {
        transform.SetParent(battleMenu.transform);
        sr = GetComponent<SpriteRenderer>();
        if (isDescription == false && isArrow == false)
        {
            sr.sortingLayerName = "menu";
            battleMenu.instructionHolder.text = "[W], [Up] and [S], [Down] to scroll, [Z] to comfirm, [X] to cancel";
            description1 = Instantiate(gameObject, transform.position, Quaternion.identity);
            description1.GetComponent<ItemMenu>().isDescription = true;
            description1.GetComponent<ItemMenu>().descriptionID = 0;
            description1.name = "Page1";

            description2 = Instantiate(gameObject, transform.position, Quaternion.identity);
            description2.GetComponent<ItemMenu>().isDescription = true;
            description2.GetComponent<ItemMenu>().descriptionID = 1;
            description2.name = "Page2";

            arrowHolder = Instantiate(gameObject, transform.position, Quaternion.identity);
            arrowHolder.GetComponent<ItemMenu>().isArrow = true;
            arrowHolder.GetComponent<ItemMenu>().descriptionID = 2;
            arrowHolder.name = "Up";

            arrowHolder = Instantiate(gameObject, transform.position, Quaternion.identity);
            arrowHolder.GetComponent<ItemMenu>().isArrow = true;
            arrowHolder.GetComponent<ItemMenu>().descriptionID = 3;
            arrowHolder.name = "Down";
        }
        else
        {
            sr.sortingLayerName = "description";
            transform.SetParent(GameObject.Find("ItemMenu(Clone)").transform);
            itemMenu = transform.parent.GetComponent<ItemMenu>();

            if (isArrow == false)
            {
                if (descriptionID + itemMenu.scroller < battleMenu.database.inventory.Count)
                {
                    sr.sprite = itemSprites[battleMenu.database.inventory[descriptionID + itemMenu.scroller].ID];
                    itemAmountHolder = Instantiate(itemAmountPrefab, transform.position, Quaternion.identity).GetComponent<TMPro.TextMeshProUGUI>();
                    itemAmountHolder.transform.SetParent(GameObject.Find("Canvas").transform);
                    itemAmountHolder.text = "x" + battleMenu.database.inventory[itemMenu.scroller + descriptionID].itemAmount;
                    itemAmountHolder.transform.position += new Vector3(3.25f, 1.25f + descriptionID * -1.55f, 0);
                }
                else
                {
                    Destroy(gameObject);
                }
                if (descriptionID == 0)
                {
                    transform.position = (Vector2)transform.position + new Vector2(0, 0.75f);
                }
                else
                {
                    transform.position = (Vector2)transform.position + new Vector2(0, -0.75f);
                    sr.color = new Color32(170, 70, 200, 255);
                }
            }
            else
            {
                if (descriptionID == 2)
                {
                    transform.position = (Vector2)transform.position + new Vector2(3.5f, 2);
                    sr.sprite = upArrow;
                }
                else
                {
                    transform.position = (Vector2)transform.position + new Vector2(3.5f, -2);
                    sr.sprite = downArrow;
                }
            }
        }
    }

    private void Update()
    {
        if (isDescription == true)
        {
            int temp = itemMenu.scroller + descriptionID;
            if (temp >= battleMenu.database.inventory.Count)
            {
                sr.sprite = null;
                itemAmountHolder.text = "";
            }
            else
            {
                sr.sprite = itemSprites[battleMenu.database.inventory[temp].ID];
                itemAmountHolder.text = "x" + battleMenu.database.inventory[temp].itemAmount;
            }
        }
        else if (isArrow == true)
        {
            if (descriptionID == 2)
            {
                if (itemMenu.scroller > 0)
                {
                    sr.sprite = upArrow;
                }
                else
                {
                    sr.sprite = null;
                }
            }
            else
            {
                if (itemMenu.scroller + 1 < battleMenu.database.inventory.Count)
                {
                    sr.sprite = downArrow;
                }
                else
                {
                    sr.sprite = null;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                battleMenu.instructionHolder.text = "[W], [Up] and [S], [Down] to scroll, [Z] to comfirm";
                battleMenu.isSelectedOption = false;
                if (description1 != null)
                    Destroy(description1.GetComponent<ItemMenu>().itemAmountHolder.gameObject);
                if (description2 != null)
                    Destroy(description2.GetComponent<ItemMenu>().itemAmountHolder.gameObject);
                Destroy(gameObject);
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (scroller + 2 <= battleMenu.database.inventory.Count)
                {
                    scroller++;
                }
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (scroller - 1 >= 0)
                {
                    scroller--;
                }
            }
            if (Input.GetKeyDown(KeyCode.Z) && battleMenu.database.inventory.Count != 0)
            {
                battleMenu.currentItem = scroller;
                battleMenu.isSelectedItem = true;
                if (description1 != null)
                    Destroy(description1.GetComponent<ItemMenu>().itemAmountHolder.gameObject);
                if (description2 != null)
                    Destroy(description2.GetComponent<ItemMenu>().itemAmountHolder.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
