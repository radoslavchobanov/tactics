using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public GameObject hexPrefab;

    public int gridWidth = 10;
    public int gridHeight = 10;

    float hexWidth = 1.732f;
    float hexHeight = 2;
    public float gap = 0.02f;

    Vector3 startPos;

    private void Awake()
    {
        AddGap();
        CalcStartPos();
        CreateGrid();
    }

    void AddGap()
    {
        hexWidth += hexWidth * gap;
        hexHeight += hexHeight * gap;
    }

    void CalcStartPos()
    {
        startPos = Vector3.zero;
    }

    Vector3 CalcWorldPos(Vector2 gridPos)
    {
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth / 2;

        float x = startPos.x + gridPos.x * hexWidth + offset;
        float z = startPos.z + gridPos.y * hexHeight * 0.75f;

        return new Vector3(x, 0, z);
    }

    void CreateGrid()
    {
        for (int z = 0; z < gridHeight; z++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector2 gridPos = new Vector2(x, z);
                GameObject hex = Instantiate(hexPrefab, CalcWorldPos(gridPos), Quaternion.identity);
                hex.transform.parent = this.transform;
                hex.name = "Tile (" + x + ", " + z + ")";
                hex.tag = "Tile";
                hex.transform.Rotate(90, 0, 0);

                hex.GetComponent<MeshCollider>().convex = true;
                hex.AddComponent<Tile>();
            }
        }
    }
}
