using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterInstance {

    public MonsterEntry MonsterEntry;

    public int ID;
    public string Name, Texture;
    public Color PaletteA, PaletteB;
    public List<int> Races;

    public MonsterStats Stats;

    public List<SpellInstance> SpellsList, SpellsCast; //= new List<Spell>();
    public List<Status> StatusesList; // = new List<Status>();

    public Point MonsterPoint;
    public bool Alive;
    public int Team, AvailableMovementPoints,
    lastDamage, turnDamage, lastTurnDamage, totalDamageTaken;
    public string lastSpellCast, lastElement;

    public MonsterInstance (MonsterEntry ThisEntry) {

        this.MonsterEntry = ThisEntry;
        this.Name = ThisEntry.Name;
        this.Texture = ThisEntry.Texture;

        Color EntryPaletteA = ThisEntry.ColorPaletteA;
        Color EntryPaletteB = ThisEntry.ColorPaletteB;

        PaletteA = new Color (EntryPaletteA.r, EntryPaletteA.g, EntryPaletteA.b, EntryPaletteA.a);
        PaletteB = new Color (EntryPaletteB.r, EntryPaletteB.g, EntryPaletteB.b, EntryPaletteB.a);

        Races = new List<int> ();
        SpellsList = new List<SpellInstance> ();
        SpellsCast = new List<SpellInstance> ();

        foreach (int EntryRace in ThisEntry.RacesList) {
            Races.Add (EntryRace);
        }

        foreach (SpellEntry SE in ThisEntry.SpellEntries) {
            SpellInstance SI = SE.Instantiate ();
            SpellsList.Add (SI);
        }

        Stats = ThisEntry.StatsList.Copy ();
        //Stats.Increase (STAT.MOV, 3);
        AvailableMovementPoints = MovementPoints ();

    }

    public void AddSpell (SpellInstance sp) {
        SpellsList.Add (sp);
        //SpellNames.Add(sp.name);
    }
    //public void AddStatus (StatusEntry st) {
    //    StatusesList.Add (st);
    //    StatusNames.Add (st.Name);
    //}

    public bool Die () {
        Alive = false;
        return (true);
    }

    public int MovementPoints () {
        return Stats[STAT.MOV];
    }
    public void ResetMovementPoints () {
        Stats.ResetValue (STAT.MOV);
    }
    public bool TakeDamage (Damage TakenDamage) {
        Stats.Decrease (STAT.HPA, TakenDamage.FinalDamage);
        return true;

    }

}