using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class WriteMaster {

    public static List<T> JsonToList<T> (string line) {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (line);
        List<T> things = new List<T> (wrapper.Items);
        return things;
    }

    /*   public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }
    */

    public static string ListToJson<T> (List<T> list, bool prettyPrint = true) {
        Wrapper<T> wrapper = new Wrapper<T> ();
        wrapper.Items = list.ToArray ();
        return JsonUtility.ToJson (wrapper, prettyPrint);
    }

    /*   public static string JsonToList<T>(string line)
        {

            Wrapper<T> wrapper = new Wrapper<T>();
            JsonUtility.FromJson<T>(line);
            wrapper.Items = array.ToArray();
            Debug.Log(wrapper.Items.Length);
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }*/

    public static void WriteUp (string file, string text, bool cleanEmpty = false) {

        try {

            TextAsset asset = Resources.Load (file + ".txt") as TextAsset;
            StreamWriter writer = new StreamWriter ("Assets/Resources/Texts/" + file + ".txt"); // Does this work?

            if (cleanEmpty) {
                string[] ss = Utility.ChewUp (text, "\n");
                string s = "";
                for (int i = 0; i < ss.Length; i++) {
                    if (!ss[i].Contains (": \"\",")) s += ss[i] + "\n"; // = "";
                }
                writer.WriteLine (s);
            } else {
                writer.WriteLine (text);
            }

            writer.Close ();

        } catch (IOException ioe) {
            UberDebug.LogChannel ("Exception", "IOException on " + file);
        }

    }

    public static void WriteUp (string file, int[, ] map) {
        string text = "";

        for (int j = 0; j < map.GetLength (1); j++) {
            for (int i = map.GetLength (0) - 1; i >= 0; i--) {
                string s;
                //if(map[j, i] < 0){
                //	s = "XXX";
                //}else{
                s = map[i, j].ToString ("0.0");
                if (s.Equals ("0.0")) s = "ZZZ";
                //}
                text += s;
                text += " ";
            }
            text += "\n";
        }

        TextAsset asset = Resources.Load (file + ".txt") as TextAsset;
        StreamWriter writer = new StreamWriter ("Assets/Resources/Texts/" + file + ".txt"); // Does this work?
        writer.WriteLine (text);
        writer.Close ();
    }

    [Serializable]
    private class Wrapper<T> {
        public T[] Items;
    }
}