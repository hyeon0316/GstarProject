using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject //굳이 게임오브젝트에 붙일 필요가 없어짐(따로관리가능)
{
    public string itemName; //아이템의 이름
    public Sprite itemImage; //아이템의 이미지
    public GameObject itemPrefab; //아이템의 프리팹

    public string EquipType; //무기 유형

    public enum ItemType
    {
        Equipment, //장비
        Used, //소모품
        Ingredient, //재료
    }

    public float itemDp;
    public float itemPower;
    public ItemType itemType; 


}
