﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false; //인벤창을 열땐 캐릭터의 기본공격 등 제한

    //필요 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotParent; //슬롯의 부모객체

    private Slot[] slots; //슬롯들


    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotParent.GetComponentsInChildren<Slot>(); //자식 객체들 제어
    }

    // Update is called once per frame
    void Update()
    {
        TryInventory();
        if (!InputNumber.activated)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                inventoryActivated = false;
            }
        }
    }

    private void TryInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;         
        }
        if (inventoryActivated)
            OpenInventory();
        else
            CloseInventory();
    }
    
    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1) //기본값 1개
    {
        if(Item.ItemType.Equipment != _item.itemType) //장비 빼고 개수 추가
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)//이미 아이템이 있을 시 개수만 증가
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }
        
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)//새로운 아이템일시 아이템 추가
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }

    public void Exit()
    {
        inventoryActivated = false;
    }
}
