using UnityEngine;

public class SynergyBuff : MonoBehaviour
{
    protected Champion receiver; // the champion thats getting the buff
    protected int attackDamage;
    protected int armor;

    // make Start function virtual or just to call it in every specific SynergyBuff Start method !!! 
    public void Init()
    {
        if (gameObject?.GetComponent<Champion>() == null)
        {
            print("!!! No CHAMPION component !!!");
            return;
        }
        receiver = gameObject.GetComponent<Champion>();
    }
}