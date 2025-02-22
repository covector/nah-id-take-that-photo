using Unity.Cinemachine;
using UnityEngine;
using static Utils;

public class MountainTracker : MonoBehaviour
{
    CinemachineOrbitalFollow ccam;
    public Transform hill;
    public Transform player;
    Vector3 hillCenter;
    public float speed;

    void Start()
    {
        hillCenter = new Vector3(hill.position.x, 0, hill.position.z);
        ccam = GetComponent<CinemachineOrbitalFollow>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 diff = (hillCenter - new Vector3(player.position.x, 0, player.position.z)).normalized;
        Vector3 viewDiff = (hillCenter - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
        //ccam.HorizontalAxis.Value = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
        // lerp the rotation
        ccam.HorizontalAxis.Value = Mathf.LerpAngle(ccam.HorizontalAxis.Value, Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg, Time.deltaTime * speed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(viewDiff), Time.deltaTime * speed);
    }
}
