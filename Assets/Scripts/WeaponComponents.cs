using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class WeaponComponents : MonoBehaviour
{
    public Sprite[] componentModules;

    private SpriteRenderer spriteRenderer;
    private Weapon parent;

    private void Start()
    {
        parent = GetComponentInParent<Weapon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = componentModules[Random.Range(0, componentModules.Length)]; 
    }

    private void Update()
    {
        transform.eulerAngles = parent.transform.eulerAngles;
    }

    public SpriteRenderer GetSpriteRenderer() {
        return spriteRenderer;
    }

}
