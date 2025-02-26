using TMPro;
using UnityEngine;

public class Script_PlayerUpgrades : MonoBehaviour
{
    [Header("Currencies")]
    [SerializeField] int points = 500;
    public int GetPoints() { return points; }
    [SerializeField] int scrap = 0;

    [Header("UI Elements")]
    [SerializeField] TMP_Text pointsText;
    [SerializeField] TMP_Text scrapText;

    void Start()
    {
        pointsText.text = "Points: " + points;
        scrapText.text = "Scrap: " + scrap;
    }

    public void AddPoints(int value){
        if (points + value < 0)
            points = 0;
        else
            points += value;

        pointsText.text = "Points: " + points;
    }

    public void AddScrap(int value){
        if (scrap + value < 0)
            scrap = 0;
        else
            scrap += value;

        scrapText.text = "Scrap: " + scrap;
    }
}
