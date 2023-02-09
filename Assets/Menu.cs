using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

public class Menu : MonoBehaviour
{
    [SerializeField] float zoom = 1.5f;
    [SerializeField] GameObject spinningCube;

    CancellationTokenSource cancellationTokenSource;

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.Scale(new Vector3(zoom, zoom, zoom));

        if (GUILayout.Button("Hello world"))
        {
            Example_HelloWorldAsync();
        }

        EditorGUILayout.Separator();

        if (GUILayout.Button("Toggle spinning cube"))
        {
            spinningCube.SetActive(!spinningCube.activeSelf);
        }

        EditorGUILayout.Separator();

        if (GUILayout.Button("Prepare scene"))
        {
            Example_PrepareScene();
        }

        if (GUILayout.Button("Prepare scene async"))
        {
            Example_PrepareSceneAsync();
        }

        if (GUILayout.Button("Prepare scene async in parallel"))
        {
            Example_PrepareSceneAsyncParallel();
        }

        EditorGUILayout.Separator();

        if (GUILayout.Button("Avoid async void"))
        {
            Example_AvoidAsyncVoid();
        }

        if (GUILayout.Button("Warning when not awaiting task"))
        {
            Example_WarningWhenNotAwaitingTask();
        }

        if (GUILayout.Button("Task.Run"))
        {
            Example_TaskRun();
        }

        if (GUILayout.Button("ContinueWith"))
        {
            Example_ContinueWith();
        }

        if (GUILayout.Button("ContinueWith + SynchronizationContext"))
        {
            Example_ContinueWithTaskSchedulerFromSynchronizationContext();
        }

        EditorGUILayout.Separator();

        var messageLoopRunning = cancellationTokenSource is { IsCancellationRequested: false };
        if (!messageLoopRunning)
        {
            if (GUILayout.Button("CancellationToken - start message loop"))
            {
                Example_CancellationToken();
            }
        }
        else
        {
            if (GUILayout.Button("CancellationToken - cancel message loop"))
            {
                Cancel();
            }
        }

        EditorGUILayout.Separator();

        if (GUILayout.Button("Overhead Task"))
        {
            Example_OverheadTask();
        }

        if (GUILayout.Button("Overhead ValueTask"))
        {
            Example_OverheadValueTask();
        }

        if (GUILayout.Button("Overhead JobSystem"))
        {
            Example_OverheadJobSystem();
        }

        EditorGUILayout.Separator();

        if (GUILayout.Button("Grandpas old system with callbacks"))
        {
            Example_GrandpasOldSystemWithCallbacks();
        }

