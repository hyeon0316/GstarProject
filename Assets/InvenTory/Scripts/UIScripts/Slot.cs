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

    private Rect invenBaseRect; //Inventory_Base 이미지의 Rect 정보 

    private Rect inforBaseRect; //Information_Base 이미지의 Rect 정보

    private InputNumber theInputNumber;

    public static bool EquipClearOn = false;

    public static bool interChange_Equip = false;

    public GameObject inforPage;

    private void Start()
    {
        theInputNumber = FindObjectOfType<InputNumber>();
        invenBaseRect = transform.parent.parent.GetComponent<RectTransform>().rect;   
        inforBaseRect = inforPage.GetComponent<RectTransform>().rect;
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
        
        
        if (item.itemType != Item.ItemType.Equipment) //장비의 경우 개수표현X(단일 아이템)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
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

    private void ClearSlot()//슬롯 초기화
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
            if (item != null)
            {
                if (item.itemType == Item.ItemType.Equipment)
                {
                    //장착
                    information.EquipItem(item);
                    ClearSlot();//장착 후 장비창에서 사라지게 함(장비창->정보창 이동)           
                }
                else
                {
                    //소모
                    SetSlotCount(-1);
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
                if (item != null)
                {
                    if (item.itemType == Item.ItemType.Equipment)
                    {
                        //장착
                        information.EquipItem(item);
                        ClearSlot();//장착 후 장비창에서 사라지게 함(장비창->정보창 이동)           
                    }
                    else
                    {
                        //소모
                        SetSlotCount(-1);
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
        interChange_Equip = true;

        if (E_Slot.invenSlotClearOn)
        {
            ClearSlot();
            E_Slot.invenSlotClearOn = false;
        }

        //정보창 Off
        if (!inforPage.activeSelf)
        {
            if (DragSlot.instance.transform.localPosition.x < invenBaseRect.xMin //인벤 창을 벗어나 드롭시 아이템 드롭
               || DragSlot.instance.transform.localPosition.x > invenBaseRect.xMax
               || DragSlot.instance.transform.localPosition.y < invenBaseRect.yMin
               || DragSlot.instance.transform.localPosition.y > invenBaseRect.yMax)
            {
                if (DragSlot.instance.dragSlot != null)
                    theInputNumber.Call();
            }
            else
            {
                DragSlot.instance.SetColor(0);
                DragSlot.instance.dragSlot = null;
            }
        }
        //정보창On(보류, DragSlot위치 기준으로 다 해야하기 때문에 어렵)
        else if (inforPage.activeSelf)
        {
            if (!((DragSlot.instance.transform.localPosition.x > invenBaseRect.xMin //정보창 or 인벤창 밖에서 드롭 시
               && DragSlot.instance.transform.localPosition.x < invenBaseRect.xMax
               && DragSlot.instance.transform.localPosition.y > invenBaseRect.yMin
               && DragSlot.instance.transform.localPosition.y < invenBaseRect.yMax) ||
               (DragSlot.instance.transform.position.y > 150
                && DragSlot.instance.transform.position.y < 950
                && DragSlot.instance.transform.position.x > 80
                && DragSlot.instance.transform.position.x < 590)))
            {
                if (DragSlot.instance.dragSlot != null)
                    theInputNumber.Call();
            }
            else if ((DragSlot.instance.transform.localPosition.x > invenBaseRect.xMin//정보창 or 인벤창 안에서 드롭 시
                && DragSlot.instance.transform.localPosition.x < invenBaseRect.xMax
                && DragSlot.instance.transform.localPosition.y > invenBaseRect.yMin
                && DragSlot.instance.transform.localPosition.y < invenBaseRect.yMax) ||
                (DragSlot.instance.transform.position.y > 150
                 && DragSlot.instance.transform.position.y < 950
                 && DragSlot.instance.transform.position.x > 80
                 && DragSlot.instance.transform.position.x < 590))
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

        if(E_Slot.interChange)
        {
            if (DragSlot_Equip.instance.dragSlot_Equip != null)
            {
                Inter_ChangeSlot();
                E_Slot.interChange = false;
            }
        }
      
    }

    private void ChangeSlot()//a와 b의 자리를 바꿀 때,
    {
        Item _tempItem = item; //드래그가 끝날때 복사될 b(드래그가 끝나는 시점에서 해당 슬롯에 있었던 아이템정보를 복사함)
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);//b 자리에 a가 들감

        if (_tempItem != null)//a자리에 b가 들어갈 때   
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);//a와 b가 교환할 때       
        else     
            DragSlot.instance.dragSlot.ClearSlot();//빈자리로 이동할 때
    }

    private void Inter_ChangeSlot()
    {
        AddItem(DragSlot_Equip.instance.dragSlot_Equip.e_item);
        EquipClearOn = true;
    }
}
