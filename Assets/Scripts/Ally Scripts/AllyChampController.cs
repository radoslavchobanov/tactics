using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class AllyChampController : Champion
{
// controller variables
    private Tile previousPointedTile;
    private Tile currentPlacedTile;
    private float timeForNextAttack = 0;

    public Slider healthBar;

    public GameObject target; // focused enemy
    public float distanceToTarget;
    public ChampionState championState;

    //champ flags
    public bool selected;
    public bool isDead;

    void Start()
    {
        //controller variables
        previousPointedTile = GetCurrentTile();
        currentPlacedTile = GetCurrentTile();

        target = null;
        distanceToTarget = Mathf.Infinity;

        //champ flags
        selected = false;
        isDead = false;

        InitChampion();
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

    void OnMouseDown()
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

    void MoveChampionForward()
    {
        gameObject.transform.position += new Vector3(0, 0, 1) * Time.deltaTime * movementSpeed;
    }
    void MoveChampionTowardsTarget()
    {
        if (target == null)
            return;

        Vector3 headingTowardTarget = target.transform.position - gameObject.transform.position; // posoka kam targeta
        distanceToTarget = headingTowardTarget.magnitude; // distanciq do targeta
        Vector3 directionToTarget = headingTowardTarget / distanceToTarget; // posoka do targeta

        if (!IsTargetReached())
        {
            gameObject.transform.position += directionToTarget * Time.deltaTime * movementSpeed;

            timeForNextAttack = Time.time + (1 / attackSpeed); // in order not to attack instantly when target is reached
        }
        else if (IsTargetReached())
            championState = ChampionState.Attacking;
    }
    void MoveChampion()
    {
        // play running animation
        if (target)
            MoveChampionTowardsTarget();
        else if (!target)
            MoveChampionForward();
    }
    
    void OnTargetDead()
    {
        target.GetComponent<EnemyController>().isDead = true;
        target.transform.position = new Vector3(target.transform.position.x + 100, target.transform.position.y, target.transform.position.z);
        target.SetActive(false);
    }
    void AttackTarget()
    {
        // play attack animation
        target.GetComponent<EnemyController>().health -= 1 * attackDamage;
        target.GetComponent<EnemyController>().healthBar.value = target.GetComponent<EnemyController>().health;

        if (target.GetComponent<EnemyController>().health <= 0)
        {
            OnTargetDead();
        }
    }
    void FightTarget()
    {
        //funckiq za biene na target....
        if (!target.GetComponent<EnemyController>().isDead && Time.time > timeForNextAttack)
        {
            AttackTarget();
            timeForNextAttack = Time.time + (1 / attackSpeed);
        }
        else if (target.GetComponent<EnemyController>().isDead)
            championState = ChampionState.Moving;
    }

    bool IsTargetReached()
    {
        if (target == null)
            return false;

        if (distanceToTarget >= attackRange)
            return false;
        return true;
    }
    bool IsTargetDead() 
    {
        if (target.GetComponent<EnemyController>().health <= 0)
            return true;
        return false;
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