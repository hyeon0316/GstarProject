using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapcon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NpcManager.inst.NpcCheck();
        NpcManager.inst.MapCheck();
        NpcManager.inst.PotalCheck();
    }
}
