using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChampionState { Attacking, Moving, Idle};

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

    public void InitChampion()
    {
        health = 100;
        movementSpeed = 1;

        InitializingChampionAttacksByType();
    }

    void InitializingChampionAttacksByType()
    {
        if (attackType == ChampionAttackType.Melee)
        {
            attackDamage = 40;
            attackSpeed = 0.6f;
            attackRange = 1.2f;
        }
        else if (attackType == ChampionAttackType.Range)
        {
            attackDamage = 5;
            attackSpeed = 1f;
            attackRange = 4f;
        }
    }

    public Tile GetCurrentTile()
    {
        return TacticsMove.GetChampionTile(gameObject);
    }
}