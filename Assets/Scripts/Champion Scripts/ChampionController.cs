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
    public void MoveChampionTowardsTarget() // moves the champion towards his target according to the shortest path
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
    public void MoveToNextTileFromShortestPath() // moves the champion to the next tile from his calculated shortest path
    {
        if (shortestPath.Count > 0 && !moving)
        {
            // popping the next tile from the shortest path
            var nextTile = shortestPath.Pop();
            // if it is not reserved, reserve it
            if (!nextTile.reserved)
                nextTile.reserved = true;
            else if (nextTile.reserved)
            {
                nextTile = nextTile.adjacentTileList[0];
            }
            
            targetTile = nextTile;
        }

        MoveToTile(targetTile);
    }
    public void MoveToTile(Tile tile) // moves the champion to a given 'tile'
    {
        if (tile != null)
        {
            Vector3 targetPos = tile.transform.position;
            targetPos.y += halfHeight + tile.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(gameObject.transform.position, targetPos) >= 0.01f) // starting to move towards the target tile
            {
                moving = true;

                Vector3 heading = targetPos - gameObject.transform.position;
                heading.Normalize();
                Vector3 velocity = heading * MovementSpeed;

                gameObject.transform.forward = heading;
                gameObject.transform.position += velocity * Time.deltaTime;
            }
            else if (Vector3.Distance(gameObject.transform.position, targetPos) < 0.01f) // arriving on the target tile
            {
                moving = false;
                gameObject.transform.position = targetPos;

                tile.reserved = false;

                CalculatePaths();
                FindShortestPath();
            }
        }
    }
    public void MoveChampion() // moves the champion towards his target OR if it has no target, moves forward
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

    public void OnTargetDead() // do that when a champion's current target dies
    {
        target.GetComponent<ChampionController>().championState = ChampionState.Dead;
        target.GetComponent<ChampionController>().isDead = true;
        target.transform.position = new Vector3(target.transform.position.x + 100, target.transform.position.y, target.transform.position.z);
        target.SetActive(false);
    }
    public void AttackTarget() // the champion makes an attack on his current target
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
    public void FightTarget() // the champion battles his current target until it is dead
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

    public bool IsTargetInRange() // checks if target is in range for attack ... TRUE if it is in range, FALSE if not in range
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
        shortestPath.Clear();

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
            if (!t.isPlayerOn && !t.reserved && t.fCost < lowest.fCost || t.fCost == lowest.fCost && t.hCost < lowest.hCost)
            {
                lowest = t;
            }
        }
        list.Remove(lowest);
        return lowest;
    }
    // ----------------------------
}