using UnityEngine;

public class HumanBuff9 : SynergyBuff
{
    private void Start() 
    {
        Init();

        attackDamage = 90;

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
