using UnityEngine;

public class StartGameplay : MonoBehaviour
{
    public GameObject[] activate;
    public void StartGame()
    {
        for (int i = 0; i < activate.Length; i++)
        {
            activate[i].SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
