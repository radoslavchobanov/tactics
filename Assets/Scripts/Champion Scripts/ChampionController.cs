﻿using System.Collections;
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

    //A* path
    public Stack<Tile> shortestPath = new Stack<Tile>();

    protected float timeForNextAttack = 0;

    private static float halfHeight = 0;
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
        return TacticsMove.singleton.GetChampionTile(gameObject);
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
        gameObject.transform.position += new Vector3(0, 0, 1) * Time.deltaTime * MovementSpeed;
    }
    public void MoveChampionTowardsTarget()
    {
        if (!IsTargetInRange() || moving)
        {
            MoveToNextTileFromShortestPath();
            timeForNextAttack = Time.time + (1 / AttackSpeed); // in order not to attack instantly when target is reached

            //CalculateDistanceToTarget();
            //gameObject.transform.position += GetDirectionToTarget() * Time.deltaTime * movementSpeed;
        }
        else if (IsTargetInRange())
            championState = ChampionState.Attacking;
    }
    public void MoveToNextTileFromShortestPath()
    {
        if (shortestPath.Count > 0 && !moving)
        {
            var nextTile = shortestPath.Pop();

            // if (nextTile.reserved == false && nextTile.IsTileEmpty())
            // {
            //     nextTile.reserved = true;
            //     targetTile = nextTile;

            //     var timeSpan = System.DateTime.Now - GameController.startRoundTime;
            //     Debug.Log( timeSpan.Seconds + ":" + timeSpan.Milliseconds + " : " + this.name + "targeted" + targetTile.name );
            // }
            // else if (nextTile.reserved == false && !nextTile.IsTileEmpty())
            // {
            //     return;
            // }
            // else if (nextTile.reserved == true)
            // {
            //     this.championState = ChampionState.OnWaiting;
            //     return;
            // }

            // ima nqkolko sluchaq: 
            // 1 -> sledvashtiq tile na koito trqbva da otide geroq e zaet (ima nqkoi na nego), !IsTileEmpty(), togava 'targetTile' trqbva da e nai blizkiq do NE zaet, NE rezerviran
            // 2 -> na 'targetTile' nqma nikoi, no nqkoi go e rezerviral che otiva na nego (reserved = true). togava pak 'targetTile' trqbva da e nai blizkiq do NE zaet, NE rezerviran
            // 3 -> na 'targetTile' nqma nikoi i ne e rezerviran. Otivai na nego.
            
            targetTile = nextTile;
        }

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
                Vector3 velocity = heading * MovementSpeed;

                gameObject.transform.forward = heading;
                gameObject.transform.position += velocity * Time.deltaTime;
            }
            else if (Vector3.Distance(gameObject.transform.position, targetPos) < 0.01f)
            {
                tile.reserved = false; // freeing the reserved tile when arriving on it

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
        else MoveChampionForward();
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

        float physicalDmgReduction = ((float)targetController.Armor / 100); // physical dmg reduction from target armor
        int dmg = AttackDamage - (int)(AttackDamage * physicalDmgReduction);

        targetController.Health -= dmg;
        targetController.healthBar.value = targetController.Health;

        if (targetController.Health <= 0)
        {
            OnTargetDead();
        }
    }
    public void FightTarget()
    {
        CalculateDistanceToTarget();
        if (distanceToTarget > AttackRange) // ako po vreme na bitka, targeta se otdalechi ot range da trugva kam nego
        {
            championState = ChampionState.Moving;
            return;
        }

        if (!target.GetComponent<ChampionController>().isDead && Time.time > timeForNextAttack)
        {
            AttackTarget();
            timeForNextAttack = Time.time + (1 / AttackSpeed);
        }
        else if (target.GetComponent<ChampionController>().isDead)
            championState = ChampionState.Moving;
    }

    public bool IsTargetInRange()
    {
        CalculateDistanceToTarget();
        if (distanceToTarget >= AttackRange)
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
        if (target.GetComponent<ChampionController>().Health <= 0)
            return true;
        return false;
    }

    // A* functions ---------------
    public void CalculatePaths()
    {
        Tile startTile = GetCurrentTile();
        Tile targetTile = target.GetComponent<ChampionController>().GetCurrentTile();

        TacticsMove.singleton.GetAdjacentTiles(targetTile);

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
            currTile.isInShortestPath = true;
            currTile = currTile.parent;
        }
    }
    public Tile FindLowestFCost(List<Tile> list)
    {
        Tile lowest = list[0];
        foreach (Tile t in list)
        {
            if (!t.isPlayerOn && t.fCost < lowest.fCost || t.fCost == lowest.fCost && t.hCost < lowest.hCost)
            {
                lowest = t;
            }
        }
        list.Remove(lowest);
        return lowest;
    }
    // ----------------------------
}