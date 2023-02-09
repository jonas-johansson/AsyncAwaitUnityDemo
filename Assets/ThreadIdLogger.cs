using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class ThreadIdLogger
{
    public static void Log(string message = null, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        if (message != null)
        {
            Debug.Log($"Thread id {System.Threading.Thread.CurrentThread.ManagedThreadId} - {message} - {memberName} - {Path.GetFileName(filePath)}:{lineNumber}");
        }
        else
        {
            Debug.Log($"Thread id {System.Threading.Thread.CurrentThread.ManagedThreadId} - {memberName} - {Path.GetFileName(filePath)}:{lineNumber}");
        }
    }
}