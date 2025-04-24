using TMPro;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Script_UIManager : NetworkBehaviour
{

    [Header("UI Elements")]
    [SerializeField] public TMP_Text pointsText;
    [SerializeField] public TMP_Text scrapText;
    [SerializeField] public Slider healthBar;
    [SerializeField] GameObject networkUI;
    [SerializeField] GameObject lobbyUI;
    [SerializeField] Button startButton;
    [SerializeField] TMP_Text lobbyText;
    [SerializeField] public TMP_Text gunInfoText;
    [SerializeField] GameObject modIconHolder;
    [SerializeField] GameObject spectatorUI;
    private string localPlayerName = "";

    [SerializeField] NetworkObject playerPrefab;

    private Script_BaseStats currentSpectator = null;
    private int currentSpectatorIndex = 0;

    public static Script_UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (Script_SessionHandler.Instance.GetActiveSession() != null)
        {
            Script_SessionHandler.Instance.LeaveActiveSession();
        }
    }

    public void SwitchToLobbyUI(bool host)
    {
        networkUI.GetComponent<RectTransform>().localScale = Vector3.zero;
        lobbyText.text = Script_SessionHandler.Instance.GetActiveSession().Name;
        if (!host)
        {
            startButton.interactable = false;
        }
    }

    public void LeaveLobby()
    {
        networkUI.GetComponent<RectTransform>().localScale = new Vector3(1.3f ,1.3f ,1.3f);
    }

    public void ToggleNetworkUI(bool toggle)
    {
        networkUI.SetActive(toggle);
        lobbyUI.SetActive(false);
    }

    public void ToggleGameplayUI(bool toggle)
    {
        pointsText.enabled = toggle;
        scrapText.enabled = toggle;
        healthBar.gameObject.SetActive(toggle);
        gunInfoText.enabled = toggle;
        modIconHolder.SetActive(toggle);
    }

    public void ToggleSpectatorUI(bool toggle)
    {
        spectatorUI.SetActive(toggle);
    }

    public string GetUsername() 
    {
        return localPlayerName;
    }

    public void SetLocalUsername(string name)
    {
        localPlayerName = name;
    }

    public void SetSpectatorInfo(string name)
    {
        spectatorUI.GetComponentInChildren<TMP_Text>().text = "Currently Spectating: " + name;
    }

    public void SpectatorCamera(int index)
    {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>().GetPlayers().Count <= 0)
        {
            return;
        }

        currentSpectator = GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>().GetPlayers()[index].GetComponent<Script_BaseStats>();
        currentSpectatorIndex = index;
        SetSpectatorInfo(GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>().GetPlayers()[index].GetComponent<Script_BaseStats>().GetPlayerName());
        currentSpectator.GetComponentInChildren<CinemachineCamera>().enabled = true;
    }

    public void CheckSpectatorCamera()
    {
        if (currentSpectator != null && currentSpectator.GetDeathStatus())
        {
            SpectatorCamera(0);
        }
    }

    public void DisableSpectatorCamera()
    {
        if (currentSpectator != null)
        {
            currentSpectator.GetComponentInChildren<CinemachineCamera>().enabled = false;
            currentSpectator = null;
        }
    }
}
