using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ImageDownloader
{
    public static async Task<Texture2D> DownloadImageAsync(string imageUrl)
    {
        await RandomDelay.Wait();
        var request = UnityWebRequestTexture.GetTexture(imageUrl);
        await request.SendWebRequest();
        return DownloadHandlerTexture.GetContent(request);
    }
}