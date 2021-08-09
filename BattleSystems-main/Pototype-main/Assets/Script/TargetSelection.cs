using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelection : MonoBehaviour
{
    public BattleMenu battleMenu;
    private int previousIndex = 0;
    private SpriteRenderer sr;

    private void Start()
    {
        transform.position = getPosition(battleMenu.isTargetAlly, battleMenu.currentTarget);
        transform.position += new Vector3(0, 0, 1);
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = (int)transform.position.z * 1;
    }

    private void LateUpdate()
    {
        if (previousIndex != battleMenu.currentTarget)
        {
            transform.position = getPosition(battleMenu.isTargetAlly, battleMenu.currentTarget);
            transform.position += new Vector3(0, 0, 1);
        }
    }

    // Can Add Vector2 Array To Act As Pivot Point, But No Need For Now.
    private Vector2 getPosition(bool isAlly, int index)
    {
        previousIndex = index;
        if (isAlly)
        {
            return battleMenu.database.allyDetails[index].GetComponent<Character>().sceneCharacter.transform.position;
        }
        else
        {
            return battleMenu.database.enemyDetails[index].GetComponent<Character>().sceneCharacter.transform.position;
        }
    }
}
