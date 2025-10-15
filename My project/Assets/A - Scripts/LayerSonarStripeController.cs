using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class LayerSonarStripeController : MonoBehaviour
{
    [Header("Sonar Orb Layer Settings")]
    public LayerMask sonarLayer;
    public float maxDetectDistance = 100f;

    private Renderer rend;
    private Material mat;

    void Start()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
    }

    void Update()
    {
        // Find all colliders on the SonarOrb layer near this object
        Collider[] colliders = Physics.OverlapSphere(transform.position, maxDetectDistance, sonarLayer);

        if (colliders.Length == 0)
        {
            mat.SetFloat("_OrbRadius", 0);
            return;
        }

        // Get closest sphere collider
        SphereCollider closest = null;
        float minDist = float.MaxValue;

        foreach (Collider c in colliders)
        {
            if (c is SphereCollider sc)
            {
                float dist = Vector3.Distance(transform.position, sc.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = sc;
                }
            }
        }

        if (closest != null)
        {
            Vector3 center = closest.transform.position;
            float radius = closest.radius * closest.transform.lossyScale.x;

            mat.SetVector("_OrbCenter", center);
            mat.SetFloat("_OrbRadius", radius);
        }
    }
}