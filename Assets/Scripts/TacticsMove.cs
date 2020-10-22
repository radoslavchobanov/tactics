using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { BuyingRound, FightingRound };


public class TacticsMove : MonoBehaviour
{
//tiles variables
    public static GameObject[] map; // array with all the tiles

//players variables 
    public static List<GameObject> champions; // list with all the champions on the board
    public static List<Vector3> championsStartRoundPositions; // list with all the starting positions of the champions

//enemy variables
    public static GameObject[] enemies; // list with all the enemies on the board
    public static List<Vector3> enemiesStartRoundPositions; // list with all the starting positions of the enemies

//game variables
    public static GameState gameState; // type of the round - buying or fighting
    public static int numberOfRound; // the number of rounds from the beginning

    void Start()
    {
        map = GameObject.FindGameObjectsWithTag("Tile");
        
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        numberOfRound = 0;
        champions = new List<GameObject>();
        championsStartRoundPositions = new List<Vector3>();
        enemiesStartRoundPositions = new List<Vector3>();

        GameObject tempChamp = CreateChampion("Seraphim", new Vector3(6, 1.4f, 0));
        champions.Add(tempChamp);
        tempChamp.AddComponent<Seraphim>().GetComponent<Seraphim>().InitChampion();
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

    public static Tile GetChampionTile(GameObject champion) // getting the current tile of a champion
    {
        RaycastHit hit; 
        Tile tile = null;

        if (Physics.Raycast(champion.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }
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
    public static void GetAdjacentTiles(Tile targetTile) // getting and storing the neighbor of every tile on the map in adjacent list
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
        for (int i=0; i<champions.Count; ++i)
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
            currChamp.GetComponent<AllyChampController>().healthBar.value = currChamp.GetComponent<AllyChampController>().healthBar.maxValue;
            currChamp.GetComponent<AllyChampController>().health = (int)currChamp.GetComponent<AllyChampController>().healthBar.value;
            currChamp.GetComponent<AllyChampController>().transform.forward = new Vector3(0, 0, 1);

            foreach (Tile t in currChamp.GetComponent<AllyChampController>().shortestPath)
            {
                t.inShortestPath = false;
            }
            currChamp.GetComponent<AllyChampController>().shortestPath.Clear();
        }
    }
    public static void ChangeChampionsState(ChampionState state) // changing the champion state to smth
    {
        foreach (GameObject obj in champions)
        {
            obj.GetComponent<AllyChampController>().championState = state;
        }
    }
    public static bool ChampionsWin() // checking if you win the round
    {
        if (enemies == null)
        {
            print("No enemies");
            return false;
        }

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
    }

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
            currEnemy.GetComponent<EnemyChampController>().healthBar.value = currEnemy.GetComponent<EnemyChampController>().healthBar.maxValue;
            currEnemy.GetComponent<EnemyChampController>().health = (int)currEnemy.GetComponent<EnemyChampController>().healthBar.value;
            currEnemy.GetComponent<EnemyChampController>().transform.forward = new Vector3(0, 0, -1);

            foreach (Tile t in currEnemy.GetComponent<EnemyChampController>().shortestPath)
            {
                t.inShortestPath = false;
            }
            currEnemy.GetComponent<EnemyChampController>().shortestPath.Clear();
        }
    }
    public static void ChangeEnemiesState(ChampionState state) // changing the enemy state to smth
    {
        foreach (GameObject obj in enemies)
        {
            obj.GetComponent<EnemyChampController>().championState = state;
        }
    }
    public static bool EnemiesWin() // checking if you lose the round
    {
        if (champions.Count == 0)
        {
            print("No champions");
            return false;
        }

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
    }

    public static void OnWonRound() // do that when win a round
    {
        print("you win");
        ChangeChampionsState(ChampionState.Idle);
    }
    public static void OnLostRound() // do that when lose a round
    {
        print("enemies win");
        ChangeEnemiesState(ChampionState.Idle);
    }

    public GameObject CreateChampion(string name, Vector3 startingPos)
    {
        GameObject champ = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        champ.name = name;
        champ.tag = "Champion";

        champ.transform.position = startingPos;
        champ.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

        champ.GetComponent<Renderer>().material.color = Color.red;

        GameObject sliderCanvas = CreateCanvas(); // canvas for the slider
        GameObject healthbarSlider = CreateSlider(); // slider for the healthbar

        //adding all the scripts for champion
        champ.AddComponent<AllyChampController>();
        champ.AddComponent<DragObject>();
        champ.AddComponent<FindClosestEnemy>();
        champ.AddComponent<HeadingController>();

        champ.GetComponent<AllyChampController>().healthBar = healthbarSlider.GetComponent<Slider>();

        GameObject CreateCanvas() //creating canvas for the slider
        {
            GameObject canvas = new GameObject();
            canvas.name = "Canvas";
            canvas.AddComponent<RectTransform>();
            canvas.AddComponent<Canvas>();
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();
            canvas.transform.SetParent(champ.transform, false);

            return canvas;
        }
        GameObject CreateSlider()  //creating slider for the healthbar
        {
            GameObject slider = new GameObject();
            slider.name = "Slider";
            slider.AddComponent<RectTransform>();
            slider.AddComponent<Slider>();

            slider.transform.position = new Vector3(0, 1.24f, 0.325f);
            slider.transform.eulerAngles = new Vector3(60, 0, 0);
            slider.transform.localScale = new Vector3(0.0087f, 0.0018f, 0);

            GameObject sliderBackground = CreateBackGround();
            GameObject sliderFillArea = CreateFillArea();
            GameObject sliderHandleArea = CreateHandleSlideArea();

            slider.GetComponent<Slider>().interactable = false;
            slider.GetComponent<Slider>().fillRect = sliderFillArea.transform.GetChild(0).GetComponent<RectTransform>();
            slider.GetComponent<Slider>().maxValue = 100;
            slider.GetComponent<Slider>().wholeNumbers = true;
            slider.GetComponent<Slider>().value = slider.GetComponent<Slider>().maxValue;

            slider.transform.SetParent(sliderCanvas.transform, false);
            return slider;

            GameObject CreateBackGround()
            {
                GameObject background = new GameObject();
                background.name = "Background";
                background.AddComponent<RectTransform>();
                background.AddComponent<CanvasRenderer>();
                background.AddComponent<Image>();

                background.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 50);
                background.GetComponent<Image>().color = Color.red;

                background.transform.SetParent(slider.transform, false);
                return background;
            }
            GameObject CreateFillArea()
            {
                GameObject fillArea = new GameObject();
                fillArea.name = "Fill Area";
                fillArea.AddComponent<RectTransform>();

                fillArea.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 50);

                GameObject fill = CreateFill();

                fillArea.transform.SetParent(slider.transform, false);
                return fillArea;

                GameObject CreateFill()
                {
                    GameObject tempFill = new GameObject();
                    tempFill.name = "Fill";
                    tempFill.AddComponent<RectTransform>();
                    tempFill.AddComponent<CanvasRenderer>();
                    tempFill.AddComponent<Image>();

                    tempFill.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                    tempFill.GetComponent<Image>().color = Color.green;

                    tempFill.transform.SetParent(fillArea.transform, false);
                    return tempFill;
                }
            }
            GameObject CreateHandleSlideArea()
            {
                GameObject handleSlideArea = new GameObject();
                handleSlideArea.name = "Handle Slide Area";
                handleSlideArea.AddComponent<RectTransform>();

                handleSlideArea.transform.SetParent(slider.transform, false);
                return handleSlideArea;
            }
        }

        return champ;
    }
}