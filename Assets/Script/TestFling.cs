using UnityEngine;

public class TestFling : MonoBehaviour
{
    public Rigidbody hipRB;
    Vector3 originalPos = new Vector3(0, 14.6f, -6.5f);
    public float force = 1;

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            originalPos = hipRB.position;
        }
        if (Input.GetMouseButton(0))
        {
            float relativeMouseY = Input.mousePosition.y / Screen.height;
            Vector3 lastPosition = transform.position;
            Vector3 newPosition = originalPos + new Vector3(0, (relativeMouseY - 0.5f) * force, 0);
            hipRB.linearVelocity = (newPosition - lastPosition) / Time.fixedDeltaTime;
        }
    }
}
