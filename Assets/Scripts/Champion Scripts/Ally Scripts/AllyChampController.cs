using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AllyChampController : ChampionController
{
    private Tile previousPointedTile;
    private Tile currentPlacedTile;

    //flags
    public bool selected;

    void Start()
    {
        //controller variables
        previousPointedTile = GetCurrentTile();
        currentPlacedTile = GetCurrentTile();

        //champ flags
        selected = false;

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

                if (selected)
                    MakeCurrentTilePointedAndPreviousUnpointed();

                break;
        }
    }
    
    private void OnMouseDown()
    {
        if (TacticsMove.gameState == GameState.BuyingRound)
        {
            if (selected)
            {
                selected = false;

                if (GetCurrentTile() != null && GetCurrentTile().homeTile)
                    gameObject.transform.position = new Vector3(GetCurrentTile().transform.position.x, gameObject.transform.position.y, GetCurrentTile().transform.position.z);
                else
                    gameObject.transform.position = new Vector3(currentPlacedTile.transform.position.x, gameObject.transform.position.y, currentPlacedTile.transform.position.z);

                TacticsMove.ClearSelectedTiles();
            }
            else if (!selected)
            {
                selected = true;
                TacticsMove.MakeHomeTilesSelectable();
                currentPlacedTile = GetCurrentTile();
            }
        }
    }
    private void OnMouseEnter()
    {
        if (GetCurrentTile() == null)
            return;

        if (!GetCurrentTile().homeTile)
            return;

        GetCurrentTile().pointed = true;
    }
    private void OnMouseExit()
    {
        if (GetCurrentTile() == null)
            return;

        GetCurrentTile().pointed = false;
    }

    public void MakeCurrentTilePointedAndPreviousUnpointed()
    {
        if (GetCurrentTile() == null)
            return;

        if (previousPointedTile != GetCurrentTile())
        {
            previousPointedTile.pointed = false;
            GetCurrentTile().pointed = true;
            previousPointedTile = GetCurrentTile();
        }
        else
        {
            GetCurrentTile().pointed = true;
        }
    }
}       