using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Script_UIManager : MonoBehaviour
{

    [Header("UI Elements")]
    [SerializeField] public TMP_Text pointsText;
    [SerializeField] public TMP_Text scrapText;
    [SerializeField] public Slider healthBar;
    [SerializeField] Button startGameButton;

    public static Script_UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    
    public void SetStartGameButton()
    {
        startGameButton.interactable = !startGameButton.interactable;
    }

    public void ToggleGameplayUI(bool toggle)
    {
        pointsText.enabled = toggle;
        scrapText.enabled = toggle;
        healthBar.gameObject.SetActive(toggle);
    }
}
