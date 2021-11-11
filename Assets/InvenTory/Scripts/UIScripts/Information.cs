using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Information : MonoBehaviour
{
    public static bool informationActivated = false;
    public static bool slotClear = false;
    [SerializeField]
    private GameObject go_InformationBase;
    [SerializeField]
    private GameObject go_EslotParent; //슬롯의 부모객체

    public E_Slot[] e_Slots;
    private Inventory theInventory;

    
    // Start is called before the first frame update
    void Start()
    {
        theInventory = FindObjectOfType<Inventory>();
        e_Slots = go_EslotParent.GetComponentsInChildren<E_Slot>();
        
    }

    // Update is called once per frame
    void Update()
    {
        TryInformation();
        if (!InputNumber.activated)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                informationActivated = false;
            }
        }
    }

    private void TryInformation()
    {
        if (!SystemBase.gamePaused)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                informationActivated = !informationActivated;
            }
        }
        if (informationActivated)
            OpenInformation();
        else
            CloseInformation();
    }

    private void OpenInformation()
    {
        go_InformationBase.SetActive(true);
    }

    private void CloseInformation()
    {
        go_InformationBase.SetActive(false);
    }

    public void EquipItem(Item _item)
    {
        SoundManager.inst.SFXPlay("EquipItem", SoundManager.inst.uiList[0]);
        for (int i = 0; i < e_Slots.Length; i++)
        {
            if (e_Slots[i].CompareTag(_item.EquipType))
            {
                Item _tempItem = e_Slots[i].e_item;
                e_Slots[i].AddEquipItem(_item);

                if (_tempItem != null)
                {
                    theInventory.AcquireItem(_tempItem);
                    Player.inst.TakeOffEffect(_tempItem);
                }
                else
                    slotClear = true;

                return;
            }        
        }
    }

    public void Exit()
    {
        informationActivated = false;
    }
}
