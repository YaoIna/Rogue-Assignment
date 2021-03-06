﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Item item;
    public Weapon weapon;
    public Sprite openSprite;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open()
    {
        spriteRenderer.sprite = openSprite;

        GameObject toInstantiate;
        if (Random.Range(0, 2) == 1)
        {
            toInstantiate = weapon.gameObject;
        }
        else
        {
            item.InitItem();
            toInstantiate = item.gameObject;
        }


        GameObject instance = Instantiate(toInstantiate, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity, transform.parent) as GameObject;

        gameObject.layer = 10;
        spriteRenderer.sortingLayerName = "Items";
    }
}
