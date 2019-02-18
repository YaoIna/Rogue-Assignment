using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isInInventory = false;
    public bool playerIsFacingRight = true;

    private WeaponComponents[] weaponComponents;
    private bool weaponUsed = false;

    public void AcquireWeapon() {
        weaponComponents = GetComponentsInChildren<WeaponComponents>();
    }

    public void UseWeapon() {
        EnableSpriteRender(true);
        weaponUsed = true;
    }
    
    public void EnableSpriteRender(bool isEnabled) {
        foreach(WeaponComponents weaponComp in weaponComponents)
        {
            weaponComp.GetSpriteRenderer().enabled = isEnabled;
        }
    }

    public Sprite GetComponentImage(int index) {
        return weaponComponents[index].GetSpriteRenderer().sprite;
    }

    private void Update()
    {
        if(weaponUsed && isInInventory)
        {
            float degreeY = 0, degreeZ = -90f, degreeZMax = 275f;
            Vector3 returnVector = Vector3.zero;
            if (!playerIsFacingRight)
            {
                degreeY = 180;
                returnVector = new Vector3(0, 180, 0);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, degreeY, degreeZ), Time.deltaTime * 20f);
            if (transform.eulerAngles.z <= degreeZMax)
            {
                        transform.eulerAngles = returnVector;
                        weaponUsed = false;
                        EnableSpriteRender(false);
                      }
        }
    }
}
