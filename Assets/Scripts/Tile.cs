using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    //main variables
    public Tile parent = null;

    public bool isHomeBattlefieldTile = false;
    public bool isWalkable = true;
    public bool isSelectable = false;
    public bool isPointed = false;
    public bool isPlayerOn = false;
    public bool isInShortestPath = false;
    public bool isBench = false;

    public bool reserved; // tile is reserved if there is a champion on it OR any champion is moving towards it

    public List<Tile> adjacentTileList = new List<Tile>();

    // vars for A*
    public float fCost = 0;
    public float hCost = 0;
    public float gCost = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.position.z < 7)
        {
            isHomeBattlefieldTile = true;
        }

        if (gameObject.transform.position.z == 0)
        {
            isBench = true;
            isHomeBattlefieldTile = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPointed)
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
        else if (isSelectable)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        //else if (inShortestPath)
        //{
        //    GetComponent<Renderer>().material.color = Color.black;
        //}
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void OnMouseEnter()
    {
        if (isHomeBattlefieldTile)
            isPointed = true;
    }
    private void OnMouseExit()
    {
        if (isPointed)
            isPointed = false;
    }

    public void Reset()
    {
        adjacentTileList.Clear();

        fCost = hCost = gCost = 0;
    }
    public void FindNeighbors(Tile targetTile)
    {
        Reset();

        FindNeighbor(Vector3.forward, targetTile);
        FindNeighbor(-Vector3.forward, targetTile);
        FindNeighbor(Vector3.right, targetTile);
        FindNeighbor(-Vector3.right, targetTile);
    }
    public void FindNeighbor(Vector3 direction, Tile targetTile)
    {
        Vector3 halfExtends = new Vector3(0.25f, 1 / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtends);

        foreach (Collider collider in colliders)
        {
            Tile tile = collider.GetComponent<Tile>();
            if (tile != null)
            {
                RaycastHit hit;

                if (tile != this && !Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || (tile == targetTile))
                {
                    adjacentTileList.Add(tile);
                }
            }
        }
    }
    public bool IsTileEmpty()
    {
        RaycastHit hit;
        return (!Physics.Raycast(gameObject.transform.position, Vector3.up, out hit, 1));
    }

    public Champion GetChampionOnTile()
    {
        RaycastHit hit;
        Physics.Raycast(gameObject.transform.position, Vector3.up, out hit, 1);
        try
        {
            return hit.collider.gameObject.GetComponent<Champion>();
        }
        catch (NullReferenceException)
        {
            return null;
        }
    }
}