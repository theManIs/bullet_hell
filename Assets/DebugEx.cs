using System;
using System.Collections.Generic;
using UnityEngine;

public class DebugEx : Debug
{
    public static void LogList<T>(List<T> list)
    {
        string debugString = "List debug \n";
        list.ForEach(v2 => debugString += v2 + "\n");
        Log(debugString);
    }
}
