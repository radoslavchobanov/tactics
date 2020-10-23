﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seraphim : Champion
{
    private ChampionController controller;
    public void InitChampion()
    {
        attackDamage = 200;
        attackSpeed = 0.6f;
        attackRange = 1.2f;
        attackType = ChampionAttackType.Melee;
        health = 1200;
        armor = 50;
        movementSpeed = 1.5f;

        _class = Class.Warrior;
        _race = Race.Human;
    }
    public void Start()
    {
        InitChampion();
        controller = gameObject.GetComponent<ChampionController>();

        controller.attackDamage = attackDamage;
        controller.attackSpeed = attackSpeed;
        controller.attackRange = attackRange;
        controller.attackType = attackType;
        controller.movementSpeed = movementSpeed;
        controller.armor = armor;
        controller._class = _class;
        controller._race = _race;

        controller.health = health;
        controller.healthBar.maxValue = health;
        controller.healthBar.value = controller.healthBar.maxValue;
    }
}