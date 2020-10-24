using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampionController : Champion
{
    //controller vars
    public GameObject target; // focused enemy  
    public Slider healthBar;
    public float distanceToTarget;
    public ChampionState championState;

    public Stack<Tile> shortestPath = new Stack<Tile>();
    protected float timeForNextAttack = 0;

    private float halfHeight = 0;
    public Tile targetTile = null;

    // flags
    public bool isDead;
    public bool isTargetInRange = false;
    public bool moving = false;

    public void InitController()
    {
        target = null;
        distanceToTarget = Mathf.Infinity;
        halfHeight = 0.9f; /*GetComponent<Collider>().bounds.extents.y;*/ // get collidera nqkuv put vrushta 1 vmesto 0.9f i stava greshka pri vectora, nikva ideq zashto
        isDead = false;
    }

    public Tile GetCurrentTile()
    {
        return TacticsMove.GetChampionTile(gameObject);
    }

    public void CalculateDistanceToTarget()
    {
        Vector3 headingTowardTarget = target.transform.position - gameObject.transform.position; // posoka kam targeta
        distanceToTarget = headingTowardTarget.magnitude; // distanciq do targeta
    }
    public Vector3 GetDirectionToTarget()
    {
        Vector3 headingTowardTarget = target.transform.position - gameObject.transform.position; // posoka kam targeta
        return headingTowardTarget / distanceToTarget; // posoka do targeta
    }

    public void MoveChampionForward()
    {
        gameObject.transform.position += new Vector3(0, 0, 1) * Time.deltaTime * movementSpeed;
    }
    public void MoveChampionTowardsTarget()
    {
        if (!IsTargetInRange() || moving)
        {
            MoveToNextTileFromShortestPath();
            timeForNextAttack = Time.time + (1 / attackSpeed); // in order not to attack instantly when target is reached

            //CalculateDistanceToTarget();
            //gameObject.transform.position += GetDirectionToTarget() * Time.deltaTime * movementSpeed;
        }
        else if (IsTargetInRange())
            championState = ChampionState.Attacking;
    }
    public void MoveToNextTileFromShortestPath()
    {
        if (shortestPath.Count > 0 && !moving)
            targetTile = shortestPath.Pop();

        MoveToTile(targetTile);
    }
    public void MoveToTile(Tile tile)
    {
        if (tile != null)
        {
            Vector3 targetPos = tile.transform.position;
            targetPos.y += halfHeight + tile.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(gameObject.transform.position, targetPos) >= 0.01f)
            {
                moving = true;

                Vector3 heading = targetPos - gameObject.transform.position;
                heading.Normalize();
                Vector3 velocity = heading * movementSpeed;

                gameObject.transform.forward = heading;
                gameObject.transform.position += velocity * Time.deltaTime;
            }
            else if (Vector3.Distance(gameObject.transform.position, targetPos) < 0.01f)
            {
                moving = false;
                gameObject.transform.position = targetPos;
                CalculatePaths();
                FindShortestPath();
            }
        }
    }
    public void MoveChampion()
    {
        // play running animation
        if (target)
        {
            if (shortestPath.Count == 0)
            {
                CalculatePaths();
                FindShortestPath();
            }

            MoveChampionTowardsTarget();
        }
        else if (!target)
            MoveChampionForward();
    }

    public void OnTargetDead()
    {
        target.GetComponent<ChampionController>().isDead = true;
        target.transform.position = new Vector3(target.transform.position.x + 100, target.transform.position.y, target.transform.position.z);
        target.SetActive(false);
    }
    public void AttackTarget()
    {
        // play attack animation
        ChampionController targetController = target.GetComponent<ChampionController>();

        Vector3 heading = target.transform.position - gameObject.transform.position;
        gameObject.transform.forward = heading;

        float physicalDmgReduction = ((float)targetController.armor / 100); // physical dmg reduction from target armor
        int dmg = attackDamage - (int)(attackDamage * physicalDmgReduction);

        targetController.health -= dmg;
        targetController.healthBar.value = targetController.health;

        if (targetController.health <= 0)
        {
            OnTargetDead();
        }
    }
    public void FightTarget()
    {
        CalculateDistanceToTarget();
        if (distanceToTarget > attackRange) // ako po vreme na bitka, targeta se otdalechi ot range da trugva kam nego
        {
            championState = ChampionState.Moving;
            return;
        }

        if (!target.GetComponent<ChampionController>().isDead && Time.time > timeForNextAttack)
        {
            AttackTarget();
            timeForNextAttack = Time.time + (1 / attackSpeed);
        }
        else if (target.GetComponent<ChampionController>().isDead)
            championState = ChampionState.Moving;
    }

    public bool IsTargetInRange()
    {
        CalculateDistanceToTarget();
        if (distanceToTarget >= attackRange)
        {
            isTargetInRange = false;
            return false;
        }
        else
        {
            isTargetInRange = true;
            return true;
        }
    }
    public bool IsTargetDead()
    {
        if (target.GetComponent<ChampionController>().health <= 0)
            return true;
        return false;
    }

    public void CalculatePaths()
    {
        Tile startTile = GetCurrentTile();
        Tile targetTile = target.GetComponent<ChampionController>().GetCurrentTile();

        TacticsMove.GetAdjacentTiles(targetTile);

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(startTile);
        startTile.hCost = Vector3.Distance(startTile.transform.position, targetTile.transform.position);
        startTile.fCost = startTile.hCost;

        while (openList.Count > 0)
        {
            Tile t = FindLowestFCost(openList);
            closedList.Add(t);

            if (t == targetTile)
            {
                return;
            }

            foreach (Tile tile in t.adjacentTileList)
            {
                if (closedList.Contains(tile))
                {
                }
                else if (openList.Contains(tile))
                {
                    float tempGCost = t.gCost + Vector3.Distance(tile.transform.position, t.transform.position);
                    if (tempGCost < tile.gCost)
                    {
                        tile.parent = t;
                        tile.gCost = tempGCost;
                        tile.fCost = tile.gCost + tile.hCost;
                    }
                }
                else
                {
                    tile.parent = t;
                    tile.gCost = t.gCost + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.hCost = Vector3.Distance(tile.transform.position, targetTile.transform.position);
                    tile.fCost = tile.gCost + tile.hCost;
                    openList.Add(tile);
                }
            }
        }
    }
    public void FindShortestPath()
    {
        Tile currTile = target.GetComponent<ChampionController>().GetCurrentTile().parent;
        while (currTile != GetCurrentTile() && shortestPath.Count < target.GetComponent<ChampionController>().GetCurrentTile().fCost - 1)
        {
            shortestPath.Push(currTile);
            //currTile.inShortestPath = true;
            currTile = currTile.parent;
        }
    }
    public Tile FindLowestFCost(List<Tile> list)
    {
        Tile lowest = list[0];
        foreach (Tile t in list)
        {
            if (t.fCost < lowest.fCost || t.fCost == lowest.fCost && t.hCost < lowest.hCost)
            {
                lowest = t;
            }
        }
        list.Remove(lowest);
        return lowest;
    }
}