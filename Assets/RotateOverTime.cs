using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    public Vector3 angularVelocity;

    void Update()
    {
        transform.Rotate(angularVelocity * Time.deltaTime);
    }
}
