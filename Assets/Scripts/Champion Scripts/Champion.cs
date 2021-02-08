using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ChampionState { Attacking, Moving, Idle, Dragged};
public enum ChampionAttackType { Melee, Range };

public enum Race { Human, Orc, Elf };
public enum Class { Warrior, Archer, Mage };

public class Champion : MonoBehaviour
{
    //attacking vars
    private int attackDamage;
    private float attackSpeed; // attacks per second
    private float attackRange;
    private ChampionAttackType attackType;
    
    //defensive vars
    private int health;
    private int armor;
    private int magicResist;

    //utility vars
    private float movementSpeed;
    public List<Buff> buffs = new List<Buff>(); // ?

    //spec vars
    private Class _class;
    private Race _race;

    //flags
    public bool haveBuffForUpdate = false;

    // getters and setters
    public int AttackDamage { get => this.attackDamage; set {this.attackDamage = value;}}
    public float AttackSpeed { get => this.attackSpeed; set {this.attackSpeed = value;}}
    public float AttackRange { get => this.attackRange; set {this.attackRange = value;}}
    public ChampionAttackType AttackType { get => this.attackType; set {this.attackType = value;}}
    
    public int Health { get => this.health; set {this.health = value;}}
    public int Armor { get => this.armor; set {this.armor = value;}}
    public int MagicResist { get => this.magicResist; set {this.magicResist = value;}}
    
    public float MovementSpeed { get => this.movementSpeed; set {this.movementSpeed = value;}}
    
    public Class _Class { get => this._class; set {this._class = value;}}
    public Race _Race { get => this._race; set {this._race = value;}}
}