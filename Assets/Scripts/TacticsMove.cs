using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditorInternal;
using UnityEngine;

public enum GameState { BuyingRound, FightingRound };

public enum ChampionAttackType { Melee, Range };

public class TacticsMove : MonoBehaviour
{
//tiles variables
    public static GameObject[] map; // array with all the tiles

//players variables 
    public static GameObject[] champions;
    public static List<Vector3> championsStartRoundPositions;

//enemy variables
    public static GameObject[] enemies;
    public static List<Vector3> enemiesStartRoundPositions;

//game variables
    public static GameState gameState;
    public static int numberOfRound;

    void Start()
    {
        map = GameObject.FindGameObjectsWithTag("Tile");

        champions = GameObject.FindGameObjectsWithTag("Champion");
        
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        numberOfRound = 0;
        championsStartRoundPositions = new List<Vector3>();
        enemiesStartRoundPositions = new List<Vector3>();
    }
    
    private void Update()
    {
        if (ChampionsWin()) // pri pobeda na championite
        {
            OnWonRound();
        }

        if (EnemiesWin()) // pri pobeda na enemy-tata
        {
            OnLostRound();
        }
    }

    public static Tile GetChampionTile(GameObject champion)
    {
        RaycastHit hit; 
        Tile tile = null;

        if (Physics.Raycast(champion.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    } // getting the current tile of a champion
    public static void MakeHomeTilesSelectable()
    {
        foreach (GameObject tileObject in map)
        {
            Tile tile = tileObject.GetComponent<Tile>();

            if (tile.homeTile == true)
                tile.selectable = true;
        }
    }
    public static void ClearSelectedTiles()
    {
        foreach (GameObject tileObject in map)
        {
            Tile tile = tileObject.GetComponent<Tile>();

            if (tile.selectable)
                tile.selectable = false;
        }
    }
    public static void GetAdjacentTiles(Tile targetTile)
    {
        foreach (GameObject tile in map)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(targetTile);
        }
    }

    public static void SaveChampionsStartRoundPositions() // saving position of each champion in array
    {
        foreach (GameObject obj in champions)
        {
            championsStartRoundPositions.Add(obj.transform.position);
        }    
    }
    public static void ResetChampionsForBuyRound() // resetting the champions for buying round
    {
        for (int i=0; i<champions.Length; ++i)
        {
            GameObject currChamp = champions.ElementAt(i);
            if (currChamp == null)
                return;

            currChamp.transform.position = championsStartRoundPositions.ElementAt(i); // resetvame poziciq
            currChamp.GetComponent<AllyChampController>().isDead = false;
            currChamp.GetComponent<AllyChampController>().isTargetInRange = false;
            currChamp.GetComponent<AllyChampController>().moving = false;
            currChamp.GetComponent<AllyChampController>().targetTile = null;
            currChamp.SetActive(true);
            currChamp.GetComponent<AllyChampController>().distanceToTarget = Mathf.Infinity;
            currChamp.GetComponent<AllyChampController>().target = null;
            currChamp.GetComponent<AllyChampController>().healthBar.value = currChamp.GetComponent<AllyChampController>().health = 100;
            currChamp.GetComponent<AllyChampController>().transform.forward = new Vector3(0, 0, 1);

            foreach (Tile t in currChamp.GetComponent<AllyChampController>().shortestPath)
            {
                t.inShortestPath = false;
            }
            currChamp.GetComponent<AllyChampController>().shortestPath.Clear();
        }
    }
    public static void ChangeChampionsState(ChampionState state)
    {
        foreach (GameObject obj in champions)
        {
            obj.GetComponent<AllyChampController>().championState = state;
        }
    } // changing the champion state to smth
    public static bool ChampionsWin()
    {
        bool allEnemiesDead = true;

        foreach (GameObject obj in enemies)
        {
            if (obj.activeInHierarchy)
            {
                allEnemiesDead = false;
                break;
            }
        }

        return allEnemiesDead;
    } // checking if you win the round

    public static void SaveEnemiesStartRoundPositions() // saving position of each enemy in array
    {
        foreach (GameObject obj in enemies)
        {
            enemiesStartRoundPositions.Add(obj.transform.position);
        }
    }
    public static void ResetEnemies() // resetting the enemies for buying round
    {
        for (int i = 0; i < enemies.Length; ++i)
        {
            GameObject currEnemy = enemies.ElementAt(i);
            if (currEnemy == null)
                return;

            currEnemy.transform.position = enemiesStartRoundPositions.ElementAt(i); // resetvame poziciq
            currEnemy.GetComponent<EnemyChampController>().isDead = false;
            currEnemy.GetComponent<EnemyChampController>().isTargetInRange = false;
            currEnemy.GetComponent<EnemyChampController>().moving = false;
            currEnemy.GetComponent<EnemyChampController>().targetTile = null;
            currEnemy.SetActive(true);
            currEnemy.GetComponent<EnemyChampController>().distanceToTarget = Mathf.Infinity;
            currEnemy.GetComponent<EnemyChampController>().target = null;
            currEnemy.GetComponent<EnemyChampController>().healthBar.value = currEnemy.GetComponent<EnemyChampController>().health = 100;
            currEnemy.GetComponent<EnemyChampController>().transform.forward = new Vector3(0, 0, -1);

            foreach (Tile t in currEnemy.GetComponent<EnemyChampController>().shortestPath)
            {
                t.inShortestPath = false;
            }
            currEnemy.GetComponent<EnemyChampController>().shortestPath.Clear();
        }
    }
    public static void ChangeEnemiesState(ChampionState state)
    {
        foreach (GameObject obj in enemies)
        {
            obj.GetComponent<EnemyChampController>().championState = state;
        }
    } // changing the enemy state to smth
    public static bool EnemiesWin()
    {
        bool allChampionsDead = true;

        foreach (GameObject obj in champions)
        {
            if (obj.activeInHierarchy)
            {
                allChampionsDead = false;
                break;
            }
        }

        return allChampionsDead;
    } // checking if you lose the round

    public static void OnWonRound()
    {
        print("you win");
        ChangeChampionsState(ChampionState.Idle);
    } // do that when win a round
    public static void OnLostRound()
    {
        print("enemies win");
        ChangeEnemiesState(ChampionState.Idle);
    } // do that when lose a round
}