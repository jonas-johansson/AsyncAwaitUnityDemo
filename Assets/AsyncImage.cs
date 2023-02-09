using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AsyncImage : MonoBehaviour
{
     RawImage Image => GetComponent<RawImage>();

     public string Url
     {
          set
          {
               var url = value;
               var taskSchedulerFromUnitySynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext();

               ImageDownloader.DownloadImageAsync(url).ContinueWith(downloadTask =>
               {
                    if (this == null)
                    {
                         // This component was destroyed before the image was downloaded.
                    }
                    else
                    {
                         StopSpinning();
                         Image.texture = downloadTask.Result;
                    }
               }, taskSchedulerFromUnitySynchronizationContext);
          }
     }

     void StopSpinning()
     {
          transform.localRotation = Quaternion.identity;
          enabled = false;
     }

     void Update()
     {
          transform.Rotate(0, 0, -720 * Time.deltaTime);
     }
}
