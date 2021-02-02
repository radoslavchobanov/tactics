using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SynergyController : MonoBehaviour
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

    // constructors
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


    // getters and setters
    public int HumanCounter
    {
        get { return humanCounter; }
        set 
        { 
            humanCounter = value;
            if (humanCounter == 3) ; // invoke event to give human buff 3, and 6, etc.

        }
    }
    public int OrcCounter
    {
        get { return orcCounter; }
        set { orcCounter = value; }
    }
    public int ElfCounter
    {
        get { return elfCounter; }
        set { elfCounter = value; }
    }
    public int WarriorCounter
    {
        get { return warriorCounter; }
        set { warriorCounter = value; }
    }
    public int ArcherCounter
    {
        get { return archerCounter; }
        set { archerCounter = value; }
    }
    public int MageCounter
    {
        get { return mageCounter; }
        set { mageCounter = value; }
    }

    private void Awake()
    {
        if (singleton != null && singleton != this)
            Destroy(this.gameObject);
        else
            singleton = this;
    }

    private void Start()
    {
        TacticsMove.singleton.ChampionEnteredBattlefield.AddListener(() => UpdateSynergies());
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
    public void UpdateSynergies()
    {
        print("Update synergies");

        ResetCounters();

        foreach (GameObject champObj in TacticsMove.singleton.champions)
        {
            Champion champ = champObj.GetComponent<Champion>();
            AddIntoSyngeries(champ);
        }

        TacticsMove.singleton.SynergiesUpdated.Invoke();
    }
    public void AddIntoSyngeries(Champion champ)
    {
        Race tempRace = champ._race;
        Class tempClass = champ._class;

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
    }
    public void RemoveFromSynergies(Champion champ)
    {
        Race tempRace = champ._race;
        Class tempClass = champ._class;

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
    }
}   