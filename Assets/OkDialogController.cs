using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OkDialogController : MonoBehaviour
{
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text message;
    [SerializeField] Button okButton;

    TaskCompletionSource<bool> closed = new TaskCompletionSource<bool>();

    public Task Closed => closed.Task;

    public void Init(string title, string message)
    {
        this.title.text = title;
        this.message.text = message;

        // Do something like this if we want to deal with interaction here.
        //okButton.SetHandler(() => Close());

        // But for now we use the Button component's onClick event directly.
        okButton.onClick.AddListener(Close);
    }

    void Close()
    {
        closed.SetResult(true);
        Destroy(gameObject);
    }
}