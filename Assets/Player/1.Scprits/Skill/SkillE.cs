using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillE : AttackState
{

    float dmg;
    float startingDmg = 5f;
    void Awake()
    {
        dmg = Player.inst.power + startingDmg;
    }

    private void OnTriggerEnter(Collider other)
    {
        TagCheck(other, dmg);
    }
}
