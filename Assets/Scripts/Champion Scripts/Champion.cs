using UnityEngine;

public enum ChampionState { Attacking, Moving, Idle, Dead, Dragged, OnWaiting};
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
    private int armor;
    private int magicResist;

    //utility vars
    private int health;
    private int mana;
    private float movementSpeed;

    //spec vars
    private Class _class;
    private Race _race;

    //flags

    // getters and setters
    public int AttackDamage { get => this.attackDamage; set {this.attackDamage = value;}}
    public float AttackSpeed { get => this.attackSpeed; set {this.attackSpeed = value;}}
    public float AttackRange { get => this.attackRange; set {this.attackRange = value;}}
    public ChampionAttackType AttackType { get => this.attackType; set {this.attackType = value;}}
    
    public int Armor { get => this.armor; set {this.armor = value;}}
    public int MagicResist { get => this.magicResist; set {this.magicResist = value;}}
    
    public int Health { get => this.health; set {this.health = value;}}
    public int Mana { get => this.mana; set {this.mana = value;}}
    public float MovementSpeed { get => this.movementSpeed; set {this.movementSpeed = value;}}
    
    public Class _Class { get => this._class; set {this._class = value;}}
    public Race _Race { get => this._race; set {this._race = value;}}
}