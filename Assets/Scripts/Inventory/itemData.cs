using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item", menuName = "data/itemData")]
public class itemData : ScriptableObject
{
    public item _item;
    public int stackCount = 1;
}
