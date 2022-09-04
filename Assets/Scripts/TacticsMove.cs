using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public enum GameState { BuyingRound, FightingRound };

public class TacticsMove : MonoBehaviour
{
    public GameObject arthurPrefab;
    public GameObject aragornPrefab;
    //singleton -----------------------------------------------------------
    public static TacticsMove singleton;
    //---------------------------------------------------------------------

    //UI shop vars --------------------------------------------------------
    public  Button aragornButton, sarumanButton, legolasButton, arthurButton;
    //---------------------------------------------------------------------

    //UI synergy vars for UI synergy text panel11 -------------------------
    public Text humanText, orcText, elfText, warriorText, archerText, mageText;
    //---------------------------------------------------------------------

    //tile variables ------------------------------------------------------
    public GameObject[] map; // array with all the tiles
    //---------------------------------------------------------------------

    //champion variables --------------------------------------------------
    [SerializeField] private List<GameObject> championsOnScreen; // list with all the ally champions on the screen
    [SerializeField] private List<GameObject> championsOnBench; // list with all the ally champions on the bench
    [SerializeField] private List<GameObject> championsOnBoard; // list with all the ally champions on the board
    private List<Vector3> championsStartRoundPositions; // list with all the starting positions of the champions
    //---------------------------------------------------------------------

    //enemy variables -----------------------------------------------------
    public GameObject[] enemies; // list with all the enemies on the board
    public List<Vector3> enemiesStartRoundPositions; // list with all the starting positions of the enemies
    //---------------------------------------------------------------------

    //game variables ------------------------------------------------------
    public GameState gameState; // type of the round - buying or fighting
    public int numberOfRound; // the number of rounds from the beginning
    //---------------------------------------------------------------------

    public List<GameObject> ChampionsOnScreen { get => championsOnScreen; private set => championsOnScreen = value; }
    public List<GameObject> ChampionsOnBench { get => championsOnBench; private set => championsOnBench = value; }
    public List<GameObject> ChampionsOnBoard { get => championsOnBoard; private set => championsOnBoard = value; }
    public List<Vector3> ChampionsStartRoundPositions { get => championsStartRoundPositions; private set => championsStartRoundPositions = value; }

    private void Awake()
    {
        if (singleton != null && singleton != this)
            Destroy(this.gameObject);
        else
            singleton = this;
    }

    private void OnBuyRoundStart() => print("Buy round start");
    private void OnBuyRoundEnd() => print("Buy round end");
    private void OnFightRoundStart() => print("Fight round start");
    private void OnFightRoundEnd() => print("Fight round end");

    void Start()
    {
        // Events init
        TacticsMoveEvents.ChampionEnteredBattlefield.AddListener(OnChampionEnteredBattlefield);
        TacticsMoveEvents.ChampionLeftBattlefield.AddListener(OnChampionLeftBattlefield);
        
        TacticsMoveEvents.ChampionEnteredBattlefield.AddListener(SynergyController.singleton.AddIntoSyngeries);
        TacticsMoveEvents.ChampionLeftBattlefield.AddListener(SynergyController.singleton.RemoveFromSynergies);

        TacticsMoveEvents.BuyRoundStart.AddListener(OnBuyRoundStart);
        TacticsMoveEvents.BuyRoundEnd.AddListener(OnBuyRoundEnd);
        TacticsMoveEvents.FightRoundStart.AddListener(OnFightRoundStart);
        TacticsMoveEvents.FightRoundEnd.AddListener(OnFightRoundEnd);

        //UI shop vars
        arthurButton.onClick.AddListener(() => { CreateChampion(arthurPrefab, new Vector3(4, 0.51f, 0)); });
        aragornButton.onClick.AddListener(() => { CreateChampion(aragornPrefab, new Vector3(4, 1, 0)); });
        sarumanButton.onClick.AddListener(() => { CreateChampion("Saruman", new Vector3(5, 1.4f, 0), Color.white); });
        legolasButton.onClick.AddListener(() => { CreateChampion("Legolas", new Vector3(6, 1.4f, 0), Color.yellow); });

        //UI synergy vars | all set

        //tile variables
        map = GameObject.FindGameObjectsWithTag("Tile");

        //player variables 
        championsOnScreen = new List<GameObject>();
        championsOnBench = new List<GameObject>();
        championsOnBoard = new List<GameObject>();

        championsStartRoundPositions = new List<Vector3>();

        //enemy variables
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesStartRoundPositions = new List<Vector3>();

        //game variables
        gameState = new GameState();
        numberOfRound = 0;

        //func calls on start
    }
    private void Update()
    {
        if (gameState == GameState.FightingRound)
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
        else if (gameState == GameState.BuyingRound)
        {

        }
    }

