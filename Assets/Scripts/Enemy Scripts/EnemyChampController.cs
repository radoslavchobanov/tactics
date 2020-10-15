using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EnemyChampController : ChampionController
{


    void Start()
    {
        InitController();
    }
    
    void Update()
    {
        switch (TacticsMove.gameState)
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