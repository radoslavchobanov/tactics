using UnityEngine;

public class ElfBuff9 : SynergyBuff
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
