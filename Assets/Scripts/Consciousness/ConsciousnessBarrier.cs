using UnityEngine;

public class ConsciousnessBarrier : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToSpawn; // The object to spawn

    [SerializeField]
    private PolygonCollider2D polygonCollider; // The PolygonCollider2D to get the points from

    [SerializeField]
    private Transform player; // Reference to the player

    [SerializeField]
    private float minDistanceFromPlayer = 5.0f; // Minimum distance from the player

    private void Awake()
    {
        SpawnAtRandomPoint();
    }

    void SpawnAtRandomPoint()
    {
        if (polygonCollider != null)
        {
            SpawnInsidePolygon();
        }
        else
        {
            Debug.LogError("No collider assigned.");
        }
    }

    void SpawnInsidePolygon()
    {
        Vector2 randomPoint;
        Vector3 spawnPosition;

        do
        {
            randomPoint = GetRandomPointInPolygon();
            spawnPosition = transform.position + (Vector3)randomPoint;
        } while (Vector3.Distance(spawnPosition, player.position) < minDistanceFromPlayer);

        // Instantiate the object at the calculated position
        var doorObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        var door = doorObject.GetComponent<Door>();

        door.Destroyed.AddListener(OnDoorDestroyed);
    }

    Vector2 GetRandomPointInPolygon()
    {
        Vector2[] points = polygonCollider.points;
        Vector2 randomPoint;

        do
        {
            randomPoint = new Vector2(
                Random.Range(polygonCollider.bounds.min.x, polygonCollider.bounds.max.x),
                Random.Range(polygonCollider.bounds.min.y, polygonCollider.bounds.max.y)
            );
        } while (!IsPointInPolygon(randomPoint, points));

        return randomPoint;
    }

    bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
    {
        int polygonLength = polygon.Length, i = 0;
        bool inside = false;
        float pointX = point.x, pointY = point.y;
        float startX, startY, endX, endY;
        Vector2 endPoint = polygon[polygonLength - 1];
        endX = endPoint.x;
        endY = endPoint.y;
        while (i < polygonLength)
        {
            startX = endX; startY = endY;
            endPoint = polygon[i++];
            endX = endPoint.x; endY = endPoint.y;
            inside ^= (endY > pointY ^ startY > pointY) && ((pointX - startX) < (endX - startX) * (pointY - startY) / (endY - startY));
        }
        return inside;
    }

    private void OnDoorDestroyed()
    {
        SpawnAtRandomPoint(); // Spawn a new object
    }

    private void OnDrawGizmos()
    {
        if (polygonCollider != null)
        {
            Gizmos.color = Color.blue;
            Vector2[] points = polygonCollider.points;
            for (int i = 0; i < points.Length; i++)
            {
                Vector2 start = transform.TransformPoint(points[i]);
                Vector2 end = transform.TransformPoint(points[(i + 1) % points.Length]);
                Gizmos.DrawLine(start, end);
            }
        }

        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, minDistanceFromPlayer);
        }
    }
}
