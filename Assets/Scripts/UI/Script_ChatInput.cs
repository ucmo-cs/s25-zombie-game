using System.Text.RegularExpressions;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Script_ChatInput : MonoBehaviour
{
    public bool deselecting = false;
    public void Selected()
    {
        GetComponent<TMP_InputField>().onEndEdit.AddListener(OnEndEdit);
    }

    public void Deselected()
    {
        if (!deselecting)
        {
            deselecting = true;
            GetComponent<TMP_InputField>().onEndEdit.RemoveListener(OnEndEdit);
            GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Input_Controller>().SetCursorState(true);
            deselecting = false;

            gameObject.SetActive(false);
        }
    }

    private void OnEndEdit(string inputString)
    {
        // Optional check if don't want users submitting an empty string.
        if (string.IsNullOrEmpty(inputString))
        {
            return;
        }

        // Checks that OnEndEdit was triggered by a Return/Enter key press this frame,
        // rather than just unfocusing (clicking off) the input field.
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (inputString.StartsWith("/addpoints"))
            {
                AddPointsCommand(inputString);
                return;
            }

            if (inputString.StartsWith("/addscrap"))
            {
                AddScrapCommand(inputString);
                return;
            }

            if (inputString.StartsWith("/nextround"))
            {
                SetNextRoundCommand(inputString);
                return;
            }

            GameObject.FindGameObjectWithTag("Chat Text").GetComponent<TMP_Text>().text += inputString + "\n";
            GetComponent<TMP_InputField>().text = "";

            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void AddPointsCommand(string inputString)
    {
        // Regular expression to match any character that is not a digit (0-9)
        string pattern = "[^0-9]";
        // Replace all occurrences of the matched characters with an empty string.
        string points = Regex.Replace(inputString, pattern, "");
        int pointsToAdd = int.Parse(points);

        GetComponent<TMP_InputField>().text = "";

        Debug.Log("Points Adding: " + pointsToAdd);

        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Script_PlayerUpgrades>().AddBonusPoints(pointsToAdd);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void AddScrapCommand(string inputString)
    {
        // Regular expression to match any character that is not a digit (0-9)
        string pattern = "[^0-9]";
        // Replace all occurrences of the matched characters with an empty string.
        string scrap = Regex.Replace(inputString, pattern, "");
        int scrapToAdd = int.Parse(scrap);

        GetComponent<TMP_InputField>().text = "";

        Debug.Log("Scrap Adding: " + scrap);

        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Script_PlayerUpgrades>().AddBonusScrap(scrapToAdd);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SetNextRoundCommand(string inputString)
    {
        // Regular expression to match any character that is not a digit (0-9)
        string pattern = "[^0-9]";
        // Replace all occurrences of the matched characters with an empty string.
        string nextRound = Regex.Replace(inputString, pattern, "");
        int nextRoundInt = int.Parse(nextRound);

        GetComponent<TMP_InputField>().text = "";

        Debug.Log("Setting next round to " + nextRoundInt);

        if (nextRoundInt > 0 && NetworkManager.Singleton.IsServer)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>().DebugNextRoundRpc(nextRoundInt);
        }
        else
        {
            if (nextRoundInt <= 0)
            {
                GameObject.FindGameObjectWithTag("Chat Text").GetComponent<TMP_Text>().text += "/nextround requires a positive integer greater than 0!" + "\n";
            }
            else
            {
                GameObject.FindGameObjectWithTag("Chat Text").GetComponent<TMP_Text>().text += "/nextround can only be performed by the host!" + "\n";
            }
        }

        EventSystem.current.SetSelectedGameObject(null);
    }
}
