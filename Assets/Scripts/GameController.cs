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
        if (TacticsMove.gameState == GameState.BuyingRound)
        {
            TacticsMove.gameState = GameState.FightingRound; // fight round begin

            AtBeginningOfFightingRoundActions();
        }

        else if (TacticsMove.gameState == GameState.FightingRound)
        {
            TacticsMove.gameState = GameState.BuyingRound; // buy round begin

            AtBeginningOfBuyingRoundActions();
        }
    } // changing the state of the round

    private void CurrentRound()
    {
        TacticsMove.numberOfRound += 1;
        print("round - " + TacticsMove.numberOfRound);
    } // increasing the round counter and printing the current round

    private void AtBeginningOfFightingRoundActions()
    {
        CurrentRound(); // at the beginning of each fighting round, add 1 to rounds and print curr round.
        TacticsMove.SaveChampionsStartRoundPositions();
        TacticsMove.SaveEnemiesStartRoundPositions();
        TacticsMove.ChangeChampionsState(ChampionState.Moving);
        TacticsMove.ChangeEnemiesState(ChampionState.Moving);
    } // do that at the beginning of every fighting round
    private void AtBeginningOfBuyingRoundActions()
    {
        TacticsMove.ResetChampionsForBuyRound();
        TacticsMove.ResetEnemies();
        TacticsMove.championsStartRoundPositions.Clear();
    } // do that at the beginning of every buying round
}