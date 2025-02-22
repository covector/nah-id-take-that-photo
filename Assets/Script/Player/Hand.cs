using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool canGrab = true;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Terrain")
        {
            canGrab = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Terrain")
        {
            canGrab = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Terrain")
        {
            canGrab = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Terrain")
        {
            canGrab = false;
        }
    }
}
