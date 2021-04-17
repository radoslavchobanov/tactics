using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EnemyChampController : ChampionController
{
    public void InitEnemyBoss()
    {
        AttackDamage = 600;
        AttackSpeed = 0.2f;
        AttackRange = 2f;
        AttackType = ChampionAttackType.Melee;
        healthBar.maxValue = 10000;
        healthBar.value = healthBar.maxValue;
        Health = (int)healthBar.maxValue;
        Armor = 0;
        MovementSpeed = 1.5f;

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
                else if (championState == ChampionState.OnWaiting)
                {
                    if (IsTargetInRange())
                        championState = ChampionState.Attacking;
                }

                break;

            case GameState.BuyingRound:
                championState = ChampionState.Idle;
                break;
        }
    }
}