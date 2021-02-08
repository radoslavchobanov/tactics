using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyBuffController : MonoBehaviour
{
    private List<GameObject> champions;

    private void Start()
    {
        SynergyController.singleton.HumanCounter3.AddListener(() => GiveHumanBuff3());
    }

    private void GiveHumanBuff3()
    {
        print("Give human buff 3 !!!");
    }
}
