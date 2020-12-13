using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EnemyChampController : ChampionController
{
    public void InitEnemyBoss()
    {
        attackDamage = 600;
        attackSpeed = 0.2f;
        attackRange = 2f;
        attackType = ChampionAttackType.Melee;
        healthBar.maxValue = 10000;
        healthBar.value = healthBar.maxValue;
        health = (int)healthBar.maxValue;
        armor = 0;
        movementSpeed = 1.5f;

        //no class
        //no race
    }

    void Start()
    {
        InitEnemyBoss();
        InitController();
    }
    
    void Update()
    {
        switch (TacticsMove.singleton.gameState)
        {
            case GameState.FightingRound:
                if (championState == ChampionState.Moving)
                    MoveChampion();
                else if (championState == ChampionState.Attacking)
                    FightTarget();

                break;

            case GameState.BuyingRound:
                championState = ChampionState.Idle;
                break;
        }
    }
}