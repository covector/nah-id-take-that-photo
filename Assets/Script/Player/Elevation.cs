using TMPro;
using UnityEngine;

public class Elevation : MonoBehaviour
{
    public float multiplier = 1.0f;
    public float offset = 0.0f;
    public Transform player;

    TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float elevation = player.position.y * multiplier + offset;
        text.text = elevation.ToString("F0") + "m";
    }
}
