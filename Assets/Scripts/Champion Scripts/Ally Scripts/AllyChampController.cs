using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

public class AllyChampController : ChampionController
{
    private Tile previousPointedTile;
    private Tile currentPlacedTile;

    //flags
    public bool selected;

    void Start()
    {
        InitController();

        //controller variables
        previousPointedTile = GetCurrentTile();
        currentPlacedTile = null;

        //champ flags
        selected = false;
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

                if (selected)
                    MakeCurrentTilePointedAndPreviousUnpointed();

                break;
        }
    }
    
    private void OnMouseDown()
    {
        // if we click on a champion while they are fighting -> nothing happens, return
        if (TacticsMove.singleton.gameState == GameState.FightingRound)
            return;

        if (selected) // when champ is selected/being dragged and we click it
        {
            selected = false;
            PlaceChampion();
            TacticsMove.singleton.ClearSelectedTiles();
        }
        
        else if (!selected) // when there is no champ selected and we click over a champ
        {
            selected = true;
            currentPlacedTile = GetCurrentTile();
            TacticsMove.singleton.MakeHomeTilesSelectable();
        }
    }
    private void OnMouseEnter()
    {
        if (GetCurrentTile() == null)
            return;

        if (!GetCurrentTile().isHomeBattlefieldTile)
            return;

        GetCurrentTile().isPointed = true;
    }
    private void OnMouseExit()
    {
        if (GetCurrentTile() == null)
            return;

        GetCurrentTile().isPointed = false;
    }

    public void MakeCurrentTilePointedAndPreviousUnpointed()
    {
        if (GetCurrentTile() == null)
            return;

        if (previousPointedTile != GetCurrentTile())
        {
            previousPointedTile.isPointed = false;
            GetCurrentTile().isPointed = true;
            previousPointedTile = GetCurrentTile();
        }
        else
        {
            GetCurrentTile().isPointed = true;
        }
    }

    private void PlaceChampion()
    {
        if (GetCurrentTile() != null && GetCurrentTile().isHomeBattlefieldTile)
        {
            gameObject.transform.position = new Vector3(GetCurrentTile().transform.position.x, gameObject.transform.position.y, GetCurrentTile().transform.position.z);

            if (currentPlacedTile.isBench)
            {
                // to update the synergies for new champ on the battlefield
                TacticsMoveEvents.championEnteredBattlefield.Invoke(this.GetComponent<Champion>());
            }
        }
        else if (GetCurrentTile() != null && GetCurrentTile().isBench)
        {
            gameObject.transform.position = new Vector3(GetCurrentTile().transform.position.x, gameObject.transform.position.y, GetCurrentTile().transform.position.z);

            if (currentPlacedTile.isHomeBattlefieldTile)
            {
                // to update the synergies for champ left the battlefield
                TacticsMoveEvents.championLeftBattlefield.Invoke(this.GetComponent<Champion>());
            }
        }
        else
            gameObject.transform.position = new Vector3(currentPlacedTile.transform.position.x, gameObject.transform.position.y, currentPlacedTile.transform.position.z);

    }
}       