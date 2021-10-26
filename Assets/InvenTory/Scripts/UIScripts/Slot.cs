using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{  

    public Item item; //획득한 아이템
    public int itemCount; //획득한 아이템의 개수
    public Image itemImage; //아이템의 이미지

    private float doubleClickTime = 0.25f; //더블클릭 관련 변수들
    private bool isOneClick = false;
    private bool isDoubleClick = false;
    private double c_Timer = 0;

    //필요 컴포넌트
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage; //빈슬롯일땐 카운트배경이미지를 띄우지 않기 위함
    [SerializeField]
    private Information information;
    [SerializeField]
    private Player thePlayer;

    private Rect invenBaseRect; //Inventory_Base 이미지의 Rect 정보 

    private InputNumber theInputNumber;
    private E_Slot e_Slot;

    public static bool pMemory = false;
    public static int pNumber;

    public GameObject inforPage;


    private void Start()
    {
        e_Slot = FindObjectOfType<E_Slot>();
        theInputNumber = FindObjectOfType<InputNumber>();
        invenBaseRect = transform.parent.parent.GetComponent<RectTransform>().rect;          
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
        
        
        if (item.itemType != Item.ItemType.Equipment) 
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else    //장비의 경우 개수표현X(단일 아이템)
        {          
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) //우클릭하여 아이템 사용
        {
            pMemory = true;
            if (item != null)
            {
                if (item.itemType == Item.ItemType.Equipment)
                {
                    for(int i =0; i< transform.parent.parent.parent.GetComponent<Inventory>().slots.Length; i++) //슬롯 자리 검사
                    {
                        if (transform.parent.parent.parent.GetComponent<Inventory>().slots[i].item != null)
                        {
                            if (transform.parent.parent.parent.GetComponent<Inventory>().slots[i].item.Equals(item))
                            {
                                pNumber = i;
                                break;
                            }
                        }
                    }
                    //장착
                    information.EquipItem(item);                   
                    if (Information.slotClear)
                    {
                        ClearSlot();  
                        Information.slotClear = false;
                    }
                }
                else
                {
                    //소모
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
                        else if (item.itemName == "Potion_Mp")
                        {
                            if (thePlayer.startingMana > thePlayer.mana)
                            {
                                thePlayer.HealMp(100);
                                SetSlotCount(-1);
                            }
                        }
                    }
                }
            }
        }

        else if (eventData.button == PointerEventData.InputButton.Left) //더블클릭
        {
            if (!isOneClick)
            {
                c_Timer = Time.time;
                isOneClick = true;
            }
            else if(isOneClick && ((Time.time - c_Timer) > doubleClickTime))
            {
                isOneClick = false;
            }

            else if (isOneClick && ((Time.time - c_Timer) < doubleClickTime))
            {
                isOneClick = false;
                isDoubleClick = true;
            }

            if(isDoubleClick)
            {
                pMemory = true;
                if (item != null)
                {
                    if (item.itemType == Item.ItemType.Equipment)
                    {
                        for (int i = 0; i < transform.parent.parent.parent.GetComponent<Inventory>().slots.Length; i++) //슬롯 자리 검사
                        {
                            if (transform.parent.parent.parent.GetComponent<Inventory>().slots[i].item != null)
                            {
                                if (transform.parent.parent.parent.GetComponent<Inventory>().slots[i].item.Equals(item))
                                {
                                    pNumber = i;
                                    break;
                                }
                            }
                        }
                        //장착
                        information.EquipItem(item);
                        if (Information.slotClear)
                        {
                            ClearSlot();
                            Information.slotClear = false;
                        }
                    }
                    else
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
                            else if (item.itemName == "Potion_Mp")
                            {
                                if (thePlayer.startingMana > thePlayer.mana)
                                {
                                    thePlayer.HealMp(100);
                                    SetSlotCount(-1);
                                }
                            }
                        }
                    }
                }
                isDoubleClick = false;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position; //마우스 포지션
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }



    public void OnEndDrag(PointerEventData eventData) //드래그가 끝날때 호출
    {   
        //정보창 Off
        if (!inforPage.activeSelf)
        {
            if (!((DragSlot.instance.transform.localPosition.x > invenBaseRect.xMin
                && DragSlot.instance.transform.localPosition.x < invenBaseRect.xMax
                && DragSlot.instance.transform.localPosition.y > invenBaseRect.yMin
                && DragSlot.instance.transform.localPosition.y < invenBaseRect.yMax) ||
                     (DragSlot.instance.transform.position.y > 12 //아이템 사용창 범위
                     && DragSlot.instance.transform.position.y < 54
                     && DragSlot.instance.transform.position.x > 600
                     && DragSlot.instance.transform.position.x < 710)))
            {
                Debug.Log(DragSlot.instance.transform.position);
                if (DragSlot.instance.dragSlot != null)
                    theInputNumber.Call();
            }
            else if ((DragSlot.instance.transform.localPosition.x > invenBaseRect.xMin 
               && DragSlot.instance.transform.localPosition.x < invenBaseRect.xMax
               && DragSlot.instance.transform.localPosition.y > invenBaseRect.yMin
               && DragSlot.instance.transform.localPosition.y < invenBaseRect.yMax) ||
                    (DragSlot.instance.transform.position.y > 12
                    &&DragSlot.instance.transform.position.y < 54
                    && DragSlot.instance.transform.position.x > 600
                    && DragSlot.instance.transform.position.x < 710))
            {
                DragSlot.instance.SetColor(0);
                DragSlot.instance.dragSlot = null;               
            }
        }

        else if (inforPage.activeSelf)
        {
            if (!((DragSlot.instance.transform.localPosition.x > invenBaseRect.xMin //정보창 or 인벤창 밖에서 드롭 시
               && DragSlot.instance.transform.localPosition.x < invenBaseRect.xMax
               && DragSlot.instance.transform.localPosition.y > invenBaseRect.yMin
               && DragSlot.instance.transform.localPosition.y < invenBaseRect.yMax) ||
               (DragSlot.instance.transform.position.y > 110
                && DragSlot.instance.transform.position.y < 380
                && DragSlot.instance.transform.position.x > 105
                && DragSlot.instance.transform.position.x < 300) ||
                  (DragSlot.instance.transform.position.y > 12
                    && DragSlot.instance.transform.position.y < 54
                    && DragSlot.instance.transform.position.x > 600
                    && DragSlot.instance.transform.position.x < 710)))
            {
                if (DragSlot.instance.dragSlot != null)
                    theInputNumber.Call();
            }

            else if ((DragSlot.instance.transform.localPosition.x > invenBaseRect.xMin//정보창 or 인벤창 안에서 드롭 시
                && DragSlot.instance.transform.localPosition.x < invenBaseRect.xMax
                && DragSlot.instance.transform.localPosition.y > invenBaseRect.yMin
                && DragSlot.instance.transform.localPosition.y < invenBaseRect.yMax) ||
                (DragSlot.instance.transform.position.y > 110
                 && DragSlot.instance.transform.position.y < 380
                 && DragSlot.instance.transform.position.x > 105
                 && DragSlot.instance.transform.position.x < 300) ||
                  (DragSlot.instance.transform.position.y > 12
                    && DragSlot.instance.transform.position.y < 54
                    && DragSlot.instance.transform.position.x > 600
                    && DragSlot.instance.transform.position.x < 710))
            {
                DragSlot.instance.SetColor(0);
                DragSlot.instance.dragSlot = null;
            }

        }
    }

    public void OnDrop(PointerEventData eventData) //다른 슬롯 위에서 드래그가 끝날때 호출
    {
        if (DragSlot.instance.dragSlot != null)//빈슬롯을 드래그해서 Null 참조를 발생하는 것을 방지    
            ChangeSlot();


        if (DragSlot_Equip.instance.dragSlot_Equip != null)
        {
            Inter_ChangeSlot();
        }

        if (DragSlot_Used.instance.dragSlot_Used != null)
        {
            Inter_Change_uSlot();
        }
    }

    private void ChangeSlot()//a와 b의 자리를 바꿀 때,
    {
        Item _tempItem = item; //드래그가 끝날때 복사될 b(드래그가 끝나는 시점에서 해당 슬롯에 있었던 아이템정보를 복사함)
        int _tempItemCount = itemCount;

        if (item == null)
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);//b 자리에 a가 들감
        else if (DragSlot.instance.dragSlot.item.itemName == item.itemName)//인벤창에서 같은 아이템을 드래그로 합칠 때
        {
            if (DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Used)
                SetSlotCount(DragSlot.instance.dragSlot.itemCount);
        }

        if (_tempItem != null && DragSlot.instance.dragSlot.item.itemName != item.itemName)//a자리에 b가 들어갈 때   
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);//a와 b가 교환할 때       
        else       
            DragSlot.instance.dragSlot.ClearSlot();//빈자리로 이동할 때
        
    }

    private void Inter_ChangeSlot() //정보창에서 인벤창으로 드래그
    {
        Item _tempItem = item;

        AddItem(DragSlot_Equip.instance.dragSlot_Equip.e_item);

        if (_tempItem != null)
            DragSlot_Equip.instance.dragSlot_Equip.AddEquipItem(_tempItem);
        else
            DragSlot_Equip.instance.dragSlot_Equip.ClearSlot();       
    }

    private void Inter_Change_uSlot()//아이템 사용창에서 인벤창으로 드래그
    {
        Item _tempItem = item;
        if (item == null)
            AddItem(DragSlot_Used.instance.dragSlot_Used.item);
        else if (DragSlot_Used.instance.dragSlot_Used.item.itemName == item.itemName)
            SetSlotCount(DragSlot_Used.instance.dragSlot_Used.itemCount);

        if (_tempItem != null && DragSlot_Used.instance.dragSlot_Used.item.itemName != item.itemName)
            DragSlot_Used.instance.dragSlot_Used.AddItem(_tempItem);
        else
            DragSlot_Used.instance.dragSlot_Used.ClearSlot();
    }
}