        if (GUILayout.Button("Modern facade on grandpas old system"))
        {
            Example_ModernFacadeOnGrandpasOldSystem();
        }
    }














    /// <summary>
    /// A simple example of what asynchronous code looks like.
    /// </summary>
    async Task Example_HelloWorldAsync()
    {
        Debug.Log("Hello");
        await Task.Delay(TimeSpan.FromSeconds(1));
        Debug.Log("World");
    }














    void Example_PrepareScene()
    {
        var sw = Stopwatch.StartNew();

        var charA = LoadCharacter("A");
        var charB = LoadCharacter("B");
        var charC = LoadCharacter("C");

        var color = FetchColorFromBackend();

        charA.SetColor(color);
        charB.SetColor(color);
        charC.SetColor(color);

        Debug.Log($"It took {sw.ElapsedMilliseconds} ms to prepare the scene.");
    }
















    async Task Example_PrepareSceneAsync()
    {
        var sw = Stopwatch.StartNew();

        var charA = await LoadCharacterAsync("A");
        var charB = await LoadCharacterAsync("B");
        var charC = await LoadCharacterAsync("C");

        var color = await FetchColorFromBackendAsync();

        charA.SetColor(color);
        charB.SetColor(color);
        charC.SetColor(color);

        Debug.Log($"It took {sw.ElapsedMilliseconds} ms to prepare the scene asynchronusly.");
    }
















    async Task Example_PrepareSceneAsyncParallel()
    {
        var sw = Stopwatch.StartNew();

        // Start all the tasks before awaiting them.
        var charTaskA = LoadCharacterAsync("A");
        var charTaskB = LoadCharacterAsync("B");
        var charTaskC = LoadCharacterAsync("C");
        var colorTask = FetchColorFromBackendAsync();

        // Then await them.
        var a = await charTaskA;
        var b = await charTaskB;
        var c = await charTaskC;
        var color = await colorTask;

        a.SetColor(color);
        b.SetColor(color);
        c.SetColor(color);

        Debug.Log($"It took {sw.ElapsedMilliseconds} ms to prepare the scene async in parallel.");
    }


























    /// <summary>
    /// In this example, `AsyncVoidThatThrows` is an async void method that throws an exception.
    /// That exception will NOT be caught by the try/catch block, so you won't see "Caught it".
    /// When an exception is thrown out of an async Task or async Task T method, that exception
    /// is captured and placed on the Task object. With async void methods, there is no Task
    /// object, so any exceptions thrown out of an async void method will be raised directly
    /// on the SynchronizationContext that was active when the async void method started.
    /// The exceptions are, however, picked up and logged by Unity by default.
    /// Async void methods are also difficult to test, as you can't await them easily.
    /// Only ever use async void methods when you are absolutely certain that you want to,
    /// for example, in event handlers. But even then, I'd argue that there are better ways.
    /// </summary>
    void Example_AvoidAsyncVoid()
    {
        try
        {
            AsyncVoidThatThrows();
        }
        catch (Exception)
        {
            Debug.Log("If you see this, we caught it (but we won't).");
        }
    }










    async void AsyncVoidThatThrows()
    {
        throw new Exception("Boom");
    }
















    /// <summary>
    /// Because this call is not awaited, execution of the current method continues before
    /// the call is completed. Consider applying the 'await' operator to the result of the call.
    /// </summary>
    void Example_WarningWhenNotAwaitingTask()
    {
        LogAsync("Nice weather today");
    }
















    async Task LogAsync(string message)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        Debug.Log($"Delayed message: {message}");
    }
















    /// <summary>
    /// Task.Run queues the specified work to run on the ThreadPool and returns a Task for that work.
    /// </summary>
    void Example_TaskRun()
    {
        // This will run on Unity's main thread.
        ThreadIdLogger.Log("Before Task.Run");

        Task.Run(() =>
        {
            // But this will run on a different thread.
            // You can NOT access Unity from here.
            ThreadIdLogger.Log("Inside Task.Run");
        });
    }
















    /// <summary>
    /// ContinueWith creates a continuation that executes asynchronously when the target Task completes.
    /// The returned Task will not be scheduled for execution until the current task has completed,
    /// whether it completes due to running to completion successfully, faulting due to an unhandled
    /// exception, or exiting early due to cancellation.
    /// </summary>
    void Example_ContinueWith()
    {
        // This will run on Unity's main thread.
        ThreadIdLogger.Log("Before");

        DoSomethingAsync().ContinueWith(task =>
        {
            // But this will run on a different thread.
            // You can NOT access Unity from here.
            ThreadIdLogger.Log("After ContinueWith");
        });
    }













    void Example_ContinueWithTaskSchedulerFromSynchronizationContext()
    {
        // This will run on Unity's main thread.
        ThreadIdLogger.Log("Before");

        DoSomethingAsync().ContinueWith(task =>
        {
            // But this will run on a different thread.
            // You CAN access Unity from here.
            ThreadIdLogger.Log("After ContinueWith");
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }


















    void Example_CancellationToken()
    {
        // The .NET Framework uses a unified model for cooperative cancellation of asynchronous
        // or long-running synchronous operations that involves two objects:
        // - A CancellationTokenSource object, which provides a cancellation token through its Token
        // property and sends a cancellation message by calling its Cancel or CancelAfter method.
        // - A CancellationToken object, which indicates whether cancellation is requested.

        // The general pattern for implementing the cooperative cancellation model is:
        // Instantiate a CancellationTokenSource object, which manages and sends cancellation
        // notification to the individual cancellation tokens.
        cancellationTokenSource = new CancellationTokenSource();

        // Pass the token returned by the CancellationTokenSource.Token property to each task or thread
        // that listens for cancellation.
        PrintMessagesUntilCanceledAsync(cancellationTokenSource.Token);

        // Continue reading...
    }

    async Task PrintMessagesUntilCanceledAsync(CancellationToken token)
    {
        // Call the CancellationToken.IsCancellationRequested method from operations that receive the
        // cancellation token. Provide a mechanism for each task or thread to respond to a cancellation
        // request. Whether you choose to cancel an operation, and exactly how you do it, depends on
        // your application logic.
        while (!token.IsCancellationRequested)
        {
            Debug.Log("Are we there yet?");
            await Task.Delay(TimeSpan.FromSeconds(1), token);
        }

        // Continue reading...
    }

    void Cancel()
    {
        // Call the CancellationTokenSource.Cancel method to provide notification of cancellation.
        // This sets the CancellationToken.IsCancellationRequested property on every copy of the
        // cancellation token to true.
        cancellationTokenSource.Cancel();

        // Call the Dispose method when you are finished with the CancellationTokenSource object.
        cancellationTokenSource.Dispose();
    }


























    async Task Example_OverheadTask()
    {
        var stopwatch = Stopwatch.StartNew();
        int total = 0;

        for (int i = 0; i < 1_000_000; i++)
        {
            int value = await GetValueUsingTaskAsync();
            total += value;
        }

        Debug.Log($"Task finished in {stopwatch.ElapsedMilliseconds} ms with value {total}.");
    }

    Task<int> GetValueUsingTaskAsync()
    {
        return Task.FromResult(1337);
    }
















    async Task Example_OverheadValueTask()
    {
        var stopwatch = Stopwatch.StartNew();
        int total = 0;

        for (int i = 0; i < 1_000_000; i++)
        {
            int value = await GetValueUsingValueTaskAsync();
            total += value;
        }

        Debug.Log($"ValueTask finished in {stopwatch.ElapsedMilliseconds} ms with value {total}.");
    }

    ValueTask<int> GetValueUsingValueTaskAsync()
    {
        return new ValueTask<int>(1337);
    }
























    void Example_OverheadJobSystem()
    {
        var stopwatch = Stopwatch.StartNew();
        int total = 0;

        for (int i = 0; i < 1_000_000; i++)
        {
            int value = GetValueUsingJobSystem();
            total += value;
        }

        Debug.Log($"JobSystem finished in {stopwatch.ElapsedMilliseconds} ms with value {total}.");
    }

    struct SomeJob : IJob
    {
        public NativeArray<int> array;

        public void Execute()
        {
            array[0] = 1337;
        }
    }

    int GetValueUsingJobSystem()
    {
        // Yes, this is terrible code, and not how the job system should be used,
        // but it shows that even the job system can be misused.
        var job = new SomeJob();
        job.array = new NativeArray<int>(1, Allocator.TempJob);
        var handle = job.Schedule();
        handle.Complete();
        int value = job.array[0];
        job.array.Dispose();
        return value;
    }


















    async Task Example_GrandpasOldSystemWithCallbacks()
    {
        GrandpasOldSystem.FetchMessage(
            successCallback: (message) =>
            {
                GrandpasOldSystem.FetchColor(
                    successCallback: (color) =>
                    {
                        Debug.Log($"Grandpa succeeded with color {color} and message {message}");
                    },
                    errorCallback: (errorCode) =>
                    {
                        Debug.LogError($"Grandpa color error: {errorCode}");
                    }
                );
            },
            errorCallback: (errorCode) =>
            {
                Debug.LogError($"Grandpa message error: {errorCode}");
            }
        );
    }
















    async Task Example_ModernFacadeOnGrandpasOldSystem()
    {
        try
        {
            string message = await ModernFacade.FetchMessageAsync();
            string color = await ModernFacade.FetchColorAsync();
            Debug.Log($"Modern facade succeeded with color {color} and message {message}");
        }
        catch (GrandpaException e)
        {
            Debug.LogException(e);
        }
    }



















    /// <summary>
    /// Fetch color from backend, takes 1 second.
    /// </summary>
    Color FetchColorFromBackend()
    {
        Thread.Sleep(1000);
        return Color.green;
    }

    /// <summary>
    /// Load a character, takes 1 second.
    /// </summary>
    Character LoadCharacter(string type)
    {
        Thread.Sleep(1000);
        return new Character(type);
    }

    async Task<Color> FetchColorFromBackendAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return Color.green;
    }

    async Task<Character> LoadCharacterAsync(string type)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return new Character(type);
    }








    async Task DoSomethingAsync()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(100));
    }



}