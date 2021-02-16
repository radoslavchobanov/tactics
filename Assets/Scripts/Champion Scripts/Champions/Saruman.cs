using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saruman : Champion
{
    private ChampionController controller;
    public void InitChampion()
    {
        AttackDamage = 100;
        AttackSpeed = 0.9f;
        AttackRange = 6f;
        AttackType = ChampionAttackType.Range;
        Health = 800;
        Armor = 20;
        MovementSpeed = 1.5f;

        _Class = Class.Mage;
        _Race = Race.Orc;
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