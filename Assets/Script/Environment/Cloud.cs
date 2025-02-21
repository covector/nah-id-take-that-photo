using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed;
    public float from;
    public float to;

    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        if (transform.position.x > to)
        {
            transform.position = new Vector3(from, transform.position.y, transform.position.z);
        }
    }
}
