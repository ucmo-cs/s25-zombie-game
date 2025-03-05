using UnityEngine;
using UnityEngine.Windows;

public class Script_OtherControls : MonoBehaviour
{
    public I_Interactable currentInteractable;

    // Input Variables
    private Input_Controller _input;

    private void Start()
    {
        _input = GetComponent<Input_Controller>();
        ToggleInput(false);
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
        if (_input.interact == true)
        {
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
