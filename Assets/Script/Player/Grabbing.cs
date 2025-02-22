using System;
using UnityEngine;
using static Utils;

public class Grabbing : MonoBehaviour
{
    public Transform hill;
    public Vector2 hillCenter { get; private set; }
    public float startHeight;

    public Rigidbody leftHand;
    public Rigidbody rightHand;

    public Transform leftShoulder;
    public Transform rightShoulder;
    public float armExtendLength;

    public Vector3 firstLeftPos;
    public Vector3 firstRightPos;
    public Rigidbody hipRB;
    public Rigidbody[] rbs;
    protected Vector3 grabPos;
    public float force = 1f;
    public float horizontalForce = 1f;
    public float yOffset = -1f;
    public float arcStrength = 1f;
    public float tiltForce = 1f;
    public int grabbing { get; private set; } = 0; // 0: not grabbing, -1: left hand, 1: right hand


    void Start()
    {
        hillCenter = new Vector2(hill.position.x, hill.position.z);
    }

    bool isAtStart()
    {
        return hipRB.position.y < startHeight;
    }

    void ExtendArmToTryGrab()
    {
        Vector3 direction = new Vector3(hillCenter.x, 0, hillCenter.y) - new Vector3(hipRB.position.x, 0, hipRB.position.z);
        direction.Normalize();
        leftHand.linearVelocity = (leftShoulder.position + new Vector3(direction.x, 0, direction.y) * armExtendLength) - leftHand.position;
        rightHand.linearVelocity = (rightShoulder.position + new Vector3(direction.x, 0, direction.y) * armExtendLength) - rightHand.position;
    }

    void Pulling()
    {
        grabPos = grabbing > 0 ? rightHand.position : leftHand.position;
        float relativeMouseY = Mathf.Clamp01(Input.mousePosition.y / Screen.height);
        float relativeMouseX = Mathf.Clamp01(Input.mousePosition.x / Screen.width);
        Vector3 direction = new Vector3(hillCenter.x , 0, hillCenter.y) - new Vector3(hipRB.position.x, 0, hipRB.position.z);
        direction.Normalize();
        Vector3 horizontal = Vector3.Cross(direction, Vector3.up);
        Vector3 lastPosition = hipRB.position;
        Vector3 newPosition =
            grabPos +
            new Vector3(0, (0.5f - relativeMouseY) * force + yOffset, 0) +
            horizontal * (relativeMouseX - 0.5f) * horizontalForce - 
            arcStrength * direction * Mathf.Sqrt(0.25f - Mathf.Pow(relativeMouseY - 0.5f, 2));
        hipRB.linearVelocity = (newPosition - lastPosition) / Time.fixedDeltaTime;
    }

    bool TryGrab()
    {
        bool canRight = rightHand.GetComponent<Hand>().canGrab;
        bool canLeft = leftHand.GetComponent<Hand>().canGrab;
        if (canRight && canLeft)
        {
            RandomGrab();
            return true;
        }
        if (canRight || canLeft)
        {
            grabbing = canRight ? 1 : -1;
            Grab();
            return true;
        }
        return false;
    }

    void Grab()
    {
        bool right = grabbing > 0;
        leftHand.GetComponent<Rigidbody>().isKinematic = !right;
        rightHand.GetComponent<Rigidbody>().isKinematic = right;
    }

    void RandomGrab()
    {
        grabbing = UnityEngine.Random.value > 0.5f ? 1 : -1;
        Grab();
    }

    void UnGrab()
    {
        grabbing = 0;
        leftHand.GetComponent<Rigidbody>().isKinematic = false;
        rightHand.GetComponent<Rigidbody>().isKinematic = false;
    }

    void Update()
    {
        if (grabbing != 0)  // in grabbing
        {
            if (!Input.GetMouseButton(0))
            {
                UnGrab();
            }
        }
        else  // free falling
        {
            if (Input.GetMouseButton(0))
            {
                TryGrab();
            }
        }
    }

    void FixedUpdate()
    {
        if (isAtStart())  // first grab
        {
            if (Input.GetMouseButton(0)) {
                
                leftHand.position = firstLeftPos;
                rightHand.position = firstRightPos;
                RandomGrab();
            }
            return;
        }
        if (grabbing != 0)  // in grabbing
        {
            Pulling();
        } else  // free falling
        {
            //ExtendArmToTryGrab();
            Vector3 diff = (ToVector3(hillCenter) - new Vector3(hipRB.position.x, 0, hipRB.position.z)).normalized;
            Debug.Log(diff);
            hipRB.AddForce(diff * tiltForce);
        }
    }
}
