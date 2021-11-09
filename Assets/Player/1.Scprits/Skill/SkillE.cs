﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillE : AttackState
{

    float dmg;
    float startingDmg = 40f;
    void Awake()
    {
        dmg = Player.inst.power + startingDmg;
        SetDmg(dmg);
    }

    private void OnTriggerEnter(Collider other)
    {
        TagCheck(other, dmg);
    }
}