    //Tile functions -------------------------------------------
    public Tile GetChampionTile(GameObject champion) // getting the current tile of a champion
    {
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(champion.transform.position, Vector3.down, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }
    public void MakeHomeTilesSelectable()
    {
        foreach (GameObject tileObject in map)
        {
            Tile tile = tileObject.GetComponent<Tile>();

            if (tile.isHomeBattlefieldTile == true)
                tile.isSelectable = true;
        }
    }
    public void ClearSelectedTiles()
    {
        foreach (GameObject tileObject in map)
        {
            Tile tile = tileObject.GetComponent<Tile>();

            if (tile.isSelectable)
                tile.isSelectable = false;
        }
    }
    public void GetAdjacentTiles(Tile targetTile) // getting and storing the neighbor of every tile on the map in adjacent list
    {
        foreach (GameObject tile in map)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(targetTile);
        }
    }
    public void ResetTiles()
    {
        foreach (GameObject obj in map)
        {
            Tile t = obj.GetComponent<Tile>();
            t.reserved = false;
        }
    }
    //----------------------------------------------------------

    //Champions functions -----------------------------------------
    public void SaveChampionsStartRoundPositions() // saving position of each champion in array
    {
        foreach (GameObject obj in ChampionsOnScreen)
        {
            championsStartRoundPositions.Add(obj.transform.position);
        }
    }
    public void ResetChampionsForBuyRound() // resetting the champions for buying round
    {
        for (int i = 0; i < ChampionsOnScreen.Count; ++i)
        {
            GameObject currChamp = ChampionsOnScreen.ElementAt(i);
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
            currChamp.GetComponent<AllyChampController>().Health = (int)currChamp.GetComponent<AllyChampController>().healthBar.value;
            currChamp.GetComponent<AllyChampController>().transform.forward = new Vector3(0, 0, 1);

            foreach (Tile t in currChamp.GetComponent<AllyChampController>().shortestPath)
            {
                t.isInShortestPath = false;
            }
            currChamp.GetComponent<AllyChampController>().shortestPath.Clear();
        }
    }
    public void ChangeChampionsState(ChampionState state) // changing the champions state to smth
    {
        foreach (GameObject obj in ChampionsOnScreen)
        {
            obj.GetComponent<AllyChampController>().championState = state;
        }
    }
    public bool ChampionsWin() // checking if you win the round
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
    //-------------------------------------------------------------

    //Enemies functions -------------------------------------------
    public void SaveEnemiesStartRoundPositions() // saving position of each enemy in array
    {
        foreach (GameObject obj in enemies)
        {
            enemiesStartRoundPositions.Add(obj.transform.position);
        }
    }
    public void ResetEnemies() // resetting the enemies for buying round
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
            currEnemy.GetComponent<EnemyChampController>().Health = (int)currEnemy.GetComponent<EnemyChampController>().healthBar.value;
            currEnemy.GetComponent<EnemyChampController>().transform.forward = new Vector3(0, 0, -1);

