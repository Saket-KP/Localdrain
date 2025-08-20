using TMPro.Examples;
using UnityEngine;

public class Location_implementation : MonoBehaviour
{
    public GameObject uiPanel;
    public Vector2 targetCoordinates = new Vector2(26.50979010726544f, 80.22604914319615f); // Replace with your target
    public float activationRadius = 10f; // Radius in meters

    private bool locationStarted = false;

    void Start()
    {
        uiPanel.SetActive(false);

        if (!Input.location.isEnabledByUser)
        {
            Debug.LogWarning("Location services not enabled by user.");
            return;
        }

        Input.location.Start();
        locationStarted = true;
    }

    void Update()
    {
        if (!locationStarted || Input.location.status != LocationServiceStatus.Running)
        {
            Debug.Log("Waiting for location...");
            return;
        }

        float currentLat = Input.location.lastData.latitude;
        float currentLon = Input.location.lastData.longitude;

        Vector2 currentLocation = new Vector2(currentLat, currentLon);

        float distance = HaversineDistance(currentLocation, targetCoordinates);

        uiPanel.SetActive(distance <= activationRadius);
    }

    float HaversineDistance(Vector2 pos1, Vector2 pos2)
    {
        float R = 6371e3f; // Earth radius in meters
        float lat1 = pos1.x * Mathf.Deg2Rad;
        float lat2 = pos2.x * Mathf.Deg2Rad;
        float deltaLat = (pos2.x - pos1.x) * Mathf.Deg2Rad;
        float deltaLon = (pos2.y - pos1.y) * Mathf.Deg2Rad;

        float a = Mathf.Sin(deltaLat / 2) * Mathf.Sin(deltaLat / 2) +
                  Mathf.Cos(lat1) * Mathf.Cos(lat2) *
                  Mathf.Sin(deltaLon / 2) * Mathf.Sin(deltaLon / 2);

        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

        return R * c;
    }
}
