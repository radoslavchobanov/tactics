using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //main variables
    public bool homeTile = false;
    public bool walkable = true;
    public bool selectable = true;
    public bool pointed = false;
    public bool isPlayerOn = false;

    public List<Tile> adjacentTileList = new List<Tile>();

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.position.z < 6)
            homeTile = true;
    }

    // Update is called once per frame
    void Update()
    {
        //CheckForPlayerOn();

        if (pointed)
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
        else if (isPlayerOn)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (selectable)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
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
    public void CheckForPlayerOn()
    {
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, Vector3.up, out hit, 1))
        {
            if (hit.collider.CompareTag("Champion"))
            {
                isPlayerOn = true;
            }
        }
        else
        {
            isPlayerOn = false;
        }
    }
}