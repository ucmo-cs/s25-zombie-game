using TMPro;
using UnityEngine;

public class Script_PlayerUpgrades : MonoBehaviour
{
    [Header("Currencies")]
    [SerializeField] int points = 500;
    public int GetPoints() { return points; }
    [SerializeField] int scrap = 0;
    public int GetScrap() { return scrap; }

    void Start()
    {
        Script_UIManager.Instance.pointsText.text = "Points: " + points;
        Script_UIManager.Instance.scrapText.text = "Scrap: " + scrap;
    }

    public void AddPoints(int value){
        if (points + value < 0)
            points = 0;
        else
            points += value;

        Script_UIManager.Instance.pointsText.text = "Points: " + points;
    }

    public void AddScrap(int value){
        if (scrap + value < 0)
            scrap = 0;
        else
            scrap += value;

        Script_UIManager.Instance.scrapText.text = "Scrap: " + scrap;
    }
}
