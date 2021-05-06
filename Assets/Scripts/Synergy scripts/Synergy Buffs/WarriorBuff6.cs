using UnityEngine;

public class WarriorBuff6 : SynergyBuff
{
    private void Start() 
    {
        Init();

        armor = 30;

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
