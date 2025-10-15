using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(LineRenderer))]
public class Wire : MonoBehaviour
{
    private XRSimpleInteractable grabInteractable;
    private LineRenderer lineRenderer;
    private Transform handTransform;

    void Awake()
    {
        grabInteractable = GetComponent<XRSimpleInteractable>();
        lineRenderer = GetComponent<LineRenderer>();

        // Basic line setup
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        handTransform = args.interactorObject.transform;
        lineRenderer.enabled = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        handTransform = null;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (handTransform && lineRenderer.enabled)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, handTransform.position);
        }
    }
}
