using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SynergyController
{
    // race counters
    private int humanCounter;
    private int orcCounter;
    private int elfCounter;

    // class counters
    private int warriorCounter;
    private int archerCounter;
    private int mageCounter;

    // buffs for synergies vars

        //human vars
    bool haveHumanBuff3 = false;
    bool haveHumanBuff6 = false;

        //orc vars
    bool haveOrcBuff3 = false;
    bool haveOrcBuff6 = false;

        //elf vars
    bool haveElfBuff3 = false;
    bool haveElfBuff6 = false;

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
        set { humanCounter = value; }
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

        UpdateBonuses();
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

        UpdateBonuses();
    }

    public void UpdateBonuses() // updating all the bonuses
    {
        //human bonuses
        if (humanCounter < 3 && haveHumanBuff3 == true)
        {
            RemoveHumanBuff3();
            haveHumanBuff3 = false;
        }
        else if (humanCounter >= 3 && humanCounter <= 5 && haveHumanBuff3 == false && haveHumanBuff6 == false)
        {
            GiveHumanBuff3();
            haveHumanBuff3 = true;
        }
        else if (humanCounter >= 3 && humanCounter <= 5 && haveHumanBuff3 == false && haveHumanBuff6 == true)
        {
            RemoveHumanBuff6();
            haveHumanBuff6 = false;
            GiveHumanBuff3();
            haveHumanBuff3 = true;
        }
        else if (humanCounter >= 6 && haveHumanBuff3 == true && haveHumanBuff6 == false)
        {
            RemoveHumanBuff3();
            haveHumanBuff3 = false;
            GiveHumanBuff6();
            haveHumanBuff6 = true;
        }

        //orc bonuses
        if (orcCounter < 3)
        {
            RemoveOrcBuff3();
            haveOrcBuff3 = false;
        }
        else if (orcCounter >= 3 && orcCounter <= 5 && haveOrcBuff3 == false && haveOrcBuff6 == false)
        {
            GiveOrcBuff3();
            haveOrcBuff3 = true;
        }
        else if (orcCounter >= 3 && orcCounter <= 5 && haveOrcBuff3 == false && haveOrcBuff6 == true)
        {
            RemoveOrcBuff6();
            haveOrcBuff6 = false;
            GiveOrcBuff3();
            haveOrcBuff3 = true;
        }
        else if (orcCounter >= 6 && haveOrcBuff3 == true && haveOrcBuff6 == false)
        {
            RemoveOrcBuff3();
            haveOrcBuff3 = false;
            GiveOrcBuff6();
            haveOrcBuff6 = true;
        }

        //elf bonuses
    }

    public void GiveHumanBuff3()
    {
        foreach (GameObject champObj in TacticsMove.singleton.champions)
        {
            Champion champ = champObj.GetComponent<Champion>();

            if (champ._race == Race.Human && champ.GetComponent<ChampionController>().GetCurrentTile().isHomeBattlefieldTile)
            {
                champ.buffs.Add(Buff.Human3);
            }
        }
    }
    public void RemoveHumanBuff3()
    {
        foreach (GameObject champObj in TacticsMove.singleton.champions)
        {
            Champion champ = champObj.GetComponent<Champion>();

            if (champ._race == Race.Human)
            {
                champ.buffs.Remove(Buff.Human3);
            }
        }
    }
    public void GiveHumanBuff6()
    {
        foreach (GameObject champObj in TacticsMove.singleton.champions)
        {
            Champion champ = champObj.GetComponent<Champion>();

            if (champ._race == Race.Human)
            {
                champ.buffs.Add(Buff.Human6);
            }
        }
    }
    public void RemoveHumanBuff6()
    {
        foreach (GameObject champObj in TacticsMove.singleton.champions)
        {
            Champion champ = champObj.GetComponent<Champion>();

            if (champ._race == Race.Human)
            {
                champ.buffs.Remove(Buff.Human6);
            }
        }
    }

    public void GiveOrcBuff3()
    {
        foreach (GameObject champObj in TacticsMove.singleton.champions)
        {
            Champion champ = champObj.GetComponent<Champion>();

            if (champ._race == Race.Orc)
            {
                champ.buffs.Add(Buff.Orc3);
            }
        }
    }
    public void RemoveOrcBuff3()
    {
        foreach (GameObject champObj in TacticsMove.singleton.champions)
        {
            Champion champ = champObj.GetComponent<Champion>();

            if (champ._race == Race.Orc)
            {
                champ.buffs.Remove(Buff.Orc3);
            }
        }
    }
    public void GiveOrcBuff6()
    {
        foreach (GameObject champObj in TacticsMove.singleton.champions)
        {
            Champion champ = champObj.GetComponent<Champion>();

            if (champ._race == Race.Orc)
            {
                champ.buffs.Add(Buff.Orc6);
            }
        }
    }
    public void RemoveOrcBuff6()
    {
        foreach (GameObject champObj in TacticsMove.singleton.champions)
        {
            Champion champ = champObj.GetComponent<Champion>();

            if (champ._race == Race.Orc)
            {
                champ.buffs.Remove(Buff.Orc6);
            }
        }
    }
}