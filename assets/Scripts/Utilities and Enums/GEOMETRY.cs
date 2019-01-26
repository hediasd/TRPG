using System;
using System.Reflection;

public static class GEOMETRY {

	public const int NONE = 0,
		SQUARE = 110,
		CONE = 111,
		CIRCLE = 112,
		LINE = 113,
		TRILINE = 114,
		HORIZONTAL_LINE = 115,
		VERTICAL_LINE = 116,
		CROSS = 117;

    public static int GetGeometryValue (string PropertyName) {
        string UppercasePropertyName = PropertyName.ToUpper ();
        FieldInfo[] fields = typeof (GEOMETRY).GetFields (BindingFlags.Static | BindingFlags.Public);

        foreach (FieldInfo f in fields) {
            if (f.Name.Equals (UppercasePropertyName)) {
                return (int) f.GetValue (null);
            }
        }

        throw new EnumNotFoundException ("Unknown name: " + UppercasePropertyName);
    }

}