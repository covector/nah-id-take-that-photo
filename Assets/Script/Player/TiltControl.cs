using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class TiltControl : MonoBehaviour
{
    public Transform hill;
    Vector3 hillCenter;
    public Transform player;
    public float torque = 1f;
    public float force = 1f;

    void Start()
    {
        hillCenter = new Vector3(hill.position.x, 0, hill.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            Vector3 diff = (hillCenter - new Vector3(player.position.x, 0, player.position.z)).normalized;
            Vector3 axis = Vector3.Cross(Vector3.up, diff);
            //player.GetComponent<Rigidbody>().AddTorque(axis * torque * Input.GetAxisRaw("Vertical"));
            player.GetComponent<Rigidbody>().AddForce(diff * force * Input.GetAxisRaw("Vertical"));
        }
    }
}
