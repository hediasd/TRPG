using System;
using System.Reflection;

public static class ELEMENT {

    public const int NONE = 0,
        DARKNESS = 1,
        EARTH = 2,
        FIRE = 3,
        ICE = 4,
        LIGHT = 5,
        STEEL = 6,
        THUNDER = 7,
        WATER = 8,
        WIND = 9,
        WOOD = 10;

    public static int GetElementValue (string PropertyName) {
        string UppercasePropertyName = PropertyName.ToUpper ();
        FieldInfo[] fields = typeof (ELEMENT).GetFields (BindingFlags.Static | BindingFlags.Public);

        foreach (FieldInfo f in fields) {
            if (f.Name.Equals (UppercasePropertyName)) {
                return (int) f.GetValue (null);
            }
        }

        throw new EnumNotFoundException ("Unknown name: " + UppercasePropertyName);
    }

}