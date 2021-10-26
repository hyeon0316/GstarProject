using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAttack : AttackState
{
    float dmg;
    float startingDmg = 0f;
    void Awake()
    {
        dmg = Player.inst.power + startingDmg;
    }

    private void OnTriggerEnter(Collider other)
    {
        TagCheck(other, dmg);
    }
}
