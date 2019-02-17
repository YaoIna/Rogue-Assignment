using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ItemType
{
    Boot, Glove
}

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public int attackNum, defenseNum;
    public Color levelColor;
    public string itemName;

    public Sprite glove;
    public Sprite boot;

    private SpriteRenderer spriteRenderer;

    private void GenerateItem()
    {
        var typeCount = Enum.GetValues(typeof(ItemType)).Length;
        itemType = (ItemType)Random.Range(0, typeCount);

        switch (itemType)
        {
            case ItemType.Glove:
                itemName = "glove";
                defenseNum = 0;
                attackNum = Random.Range(1, 6);
                spriteRenderer.sprite = glove;
                break;
            case ItemType.Boot:
                itemName = "boot";
                attackNum = 0;
                defenseNum = Random.Range(1, 6);
                spriteRenderer.sprite = boot;
                break;
            default:
                break;
        }
        int itemLevel = Random.Range(1, 101);
        if (itemLevel > 0 && itemLevel < 51)
        {
            spriteRenderer.color = levelColor = Color.green;
            attackNum += Random.Range(1, 4);
            defenseNum += Random.Range(1, 4);
        }
        else if (itemLevel > 50 && itemLevel < 76)
        {
            spriteRenderer.color = levelColor = Color.blue;
            attackNum += Random.Range(4, 10);
            defenseNum += Random.Range(4, 10);
        }
        else if (itemLevel > 75 && itemLevel < 91)
        {
            spriteRenderer.color = levelColor = Color.yellow;
            attackNum += Random.Range(15, 25);
            defenseNum += Random.Range(15, 25);
        }
        else
        {
            spriteRenderer.color = levelColor = Color.magenta;
            attackNum += Random.Range(40, 55);
            defenseNum += Random.Range(40, 55);
        }
    }

    public void InitItem()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GenerateItem();
    }


}
