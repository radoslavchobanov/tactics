﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public enum ChampionState { Attacking, Moving, Idle, Dragged};
public enum ChampionAttackType { Melee, Range };

public enum Race { Human, Orc, Elf };
public enum Class { Warrior, Archer, Mage };

public enum Buff { Human3, Human6, Orc3, Orc6 };

public class Champion : MonoBehaviour
{
    //attacking vars
    public int attackDamage;
    public float attackSpeed; // attacks per second
    public float attackRange;
    public ChampionAttackType attackType;

    //defensive vars
    public int health;
    public int armor;

    //utility vars
    public float movementSpeed;
    public List<Buff> buffs = new List<Buff>();

    //spec vars
    public Class _class;
    public Race _race;

    //flags
    public bool haveBuffForUpdate = false;
}