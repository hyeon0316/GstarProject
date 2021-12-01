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
    void Update()
    {
        if (InputNumber.activated == false)
            CommendNumber();
    }

    private void CommendNumber()
    {
        for (int i = 0; i < uSlots.Length; i++)
        {
            if (i == 0)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    uSlots[0].UseItem();
                    Debug.Log(1);
                }
            }
            else if (i == 1)
            {
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    uSlots[1].UseItem();
                    Debug.Log(2);
                }
            }
            else if (i == 2)
            {
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    uSlots[2].UseItem();
                    Debug.Log(3);
                }
            }
        }
    }
}
