﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Use_Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;
    public int itemCount; //획득한 아이템의 개수
    public Image itemImage; //아이템의 이미지

    private float doubleClickTime = 0.25f; //더블클릭 관련 변수들
    private bool isOneClick = false;
    private bool isDoubleClick = false;
    private double c_Timer = 0;

    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;
    [SerializeField]
    private Player thePlayer;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        CommendNumber();
    }
    private void CommendNumber()
    {
        for (int i = 0; i < transform.parent.GetComponent<UseSlotBase>().uSlots.Length; i++) //슬롯 자리 검사
        {
            if (transform.parent.GetComponent<UseSlotBase>().uSlots[i].item != null)
            {
                switch(i)
                {
                    case 0:
                        if (Input.GetKeyDown(KeyCode.Alpha1))
                        {
                            if (item.itemType == Item.ItemType.Used)
                            {
                                if (item.itemName == "Potion_Hp")
                                {
                                    if (thePlayer.startingHealth > thePlayer.health)
                                    {
                                        thePlayer.HealHp(100);
                                        SetSlotCount(-1);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
    private void SetColor(float _alpha) //이미지 투명도 조절
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    public void AddItem(Item _item, int _count = 1)//아이템 획득
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        go_CountImage.SetActive(true);
        text_Count.text = itemCount.ToString();
        SetColor(1);
    }

    public void SetSlotCount(int _conut) //아이템 개수 조정
    {
        itemCount += _conut;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    public void ClearSlot()//슬롯 초기화
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot_Used.instance.dragSlot_Used = this;
            DragSlot_Used.instance.DragSetImage(itemImage);
            DragSlot_Used.instance.transform.position = eventData.position; //마우스 포지션
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot_Used.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot_Used.instance.SetColor(0);
        DragSlot_Used.instance.dragSlot_Used = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot_Used.instance.dragSlot_Used != null)//빈슬롯을 드래그해서 Null 참조를 발생하는 것을 방지    
            ChangeSlot();

        if (DragSlot.instance.dragSlot != null)
        {
            Inter_ChangeSlot();
        }
    }

    private void ChangeSlot()//a와 b의 자리를 바꿀 때,
    {
        Item _tempItem = item; //드래그가 끝날때 복사될 b(드래그가 끝나는 시점에서 해당 슬롯에 있었던 아이템정보를 복사함)
        int _tempItemCount = itemCount;

        AddItem(DragSlot_Used.instance.dragSlot_Used.item, DragSlot_Used.instance.dragSlot_Used.itemCount);//b 자리에 a가 들감

        if (_tempItem != null)//a자리에 b가 들어갈 때   
            DragSlot_Used.instance.dragSlot_Used.AddItem(_tempItem, _tempItemCount);//a와 b가 교환할 때       
        else
            DragSlot_Used.instance.dragSlot_Used.ClearSlot();//빈자리로 이동할 때
    }

    private void Inter_ChangeSlot() //인벤창에서 아이템사용창으로 드래그
    {
        Item _tempItem = item;

        AddItem(DragSlot.instance.dragSlot.item);

        if (_tempItem != null)
            DragSlot.instance.dragSlot.AddItem(_tempItem);
        else
            DragSlot.instance.dragSlot.ClearSlot();
    }
}