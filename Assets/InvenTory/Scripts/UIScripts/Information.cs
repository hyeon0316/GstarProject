using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Information : MonoBehaviour
{
    public static bool informationActivated = false;

    [SerializeField]
    private GameObject go_InformationBase;
    [SerializeField]
    private GameObject go_EslotParent; //슬롯의 부모객체

    private E_Slot[] e_Slots;

    // Start is called before the first frame update
    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.U))
        {
            informationActivated = !informationActivated;           
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
        for (int i = 0; i < e_Slots.Length; i++)
        {
            if (e_Slots[i].e_item == null) //템창이 빈칸일때
            {
                if (e_Slots[i].CompareTag(_item.EquipType))
                {
                    e_Slots[i].AddEquipItem(_item);
                    return;
                }
            }
        }
    }

    public void Exit()
    {
        informationActivated = false;
    }
}
