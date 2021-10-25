using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UseSlotBase : MonoBehaviour
{

    [SerializeField]
    private GameObject go_UseSlotParent; //슬롯의 부모객체

    public Use_Slot[] uSlots;
    // Start is called before the first frame update
    void Start()
    {
        uSlots = go_UseSlotParent.GetComponentsInChildren<Use_Slot>();
    }
}
