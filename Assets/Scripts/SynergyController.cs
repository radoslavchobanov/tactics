using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SynergyController : SynergyBuffController
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
    public UnityEvent HumanCounter3;
    public UnityEvent HumanCounter6;
    public UnityEvent HumanCounter9;
        //----------------------------
        //Orc events------------------
    public UnityEvent OrcCounter3;
    public UnityEvent OrcCounter6;
    public UnityEvent OrcCounter9;
        //----------------------------
        //Elf events------------------
    public UnityEvent ElfCounter3;
    public UnityEvent ElfCounter6;
    public UnityEvent ElfCounter9;
        //----------------------------
        //Warrior events--------------
    public UnityEvent WarriorCounter3;
    public UnityEvent WarriorCounter6;
    public UnityEvent WarriorCounter9;
        //----------------------------
        //Archer events---------------
    public UnityEvent ArcherCounter3;
    public UnityEvent ArcherCounter6;
    public UnityEvent ArcherCounter9;
        //----------------------------
        //Mage events-----------------
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
            switch (humanCounter)
            {
                case 3: HumanCounter3.Invoke(); break;
                case 6: HumanCounter6.Invoke(); break;
                case 9: HumanCounter9.Invoke(); break;
            }
        }
    }
    public int OrcCounter
    {
        get { return orcCounter; }
        set 
        { 
            orcCounter = value;
            switch (orcCounter)
            {
                case 3: OrcCounter3.Invoke(); break;
                case 6: OrcCounter6.Invoke(); break;
                case 9: OrcCounter9.Invoke(); break;
            }
        }
    }
    public int ElfCounter
    {
        get { return elfCounter; }
        set 
        { 
            elfCounter = value;
            switch (elfCounter)
            {
                case 3: ElfCounter3.Invoke(); break;
                case 6: ElfCounter6.Invoke(); break;
                case 9: ElfCounter9.Invoke(); break;
            }
        }
    }
    public int WarriorCounter
    {
        get { return warriorCounter; }
        set 
        { 
            warriorCounter = value;
            switch (warriorCounter)
            {
                case 3: WarriorCounter3.Invoke(); break;
                case 6: WarriorCounter6.Invoke(); break;
                case 9: WarriorCounter9.Invoke(); break;
            }
        }
    }
    public int ArcherCounter
    {
        get { return archerCounter; }
        set 
        { 
            archerCounter = value;
            switch (archerCounter)
            {
                case 3: ArcherCounter3.Invoke(); break;
                case 6: ArcherCounter6.Invoke(); break;
                case 9: ArcherCounter9.Invoke(); break;
            }
        }
    }
    public int MageCounter
    {
        get { return mageCounter; }
        set 
        { 
            mageCounter = value;
            switch (mageCounter)
            {
                case 3: MageCounter3.Invoke(); break;
                case 6: MageCounter6.Invoke(); break;
                case 9: MageCounter9.Invoke(); break;
            }
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