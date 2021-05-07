using UnityEngine;

public class ArcherBuff3 : SynergyBuff
{
    private void Start() 
    {
        Init();

        attackDamage = 30;

        GiveAttackDamage();
    }
    private void OnDestroy()
    {
        DecreaseAttackDamage();
    }

    private void GiveAttackDamage()
    {
        receiver.AttackDamage += attackDamage;
    }
    private void DecreaseAttackDamage()
    {
        receiver.AttackDamage -= attackDamage;
    }
}
