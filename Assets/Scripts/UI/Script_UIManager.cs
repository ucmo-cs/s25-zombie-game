using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Script_UIManager : MonoBehaviour
{

    [Header("UI Elements")]
    [SerializeField] public TMP_Text pointsText;
    [SerializeField] public TMP_Text scrapText;
    [SerializeField] public Slider healthBar;

    public static Script_UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
