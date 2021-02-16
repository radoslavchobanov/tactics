using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legolas : Champion
{
    private ChampionController controller;
    public void InitChampion()
    {
        AttackDamage = 150;
        AttackSpeed = 1f;
        AttackRange = 6.6f;
        AttackType = ChampionAttackType.Range;
        Health = 860;
        Armor = 25;
        MovementSpeed = 1.5f;

        _Class = Class.Archer;
        _Race = Race.Elf;
    }
    public void Start()
    {
        InitChampion();
        controller = gameObject.GetComponent<ChampionController>();

        controller.AttackDamage = AttackDamage;
        controller.AttackSpeed = AttackSpeed;
        controller.AttackRange = AttackRange;
        controller.AttackType = AttackType;
        controller.MovementSpeed = MovementSpeed;
        controller.Armor = Armor;
        controller._Class = _Class;
        controller._Race = _Race;

        controller.Health = Health;
        controller.healthBar.maxValue = Health;
        controller.healthBar.value = controller.healthBar.maxValue;
    }
}