            foreach (Tile t in currEnemy.GetComponent<EnemyChampController>().shortestPath)
            {
                t.isInShortestPath = false;
            }
            currEnemy.GetComponent<EnemyChampController>().shortestPath.Clear();
        }
    }
    public void ChangeEnemiesState(ChampionState state) // changing the enemy state to smth
    {
        foreach (GameObject obj in enemies)
        {
            obj.GetComponent<EnemyChampController>().championState = state;
        }
    }
    public bool EnemiesWin() // checking if you lose the round
    {
        if (ChampionsOnScreen.Count == 0)
        {
            print("No champions");
            return true;
        }

        bool allChampionsDead = true;

        foreach (GameObject obj in ChampionsOnScreen)
        {
            if (obj.activeInHierarchy)
            {
                allChampionsDead = false;
                break;
            }
        }

        return allChampionsDead;
    }
    //-------------------------------------------------------------

    public void OnWonRound() // do that when win a round
    {
        print("you win");
        ChangeChampionsState(ChampionState.Idle);
    }
    public void OnLostRound() // do that when lose a round
    {
        print("enemies win");
        ChangeEnemiesState(ChampionState.Idle);
    }
    private void OnChampionEnteredBattlefield(Champion champ)
    {
        ChampionsOnBench.Remove(champ.gameObject);
        ChampionsOnBoard.Add(champ.gameObject);
    }
    private void OnChampionLeftBattlefield(Champion champ)
    {
        ChampionsOnBoard.Remove(champ.gameObject);
        ChampionsOnBench.Add(champ.gameObject);
    }

    public GameObject CreateChampion(string name, Vector3 startingPos, Color color) // creating champion NAME on given position with given color
    {
        GameObject champ = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        // adding the correct script to the object of type name
        Type champType = Type.GetType(name);
        champ.AddComponent(champType);

        champ.name = name;
        champ.tag = "Champion";

        champ.transform.position = startingPos;
        champ.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

        champ.GetComponent<Renderer>().material.color = color;

        GameObject Canvas = CreateCanvas(); // canvas for the slider and text
        GameObject healthbarSlider = CreateSlider(); // slider for the healthbar
        CreateText(); // text for the champion stats panel

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

            slider.transform.SetParent(Canvas.transform, false);
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
        GameObject CreateText()  //creating text for the stats
        {
            GameObject text = new GameObject();
            text.name = "Text";
            text.AddComponent<RectTransform>();
            text.AddComponent<Text>();

            // text.SetActive(false);
            text.GetComponent<Text>().enabled = false;

            text.transform.position = new Vector3(1.2f, 1.29f, 0.22f);
            text.transform.eulerAngles = new Vector3(80, 0, 0);
            text.transform.localScale = new Vector3(0.00783f, 0.007f, 0);

            text.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text.GetComponent<Text>().fontStyle = FontStyle.Normal;
            text.GetComponent<Text>().fontSize = 30;
            text.GetComponent<Text>().color = Color.white;
            text.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;

            text.transform.SetParent(Canvas.transform, false);
            return text;
        }

        ChampionsOnScreen.Add(champ);
        ChampionsOnBench.Add(champ);
        return champ;
    }
    public GameObject CreateChampion(GameObject prefab, Vector3 startingPos)
    {
        GameObject champ = Instantiate(prefab, startingPos, Quaternion.identity);
        ChampionsOnScreen.Add(champ);
        ChampionsOnBench.Add(champ);

        return champ;
    }
    
    public void UpdateSynergiesTextVisuals()
    {
        humanText.text = "Humans - " + SynergyController.singleton.HumanCounter;
        orcText.text = "Orcs - " + SynergyController.singleton.OrcCounter;
        elfText.text = "Elfs - " + SynergyController.singleton.ElfCounter;

        warriorText.text = "Warriors - " + SynergyController.singleton.WarriorCounter;
        archerText.text = "Archers - " + SynergyController.singleton.ArcherCounter;
        mageText.text = "Mages - " + SynergyController.singleton.MageCounter;
    }
}