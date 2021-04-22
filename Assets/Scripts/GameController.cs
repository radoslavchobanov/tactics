using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{   
    public static System.DateTime startRoundTime;

    public Button btn;

    private void Start()
    {
        btn.onClick.AddListener(changeRoundState);
    }
    void changeRoundState() // changing the state of the round
    {
        if (TacticsMove.singleton.gameState == GameState.BuyingRound)
        {
            TacticsMoveEvents.BuyRoundEnd.Invoke();

            TacticsMove.singleton.gameState = GameState.FightingRound; // fight round begin
            startRoundTime = System.DateTime.Now;

            TacticsMoveEvents.FightRoundStart.Invoke();

            AtBeginningOfFightingRoundActions();
        }

        else if (TacticsMove.singleton.gameState == GameState.FightingRound)
        {
            TacticsMoveEvents.FightRoundEnd.Invoke();

            TacticsMove.singleton.gameState = GameState.BuyingRound; // buy round begin

            TacticsMoveEvents.BuyRoundStart.Invoke();

            AtBeginningOfBuyingRoundActions();
        }
    }

    private void CurrentRound() // increasing the round counter and printing the current round
    {
        TacticsMove.singleton.numberOfRound += 1;
        print("round - " + TacticsMove.singleton.numberOfRound);
    }

    private void AtBeginningOfFightingRoundActions() // do that at the beginning of every fighting round
    {
        TacticsMove.singleton.SaveChampionsStartRoundPositions();
        TacticsMove.singleton.SaveEnemiesStartRoundPositions();
        TacticsMove.singleton.ChangeChampionsState(ChampionState.Moving);
        TacticsMove.singleton.ChangeEnemiesState(ChampionState.Moving);
    } 
    private void AtBeginningOfBuyingRoundActions() // do that at the beginning of every buying round
    {
        CurrentRound(); // at the beginning of each buying round, add 1 to rounds and print curr round.
        TacticsMove.singleton.ResetChampionsForBuyRound();
        TacticsMove.singleton.ResetEnemies();
        TacticsMove.singleton.ChampionsStartRoundPositions.Clear();
        TacticsMove.singleton.enemiesStartRoundPositions.Clear();
    } 
}