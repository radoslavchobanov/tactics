using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyBuffGiver : MonoBehaviour
{

    private void AddBuff(string buffName, GameObject champ)
    {
        System.Type buffScript = System.Type.GetType(buffName);

        champ.AddComponent(buffScript);
    }
    private void RemoveBuff(Component buffName, GameObject champ)
    {

    }

    public void GiveHumanBuff3()
    {
        // print("Give human buff 3 !!!");

        foreach (GameObject champObj in TacticsMove.singleton.ChampionsOnBoard)
        {
            if (champObj.GetComponent<Champion>()?._Race == Race.Human)
                champObj.AddComponent<HumanBuff3>();
        }
    }
    protected void RemoveHumanBuff3()
    {
        // print("Remove human buff 3 !!!");

        foreach (GameObject champObj in TacticsMove.singleton.ChampionsOnScreen)
        {
            if (champObj.GetComponent<Champion>()?._Race == Race.Human && champObj.GetComponent<HumanBuff3>() != null)
                Destroy(champObj.GetComponent<HumanBuff3>());
        }
    }
    protected void GiveHumanBuff6()
    {
        // print("Give human buff 6 !!!");

        foreach (GameObject champObj in TacticsMove.singleton.ChampionsOnBoard)
        {
            if (champObj.GetComponent<Champion>()?._Race == Race.Human && champObj.GetComponent<HumanBuff6>() == null)
                champObj.AddComponent<HumanBuff6>();
        }
    }
    protected void RemoveHumanBuff6()
    {
        // print("Remove human buff 6 !!!");

        foreach (GameObject champObj in TacticsMove.singleton.ChampionsOnScreen)
        {
            if (champObj.GetComponent<Champion>()?._Race != Race.Human)
                continue;

            if (champObj.GetComponent<HumanBuff6>() == null)
            {
                print("!!! Champion [" + champObj + "] doesn't have HumanBuff6 component !!!");
                continue;
            }

            Destroy(champObj.GetComponent<HumanBuff6>());
        }
    }
    protected void GiveHumanBuff9()
    {
        // print("Give human buff 9 !!!");

        foreach (GameObject champObj in TacticsMove.singleton.ChampionsOnBoard)
        {
            if (champObj.GetComponent<Champion>()?._Race == Race.Human && champObj.GetComponent<HumanBuff9>() == null)
                champObj.AddComponent<HumanBuff9>();
        }
    }
    protected void RemoveHumanBuff9()
    {
        // print("Remove human buff 9 !!!");

        foreach (GameObject champObj in TacticsMove.singleton.ChampionsOnScreen)
        {
            if (champObj.GetComponent<Champion>()?._Race != Race.Human)
                continue;

            if (champObj.GetComponent<HumanBuff9>() == null)
            {
                print("!!! Champion [" + champObj + "] doesn't have HumanBuff9 component !!!");
                continue;
            }

            Destroy(champObj.GetComponent<HumanBuff9>());
        }
    }
}