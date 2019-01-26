using System;
using System.Reflection;

public static class RACE {

	public const int NONE = 0,
		AQUA = 1, //
		BEAST = 2,
		BIRD = 3,
		BUG = 4,
		DEMON = 5,
		DRAGON = 6,
		FAIRY = 7,
		FISH = 8,
		HUMANOID = 9,
		ICE = 10, //
		MACHINE = 11,
		PLANT = 12,
		PLASMA = 13, //
		ROCK = 14,
		SPIRIT = 15,
		STEEL = 16, //
		UNDEAD = 17;

	public static int GetRaceValue (string PropertyName) {
		string UppercasePropertyName = PropertyName.ToUpper ();
		FieldInfo[] fields = typeof (RACE).GetFields (BindingFlags.Static | BindingFlags.Public);

		foreach (FieldInfo f in fields) {
			if (f.Name.Equals (UppercasePropertyName)) {
				return (int) f.GetValue (null);
			}
		}

		throw new EnumNotFoundException ("Unknown name: " + UppercasePropertyName);
	}

	public static string GetRaceName (int Value) {
		FieldInfo[] fields = typeof (RACE).GetFields (BindingFlags.Static | BindingFlags.Public);

		foreach (FieldInfo f in fields) {
			if (((int) f.GetValue (null)) == Value) {
				return f.Name.ToUpper ();
			}
		}

		throw new EnumNotFoundException ("Unknown value: " + Value);
	}

}