using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public enum GameState { BuyingRound, FightingRound };

public enum ChampionAttackType { Melee, Range };

public class TacticsMove : MonoBehaviour
{
//tiles variables
    public static GameObject[] tiles;

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
        tiles = GameObject.FindGameObjectsWithTag("Tile");

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
        foreach (GameObject tileObject in tiles)
        {
            Tile tile = tileObject.GetComponent<Tile>();

            if (tile.homeTile == true)
                tile.selectable = true;
        }
    }
    public static void ClearSelectedTiles()
    {
        foreach (GameObject tileObject in tiles)
        {
            Tile tile = tileObject.GetComponent<Tile>();

            if (tile.selectable)
                tile.selectable = false;
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
            currChamp.SetActive(true);
            currChamp.GetComponent<AllyChampController>().distanceToTarget = Mathf.Infinity;
            currChamp.GetComponent<AllyChampController>().target = null;
            currChamp.GetComponent<AllyChampController>().healthBar.value = currChamp.GetComponent<AllyChampController>().health = 100;
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
            currEnemy.GetComponent<EnemyController>().isDead = false;
            currEnemy.SetActive(true);
            currEnemy.GetComponent<EnemyController>().distanceToTarget = Mathf.Infinity;
            currEnemy.GetComponent<EnemyController>().target = null;
            currEnemy.GetComponent<EnemyController>().healthBar.value = currEnemy.GetComponent<EnemyController>().health = 100;
        }
    }
    public static void ChangeEnemiesState(ChampionState state)
    {
        foreach (GameObject obj in enemies)
        {
            obj.GetComponent<EnemyController>().enemyState = state;
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