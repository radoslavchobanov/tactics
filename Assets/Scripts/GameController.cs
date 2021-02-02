using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Button btn;

    private void Start()
    {
        btn.onClick.AddListener(changeRoundState);
    }
    void changeRoundState()
    {
        if (TacticsMove.singleton.gameState == GameState.BuyingRound)
        {
            TacticsMove.singleton.gameState = GameState.FightingRound; // fight round begin

            AtBeginningOfFightingRoundActions();
        }

        else if (TacticsMove.singleton.gameState == GameState.FightingRound)
        {
            TacticsMove.singleton.gameState = GameState.BuyingRound; // buy round begin

            AtBeginningOfBuyingRoundActions();
        }
    } // changing the state of the round

    private void CurrentRound()
    {
        TacticsMove.singleton.numberOfRound += 1;
        print("round - " + TacticsMove.singleton.numberOfRound);
    } // increasing the round counter and printing the current round

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
        TacticsMove.singleton.championsStartRoundPositions.Clear();
        TacticsMove.singleton.enemiesStartRoundPositions.Clear();
    } 
}