using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public static class WriteMaster
{

    public static List<T> JsonToList<T>(string line)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(line);
        List<T> things = new List<T>(wrapper.Items);
        return things;
    }

 /*   public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
*/
	public static string ListToJson<T>(List<T> list, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = list.ToArray();
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

/*   public static string JsonToList<T>(string line)
    {

        Wrapper<T> wrapper = new Wrapper<T>();
        JsonUtility.FromJson<T>(line);
        wrapper.Items = array.ToArray();
        Debug.Log(wrapper.Items.Length);
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }*/

    public static void WriteUp(string file, string text)
    {
        TextAsset asset = Resources.Load(file + ".txt") as TextAsset;
        StreamWriter writer = new StreamWriter("Assets/Resources/Texts/" + file + ".txt"); // Does this work?
        writer.WriteLine(text);
        writer.Close();
    }
       
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
