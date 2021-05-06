using UnityEngine;
using UnityEngine.Events;

public class SynergyController : SynergyBuffGiver
{
    //singleton -----------------------------------------------------------
    public static SynergyController singleton;
    //---------------------------------------------------------------------

    // race counters
    private int humanCounter;
    private int orcCounter;
    private int elfCounter;
        
    // class counters
    private int warriorCounter;
    private int archerCounter;
    private int mageCounter;

    #region BUFF EVENTS
//BUFF EVENTS -----------------------------------------------------------
        //Human events----------------
    public UnityEvent HumanCounter0;
    public UnityEvent HumanCounter3;
    public UnityEvent HumanCounter6;
    public UnityEvent HumanCounter9;
        //----------------------------
        //Orc events------------------
    public UnityEvent OrcCounter0;
    public UnityEvent OrcCounter3;
    public UnityEvent OrcCounter6;
    public UnityEvent OrcCounter9;
        //----------------------------
        //Elf events------------------
    public UnityEvent ElfCounter0;
    public UnityEvent ElfCounter3;
    public UnityEvent ElfCounter6;
    public UnityEvent ElfCounter9;
        //----------------------------
        //Warrior events--------------
    public UnityEvent WarriorCounter0;
    public UnityEvent WarriorCounter3;
    public UnityEvent WarriorCounter6;
    public UnityEvent WarriorCounter9;
        //----------------------------
        //Archer events---------------
    public UnityEvent ArcherCounter0;
    public UnityEvent ArcherCounter3;
    public UnityEvent ArcherCounter6;
    public UnityEvent ArcherCounter9;
        //----------------------------
        //Mage events-----------------
    public UnityEvent MageCounter0;
    public UnityEvent MageCounter3;
    public UnityEvent MageCounter6;
    public UnityEvent MageCounter9;
//--------------------------------------------------------------
#endregion

    #region CounterSettersAndGetters
    public int HumanCounter
    {
        get { return humanCounter; }
        set 
        { 
            humanCounter = value;

            if (humanCounter < 3) HumanCounter0?.Invoke();
            else if (humanCounter >= 3 && humanCounter < 6) HumanCounter3.Invoke();
            else if (humanCounter >= 6 && humanCounter < 9) HumanCounter6.Invoke();
            else if (humanCounter >= 9) HumanCounter9.Invoke();
        }
    }
    public int OrcCounter
    {
        get { return orcCounter; }
        set 
        { 
            orcCounter = value;
            
            if (orcCounter < 3) OrcCounter0?.Invoke();
            else if (orcCounter >= 3 && orcCounter < 6) OrcCounter3.Invoke();
            else if (orcCounter >= 6 && orcCounter < 9) OrcCounter6.Invoke();
            else if (orcCounter >= 9) OrcCounter9.Invoke();
        }
    }
    public int ElfCounter
    {
        get { return elfCounter; }
        set 
        { 
            elfCounter = value;
    
            if (elfCounter < 3) ElfCounter0?.Invoke();
            else if (elfCounter >= 3 && elfCounter < 6) ElfCounter3.Invoke();
            else if (elfCounter >= 6 && elfCounter < 9) ElfCounter6.Invoke();
            else if (elfCounter >= 9) ElfCounter9.Invoke();
        }
    }
    public int WarriorCounter
    {
        get { return warriorCounter; }
        set 
        { 
            warriorCounter = value;

            if (warriorCounter < 3) WarriorCounter0?.Invoke();
            else if (warriorCounter >= 3 && warriorCounter < 6) WarriorCounter3.Invoke();
            else if (warriorCounter >= 6 && warriorCounter < 9) WarriorCounter6.Invoke();
            else if (warriorCounter >= 9) WarriorCounter9.Invoke();
        }
    }
    public int ArcherCounter
    {
        get { return archerCounter; }
        set 
        { 
            archerCounter = value;
                
            if (archerCounter < 3) ArcherCounter0?.Invoke();
            else if (archerCounter >= 3 && archerCounter < 6) ArcherCounter3.Invoke();
            else if (archerCounter >= 6 && archerCounter < 9) ArcherCounter6.Invoke();
            else if (archerCounter >= 9) ArcherCounter9.Invoke();
        }
    }
    public int MageCounter
    {
        get { return mageCounter; }
        set 
        { 
            mageCounter = value;
                
            if (mageCounter < 3) MageCounter0?.Invoke();
            else if (mageCounter >= 3 && mageCounter < 6) MageCounter3.Invoke();
            else if (mageCounter >= 6 && mageCounter < 9) MageCounter6.Invoke();
            else if (mageCounter >= 9) MageCounter9.Invoke();
        }
    }
#endregion
    
    #region Constructors
    public SynergyController()
    {
        //races
        HumanCounter = 0;
        OrcCounter = 0;
        ElfCounter = 0;

        //classes
        WarriorCounter = 0;
        ArcherCounter = 0;
        MageCounter = 0;
    }
    #endregion
    
    private void Awake()
    {
        if (singleton != null && singleton != this)
            Destroy(this.gameObject);
        else
            singleton = this;

    }

    public void ResetCounters()
    {        
        //races
        HumanCounter = 0;
        OrcCounter = 0;
        ElfCounter = 0;

        //classes
        WarriorCounter = 0;
        ArcherCounter = 0;
        MageCounter = 0;
    }
    public void PrintSynergies()
    {
        MonoBehaviour.print("Humans - " + HumanCounter);
        MonoBehaviour.print("Orcs - " + OrcCounter);
        MonoBehaviour.print("Elfs - " + ElfCounter);

        MonoBehaviour.print("Warriors - " + WarriorCounter);
        MonoBehaviour.print("Archers - " + ArcherCounter);
        MonoBehaviour.print("Mages - " + MageCounter);
    }
    public void AddIntoSyngeries(Champion champ)
    {
        Race tempRace = champ._Race;
        Class tempClass = champ._Class;

        //finding the race and class of the given champ
        if (tempRace == Race.Human)
            HumanCounter++;
        if (tempRace == Race.Orc)
            OrcCounter++;
        if (tempRace == Race.Elf)
            ElfCounter++;

        if (tempClass == Class.Warrior)
            WarriorCounter++;
        if (tempClass == Class.Archer)
            ArcherCounter++;
        if (tempClass == Class.Mage)
            MageCounter++;
        // ...
        print("Champion " + champ + " added to synergies succesfully !!!");

        TacticsMove.singleton.UpdateSynergiesTextVisuals();
    }
    public void RemoveFromSynergies(Champion champ)
    {
        Race tempRace = champ._Race;
        Class tempClass = champ._Class;

        //finding the race and class of the given champ
        if (tempRace == Race.Human)
            HumanCounter--;
        if (tempRace == Race.Orc)
            OrcCounter--;
        if (tempRace == Race.Elf)
            ElfCounter--;

        if (tempClass == Class.Warrior)
            WarriorCounter--;
        if (tempClass == Class.Archer)
            ArcherCounter--;
        if (tempClass == Class.Mage)
            MageCounter--;
        // ...
        print("Champion " + champ.name + " removed from synergies succesfully !!!");

        TacticsMove.singleton.UpdateSynergiesTextVisuals();
    }
}