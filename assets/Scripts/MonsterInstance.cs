using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInstance {

    public MonsterEntry MonsterEntry;

    public int ID;
    public string Name, Texture;
    public Color PaletteA, PaletteB;

    public Stats StatsList;

    public Point MonsterPoint;
    public bool Alive;
    public int Team, AvailableMovementPoints,
    lastDamage, turnDamage, lastTurnDamage, totalDamageTaken;
    public string lastSpellCast, lastElement;

    public List<SpellEntry> SpellsCast; //= new List<Spell>();
    public List<Status> StatusesList; // = new List<Status>();

    public MonsterInstance (MonsterEntry MonsterEntry) {

        this.MonsterEntry = MonsterEntry;

        //mon.SpellsList = new List<SpellEntry> (SpellsList);
        //mon.SpellNames = new List<string>(SpellNames);
        //mon.StatusNames = new List<string> (StatusNames);
        //mon.SpellsCast = new List<SpellEntry> ();

        Color EntryPaletteA = MonsterEntry.PaletteA_;
        Color EntryPaletteB = MonsterEntry.PaletteB_;

        PaletteA = new Color (EntryPaletteA.r, EntryPaletteA.g, EntryPaletteA.b, EntryPaletteA.a);
        PaletteB = new Color (EntryPaletteB.r, EntryPaletteB.g, EntryPaletteB.b, EntryPaletteB.a);

        StatsList = MonsterEntry.StatsList.Copy ();
        //Array.Copy(STATS_, mon.STATS_, 9);

        StatsList.Increase (STAT.MOV, 3);
        AvailableMovementPoints = MovementPoints ();

    }

    public void AddSpell (SpellEntry sp) {
        SpellsList.Add (sp);
        //SpellNames.Add(sp.name);
    }
    public void AddStatus (StatusEntry st) {
        StatusesList.Add (st);
        StatusNames.Add (st.Name);
    }

    public bool Die () {
        Alive = false;
        return true;
    }

    public int MovementPoints () {
        return StatsList[STAT.MOV];
    }
    public void ResetMovementPoints () {
        StatsList.ResetValue (STAT.MOV);
    }
    public bool TakeDamage (Damage TakenDamage) {
        StatsList.Decrease (STAT.HPA, TakenDamage.FinalDamage);
        return true;

    }

}