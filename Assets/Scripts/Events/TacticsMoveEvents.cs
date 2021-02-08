using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class TacticsMoveEvents
{
    public static ChampionEnteredEvent championEnteredBattlefield = new ChampionEnteredEvent();
    public static ChampionEnteredEvent championLeftBattlefield = new ChampionEnteredEvent();

}

public class ChampionEnteredEvent : UnityEvent<Champion>
{
}