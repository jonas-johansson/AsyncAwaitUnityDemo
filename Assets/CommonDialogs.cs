using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CommonDialogs
{
    public static async Task ShowOkDialogAsync(string title, string message)
    {
        var canvas = FindCanvasInActiveScene();
        var okDialog = Object.Instantiate(Resources.Load<OkDialogController>("OkDialog"), canvas.transform);
        okDialog.Init(title, message);
        await okDialog.Closed;
    }

    static Canvas FindCanvasInActiveScene()
    {
        var activeScene = SceneManager.GetActiveScene();
        var rootGameObjects = activeScene.GetRootGameObjects();
        foreach (var rootGameObject in rootGameObjects)
        {
            var canvas = rootGameObject.GetComponent<Canvas>();
            if (canvas != null)
            {
                return canvas;
            }
        }
        throw new System.Exception("No canvas found in active scene.");
    }
}