using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lelolas : Champion
{
    private ChampionController controller;
    public void InitChampion()
    {
        attackDamage = 150;
        attackSpeed = 1f;
        attackRange = 4.6f;
        attackType = ChampionAttackType.Range;
        health = 860;
        armor = 25;
        movementSpeed = 1.5f;

        _class = Class.Archer;
        _race = Race.Elf;
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
