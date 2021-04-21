﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBuff6 : SynergyBuff
{
    private void Start() 
    {
        Init();

        attackDamage = 60;

        GiveAttackDamage();    
    }
    private void OnDestroy()
    {
        DecreaseAttackDamage();
    }

    private void GiveAttackDamage()
    {
        receiver.AttackDamage += attackDamage;
    }
    private void DecreaseAttackDamage()
    {
        receiver.AttackDamage -= attackDamage;
    }
}