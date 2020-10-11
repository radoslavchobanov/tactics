using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : Champion
{
    private float timeForNextAttack = 0;

    public Slider healthBar;

    public GameObject target; // focused enemy
    public float distanceToTarget;
    public ChampionState enemyState;

    //enemy flags
    public bool isDead;

    void Start()
    {
        target = null;
        distanceToTarget = Mathf.Infinity;

        isDead = false;

        InitChampion();
    }
    
    void Update()
    {
        switch (TacticsMove.gameState)
        {
            case GameState.FightingRound:
                if (enemyState == ChampionState.Moving)
                    MoveEnemy();
                else if (enemyState == ChampionState.Attacking)
                    FightTarget();

                break;

            case GameState.BuyingRound:
                enemyState = ChampionState.Idle;
                break;
        }
    }

    void MoveEnemyForward()
    {
        gameObject.transform.position += new Vector3(0, 0, -1) * Time.deltaTime * movementSpeed;
    }
    void MoveEnemyTowardsTarget()
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
            enemyState = ChampionState.Attacking;
    }
    void MoveEnemy()
    {
        // play running animation
        if (target)
            MoveEnemyTowardsTarget();
        else if (!target)
            MoveEnemyForward();
    }

    void OnTargetDead()
    {
        target.GetComponent<AllyChampController>().isDead = true;
        target.transform.position = new Vector3(target.transform.position.x + 100, target.transform.position.y, target.transform.position.z);
        target.SetActive(false);
    }
    void AttackTarget()
    {
        // play attack animation
        target.GetComponent<AllyChampController>().health -= 1 * attackDamage;
        target.GetComponent<AllyChampController>().healthBar.value = target.GetComponent<AllyChampController>().health;

        if (target.GetComponent<AllyChampController>().health <= 0)
        {
            OnTargetDead();
        }
    }
    void FightTarget()
    {
        //funckiq za biene na target....
        if (!target.GetComponent<AllyChampController>().isDead && Time.time > timeForNextAttack)
        {
            AttackTarget();
            timeForNextAttack = Time.time + (1 / attackSpeed);
        }
        else if (target.GetComponent<AllyChampController>().isDead)
        {
            enemyState = ChampionState.Moving;
        }
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
        if (target.GetComponent<AllyChampController>().health <= 0)
        {
            target.transform.position = new Vector3(target.transform.position.x + 100, target.transform.position.y, target.transform.position.z);
            return true;
        }
        return false;
    }
}