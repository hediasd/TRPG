using System;
using System.Reflection;

public static class TARGETS {

	public const int NONE = 100,
		SELF = 101,
		ALLIES = 102,
		ENEMIES = 103,
		BOTH = 104,
		ALL = 105;

	public static int GetTargetsValue (string PropertyName) {
		string UppercasePropertyName = PropertyName.ToUpper ();
		FieldInfo[] fields = typeof (TARGETS).GetFields (BindingFlags.Static | BindingFlags.Public);

		foreach (FieldInfo f in fields) {
			if (f.Name.Equals (UppercasePropertyName)) {
				return (int) f.GetValue (null);
			}
		}

		throw new EnumNotFoundException ("Unknown name: " + UppercasePropertyName);
	}

	public static string GetTargetsName (int Value) {
		FieldInfo[] fields = typeof (TARGETS).GetFields (BindingFlags.Static | BindingFlags.Public);

		foreach (FieldInfo f in fields) {
			if (((int) f.GetValue (null)) == Value) {
				return f.Name.ToUpper ();
			}
		}

		throw new EnumNotFoundException ("Unknown value: " + Value);
	}

}