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

        ConnectSynergyEvents();
    }

    private void ConnectSynergyEvents()
    {
        SynergyController.singleton.HumanCounter0.AddListener(HumanBuff0);
        SynergyController.singleton.HumanCounter3.AddListener(HumanBuff3);
        SynergyController.singleton.HumanCounter6.AddListener(HumanBuff6);
        SynergyController.singleton.HumanCounter9.AddListener(HumanBuff9);
        
        SynergyController.singleton.WarriorCounter0.AddListener(WarriorBuff0);
        SynergyController.singleton.WarriorCounter3.AddListener(WarriorBuff3);
        SynergyController.singleton.WarriorCounter6.AddListener(WarriorBuff6);
        SynergyController.singleton.WarriorCounter9.AddListener(WarriorBuff9);
    }
    private void HumanBuff0()
    {
        if (this.gameObject.GetComponent<HumanBuff3>() != null)
            Destroy(this.gameObject.GetComponent<HumanBuff3>());
    }
    private void HumanBuff3()
    {
        if (this.GetComponent<AllyChampController>().GetCurrentTile().isHomeBattlefieldTile && this.gameObject.GetComponent<HumanBuff3>() == null)
            this.gameObject.AddComponent<HumanBuff3>();
        else if (this.GetComponent<AllyChampController>().GetCurrentTile().isBench)
            Destroy(this.gameObject.GetComponent<HumanBuff3>());

        if (this.gameObject.GetComponent<HumanBuff6>() != null)
            Destroy(this.gameObject.GetComponent<HumanBuff6>());
    }
    private void HumanBuff6()
    {
        if (this.gameObject.GetComponent<HumanBuff3>() != null)
            Destroy(this.gameObject.GetComponent<HumanBuff3>());
        else if (this.gameObject.GetComponent<HumanBuff9>() != null)
            Destroy(this.gameObject.GetComponent<HumanBuff9>());

        if (this.GetComponent<AllyChampController>().GetCurrentTile().isHomeBattlefieldTile && this.gameObject.GetComponent<HumanBuff6>() == null)
            this.gameObject.AddComponent<HumanBuff6>();
        else if (this.GetComponent<AllyChampController>().GetCurrentTile().isBench)
            Destroy(this.gameObject.GetComponent<HumanBuff6>());
    }
    private void HumanBuff9()
    {
        if (this.GetComponent<AllyChampController>().GetCurrentTile().isHomeBattlefieldTile && this.gameObject.GetComponent<HumanBuff9>() == null)
            this.gameObject.AddComponent<HumanBuff9>();
        else if (this.GetComponent<AllyChampController>().GetCurrentTile().isBench)
            Destroy(this.gameObject.GetComponent<HumanBuff9>());

        if (this.gameObject.GetComponent<HumanBuff6>() != null)
            Destroy(this.gameObject.GetComponent<HumanBuff6>());
    }
    
    private void WarriorBuff0()
    {
        if (this.gameObject.GetComponent<WarriorBuff3>() != null)
            Destroy(this.gameObject.GetComponent<WarriorBuff3>());
    }
    private void WarriorBuff3()
    {
        if (this.GetComponent<AllyChampController>().GetCurrentTile().isHomeBattlefieldTile && this.gameObject.GetComponent<WarriorBuff3>() == null)
            this.gameObject.AddComponent<WarriorBuff3>();
        else if (this.GetComponent<AllyChampController>().GetCurrentTile().isBench)
            Destroy(this.gameObject.GetComponent<WarriorBuff3>());

        if (this.gameObject.GetComponent<WarriorBuff6>() != null)
            Destroy(this.gameObject.GetComponent<WarriorBuff6>());
    }
    private void WarriorBuff6()
    {
        if (this.gameObject.GetComponent<WarriorBuff3>() != null)
            Destroy(this.gameObject.GetComponent<WarriorBuff3>());
        else if (this.gameObject.GetComponent<WarriorBuff9>() != null)
            Destroy(this.gameObject.GetComponent<WarriorBuff9>());

        if (this.GetComponent<AllyChampController>().GetCurrentTile().isHomeBattlefieldTile && this.gameObject.GetComponent<WarriorBuff6>() == null)
            this.gameObject.AddComponent<WarriorBuff6>();
        else if (this.GetComponent<AllyChampController>().GetCurrentTile().isBench)
            Destroy(this.gameObject.GetComponent<WarriorBuff6>());
    }
    private void WarriorBuff9()
    {
        if (this.GetComponent<AllyChampController>().GetCurrentTile().isHomeBattlefieldTile && this.gameObject.GetComponent<WarriorBuff9>() == null)
            this.gameObject.AddComponent<WarriorBuff9>();
        else if (this.GetComponent<AllyChampController>().GetCurrentTile().isBench)
            Destroy(this.gameObject.GetComponent<WarriorBuff9>());

        if (this.gameObject.GetComponent<WarriorBuff6>() != null)
            Destroy(this.gameObject.GetComponent<WarriorBuff6>());
    }
}