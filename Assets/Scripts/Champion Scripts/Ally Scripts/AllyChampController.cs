using UnityEngine;
using UnityEngine.UI;

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
                else if (championState == ChampionState.OnWaiting)
                {
                    if (IsTargetInRange())
                        championState = ChampionState.Attacking;
                }

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
    private void OnMouseOver() 
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (this.gameObject.transform.Find("Canvas").Find("Text").GetComponent<Text>().enabled == true)
            {
                this.gameObject.transform.Find("Canvas").Find("Text").GetComponent<Text>().enabled = false;
                return;
            }

                this.gameObject.transform.Find("Canvas").Find("Text").GetComponent<Text>().text = MakeStringWithStats();
                this.gameObject.transform.Find("Canvas").Find("Text").GetComponent<Text>().enabled = true;
        }    
    }

    private string MakeStringWithStats()
    {
        string text = "AD " + this.gameObject.GetComponent<Champion>().AttackDamage.ToString() + "\n";
                text += "Armor " + this.gameObject.GetComponent<Champion>().Armor.ToString();

        return text;
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
        // if the champ is placed TO HOME TILE, to make his position the tile position
        if (GetCurrentTile() != null && GetCurrentTile().isHomeBattlefieldTile)
        {
            gameObject.transform.position = new Vector3(GetCurrentTile().transform.position.x, gameObject.transform.position.y, GetCurrentTile().transform.position.z);
            
            // if the champ is placed FROM BENCH TILE, to call the event for champ entered battlefield
            if (currentPlacedTile.isBench)
                TacticsMoveEvents.ChampionEnteredBattlefield.Invoke(this.GetComponent<Champion>());
        }
        // if the champ is placed TO BENCH TILE, to make his position the tile position
        else if (GetCurrentTile() != null && GetCurrentTile().isBench)
        {
            gameObject.transform.position = new Vector3(GetCurrentTile().transform.position.x, gameObject.transform.position.y, GetCurrentTile().transform.position.z);

            // if the champ is placed FROM HOME TILE, to call the event for champ left battlefield
            if (currentPlacedTile.isHomeBattlefieldTile)
                TacticsMoveEvents.ChampionLeftBattlefield.Invoke(this.GetComponent<Champion>());
        }
        else
            gameObject.transform.position = new Vector3(currentPlacedTile.transform.position.x, gameObject.transform.position.y, currentPlacedTile.transform.position.z);
    }
}