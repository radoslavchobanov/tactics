using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //main variables
    public bool homeTile = false;
    public bool walkable = true;
    public bool selectable = true;
    public bool pointed = false;
    public bool isPlayerOn = false;
    public Tile parent = null;
    public bool inShortestPath = false;

    public List<Tile> adjacentTileList = new List<Tile>();

    // vars for A*
    public float fCost = 0;
    public float hCost = 0;
    public float gCost = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.position.z < 6)
            homeTile = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (pointed)
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
        else if (selectable)
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
        if (homeTile)
            pointed = true;
    }
    private void OnMouseExit()
    {
        if (pointed)
            pointed = false;
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

                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || (tile == targetTile))
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
}