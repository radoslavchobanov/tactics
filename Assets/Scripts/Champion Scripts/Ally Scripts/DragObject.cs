using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 offSet;
    private float mouseZCoord;

    AllyChampController champ;

    private void Start()
    {
        champ = gameObject.GetComponent<AllyChampController>(); // taking the champ as a PlayerController object so we can access "selected"
    }

    private void Update()
    {
        if (champ.selected)
        {
            transform.position = new Vector3(GetMouseWorldPos().x + offSet.x, gameObject.transform.position.y, GetMouseWorldPos().z + offSet.z);
        }
    }

    void OnMouseDown()
    {
        mouseZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        offSet = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mouseZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}