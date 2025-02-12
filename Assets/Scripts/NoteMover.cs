using UnityEngine;

public class NoteMover : MonoBehaviour
{
    private Vector3 target;
    private float travelTime;
    private float spawnTime;

    public void Initialize(Vector3 targetPosition, float timeToReach)
    {
        target = targetPosition;
        travelTime = timeToReach;
        spawnTime = Time.time;
    }

    private void Update()
    {
        float elapsed = Time.time - spawnTime;
        float t = elapsed / travelTime;
        if (t > 1) t = 1;

        transform.position = Vector3.Lerp(transform.position, target, t);

        if (t >= 1)
        {
            Destroy(gameObject);
        }
    }
}
