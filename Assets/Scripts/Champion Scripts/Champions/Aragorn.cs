using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aragorn : Champion
{
    private ChampionController controller;
    public void InitChampion()
    {
        AttackDamage = 200;
        AttackSpeed = 0.6f;
        AttackRange = 2f;
        AttackType = ChampionAttackType.Melee;
        Health = 1200;
        Armor = 50;
        MovementSpeed = 1.5f;

        _Class = Class.Warrior;
        _Race = Race.Human;
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