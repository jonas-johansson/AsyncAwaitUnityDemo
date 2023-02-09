using UnityEngine;

public class InteractionBusyIndicatorController : MonoBehaviour
{
    void Start()
    {
        InteractionSystem.InteractionAllowedChanged += InteractionSystem_InteractionAllowedChanged;
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        InteractionSystem.InteractionAllowedChanged -= InteractionSystem_InteractionAllowedChanged;
    }

    void InteractionSystem_InteractionAllowedChanged(bool isInteractionAllowed)
    {
        gameObject.SetActive(!isInteractionAllowed);
    }

    void Update()
    {
        transform.Rotate(0, 0, -720 * Time.deltaTime);
    }
}
