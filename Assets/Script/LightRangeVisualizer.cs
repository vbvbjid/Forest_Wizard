using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightRangeVisualizer : MonoBehaviour
{
    private Light lightSource;

    void Start()
    {
        lightSource = GetComponent<Light>();
    }

    void OnDrawGizmos()
    {
        if (lightSource != null)
        {
            // Set the color of the gizmo to match the light color
            Gizmos.color = lightSource.color;
            Gizmos.DrawWireSphere(transform.position, lightSource.range); // Draw a wire sphere for point lights

            // Optionally, you can add more visualizations for other types of lights
            if (lightSource.type == LightType.Spot)
            {
                // Draw a cone for spot lights
                DrawSpotLightGizmo(lightSource.range, lightSource.spotAngle);
            }
        }
    }

    void DrawSpotLightGizmo(float range, float angle)
    {
        // Calculate the cone height and base radius
        float height = range;
        float radius = range * Mathf.Tan(angle * 0.5f * Mathf.Deg2Rad);

        // Draw the cone
        Gizmos.color = lightSource.color;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * height);
        Gizmos.DrawWireSphere(transform.position + transform.forward * height, radius);
    }
}