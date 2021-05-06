using UnityEngine;

public class WarriorBuff9 : SynergyBuff
{
    private void Start() 
    {
        Init();

        armor = 45;

        GiveArmor();
    }
    private void OnDestroy()
    {
        DecreaseArmor();
    }

    private void GiveArmor()
    {
        receiver.Armor += armor;
    }
    private void DecreaseArmor()
    {
        receiver.Armor -= armor;
    }
}
