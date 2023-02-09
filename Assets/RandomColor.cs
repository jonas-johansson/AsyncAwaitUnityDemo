using System.Threading;
using UnityEngine;

public class RandomColor
{
    static int ColorIndex;
    static Color[] Colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan, Color.black };

    public static Color Next()
    {
        Interlocked.Increment(ref ColorIndex);
        return Colors[ColorIndex % Colors.Length];
    }
}