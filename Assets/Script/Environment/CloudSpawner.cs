using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public float size;
    public int count;
    public GameObject cloudPrefab;
    public Vector2 speedRange;

    void Awake()
    {
        Spawn();
    }

    void Spawn()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(cloudPrefab, new Vector3(Random.Range(-size, size), transform.position.y, Random.Range(-size, size)), Quaternion.identity, transform);
            Cloud cloud = go.GetComponent<Cloud>();
            cloud.speed = Random.Range(speedRange.x, speedRange.y);
            cloud.from = -size;
            cloud.to = size;
        }
    }
}
