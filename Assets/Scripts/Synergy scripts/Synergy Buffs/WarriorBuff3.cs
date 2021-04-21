﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBuff3 : SynergyBuff
{
    private void Start() 
    {
        Init();

        armor = 15;

        GiveArmor();
    }
    private void OnDestroy()
    {
        DecreaseArmor();
    }

    private void GiveArmor()
    {
        receiver.Armor += armor;
    }
    private void DecreaseArmor()
    {
        receiver.Armor -= armor;
    }
}