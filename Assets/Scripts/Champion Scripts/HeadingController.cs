using UnityEngine;

public class HeadingController : MonoBehaviour
{
    private void Update()
    {
        if (gameObject.GetComponent<ChampionController>().isTargetInRange && !gameObject.GetComponent<ChampionController>().moving)
        {
            GameObject target = gameObject.GetComponent<ChampionController>().target;
            Vector3 heading = target.transform.position - gameObject.transform.position;
            gameObject.transform.forward = heading;
        }
    }
}
