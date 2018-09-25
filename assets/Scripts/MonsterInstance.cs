using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterInstance {

    public MonsterEntry MonsterEntry;

    public int ID;
    public string Name, Texture;
    public Color PaletteA, PaletteB;

    public MonsterStats Stats;

    public List<SpellInstance> SpellsList, SpellsCast; //= new List<Spell>();
    public List<Status> StatusesList; // = new List<Status>();

    public Point MonsterPoint;
    public bool Alive;
    public int Team, AvailableMovementPoints,
    lastDamage, turnDamage, lastTurnDamage, totalDamageTaken;
    public string lastSpellCast, lastElement;

    public MonsterInstance (MonsterEntry Entry) {

        this.MonsterEntry = Entry;

        this.Name = Entry.Name;
        this.Texture = Entry.Texture;

        Color EntryPaletteA = Entry.ColorPaletteA;
        Color EntryPaletteB = Entry.ColorPaletteB;

        PaletteA = new Color (EntryPaletteA.r, EntryPaletteA.g, EntryPaletteA.b, EntryPaletteA.a);
        PaletteB = new Color (EntryPaletteB.r, EntryPaletteB.g, EntryPaletteB.b, EntryPaletteB.a);

        SpellsList = new List<SpellInstance> ();
        SpellsCast = new List<SpellInstance> ();

        foreach (SpellEntry SE in Entry.SpellEntries) {
            SpellInstance SI = SE.Instantiate ();
            SpellsList.Add (SI);
        }

        Stats = Entry.StatsList.Copy ();
        Stats.Increase (STAT.MOV, 3);
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
        return true;
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