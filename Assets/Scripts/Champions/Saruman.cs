using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saruman : Champion
{
    private ChampionController controller;
    public void InitChampion()
    {
        attackDamage = 100;
        attackSpeed = 0.9f;
        attackRange = 4f;
        attackType = ChampionAttackType.Range;
        health = 800;
        armor = 20;
        movementSpeed = 1.5f;

        _class = Class.Mage;
        _race = Race.Orc;
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
