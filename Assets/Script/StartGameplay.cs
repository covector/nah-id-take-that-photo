using UnityEngine;

public class StartGameplay : MonoBehaviour
{
    public GameObject player;
    public void StartGame()
    {
        player.SetActive(true);
        gameObject.SetActive(false);
    }
}
