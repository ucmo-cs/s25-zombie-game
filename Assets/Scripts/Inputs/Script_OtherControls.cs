using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Script_OtherControls : NetworkBehaviour
{
    public I_Interactable currentInteractable;

    // Input Variables
    [SerializeField] private Input_Controller _input;

    private void Start()
    {
        if (IsLocalPlayer)
        {
            GameObject.FindGameObjectWithTag("Bar").GetComponent<Script_Bar>().SetPlayer(gameObject);
            gameObject.tag = "LocalPlayer";
        }
    }

    public void EnableInput()
    {
        if (IsLocalPlayer)
        {
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<Input_Controller>().enabled = true;
            GetComponentInChildren<CinemachineCamera>().enabled = true;
            ToggleCursor(true);
            ToggleInput(true);
        }
    }

    private void Update()
    {
        if (_input != null)
        {
            SkipTimer();
            CheckInteract();
        }
    }

    public void SkipTimer()
    {
        if (_input.endRound == true)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>().EndRoundTransitionEarly();
            _input.endRound = false;
        }
    }

    public void CheckInteract()
    {
        Debug.Log("Checking interact");
        if (_input.interact == true)
        {
            Debug.Log("Reached Interact");
            if (currentInteractable != null)
            {
                currentInteractable.OnInteract();
            }
            _input.interact = false;
        }
    }

    public void ToggleCursor()
    {
        _input.SetCursorState(!_input.cursorLocked);
    }

    public void ToggleCursor(bool toggle)
    {
        _input.SetCursorState(toggle);
    }

    public void ToggleInput(bool input)
    {
        _input.ToggleEnabled(input);
    }
}
