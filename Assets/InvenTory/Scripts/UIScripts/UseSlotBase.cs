using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSlotBase : MonoBehaviour
{
    [SerializeField]
    private GameObject go_UseSlotParent; //슬롯의 부모객체

    private Use_Slot[] uSlots;
    // Start is called before the first frame update
    void Start()
    {
        uSlots = go_UseSlotParent.GetComponentsInChildren<Use_Slot>();
    }

    //반복문 돌려서 슬롯마다 번호 커맨드 부여
}
