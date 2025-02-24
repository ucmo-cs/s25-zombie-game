using TMPro;
using UnityEngine;

public class Script_Bar : MonoBehaviour, I_Interactable
{
    [Header("UI")]
    [SerializeField] TMP_Text prompt;
    [SerializeField] GameObject scrollViewUI;

    private BoxCollider menuTrigger;

    private bool playerIsAtBar = false;
    public void SetPlayerIsAtBar(bool value) { playerIsAtBar = value; }

    private GameObject player;

    private void Start()
    {
        menuTrigger = gameObject.AddComponent<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Player");

        menuTrigger.size = new Vector3(1.5f, 1.5f, 1.2f);
        menuTrigger.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsAtBar = true;
            player.GetComponent<Script_OtherControls>().currentInteractable = this;
            prompt.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsAtBar = false;
            player.GetComponent<Script_OtherControls>().currentInteractable = null;
            prompt.enabled = false;
        }
    }

    public void OnInteract()
    {
        if (playerIsAtBar)
        {
            Debug.Log("Open bar menu");
            scrollViewUI.SetActive(true);
            prompt.enabled = false;
            player.GetComponent<Script_OtherControls>().ToggleCursor();
            player.GetComponent<Script_OtherControls>().ToggleInput(false);
            scrollViewUI.GetComponentInChildren<Script_BarMenu>().inMenu = true;
        }
    }

    public void ReactivatePrompt()
    {
        if (playerIsAtBar)
        {
            prompt.enabled = true;
        }
    }
}
