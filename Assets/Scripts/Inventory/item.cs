using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item", menuName = "data/item")]

public class item : ScriptableObject
{
    [Header ("General")]
    public bool isUsable = false;
    public bool isEquipable = false;
    public bool destroyOnUse = false;
    public bool isKeyItem = false;
    public bool isHealingItem = false;
    public bool isAmmo = false;
    public bool isWeapon = false;
    public string Description = "";
    public string exampineDescription = "";
    public Sprite icon;
    public Sprite examineSprite;
    public bool exitMenuOnUse = false;
    public bool linkToEvent = false;
    public int eventID = 0;
    public bool stackable = false;
    public int maxStack = 1;

    [Header("Specific")]
    public int healPercentage = 0;
    public item weaponToReload;
    public item giveItemOnExamine;
    public bool destroyOnExamine;
    public bool eventOnExamine = false;
    public int eventId = 0;
}
