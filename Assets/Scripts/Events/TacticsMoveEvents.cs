using UnityEngine.Events;

public class ChampionEnteredEvent : UnityEvent<Champion> {}

public static class TacticsMoveEvents
{
    public static ChampionEnteredEvent ChampionEnteredBattlefield = new ChampionEnteredEvent();
    public static ChampionEnteredEvent ChampionLeftBattlefield = new ChampionEnteredEvent();

    public static UnityEvent BuyRoundStart = new UnityEvent();
    public static UnityEvent BuyRoundEnd = new UnityEvent();
    public static UnityEvent FightRoundStart = new UnityEvent();
    public static UnityEvent FightRoundEnd = new UnityEvent();

}