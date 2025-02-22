using UnityEngine;
using UnityEngine.InputSystem;

public class StartGameplay : MonoBehaviour
{
    public GameObject[] activate;
    InputAction grabInput;
    bool locked = true;

    void Start()
    {
        grabInput = InputSystem.actions.FindAction("Grab");
        Utils.RunDelay(this, () => locked = false, 0.1f);
    }

    void Update()
    {
        if (!locked && grabInput.WasPressedThisFrame())
        {
            Utils.RunDelay(this, StartGame, 0.1f);
        }
    }

    public void StartGame()
    {
        for (int i = 0; i < activate.Length; i++)
        {
            activate[i].SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
