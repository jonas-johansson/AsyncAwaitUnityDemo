using UnityEngine;

public class DemoMonoBehaviour : MonoBehaviour
{
    static readonly Vector3 AngularVelocity = new(30, 20, 50);
    protected bool IsSpinning { set; get; }

    protected void SetColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }

    void Update()
    {
        if (IsSpinning)
        {
            transform.Rotate(AngularVelocity * Time.deltaTime);
        }
    }
}