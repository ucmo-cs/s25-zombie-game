using System.Collections.Generic;
using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using static Script_BaseStats;

public class Script_PlayerUpgrades : NetworkBehaviour
{
    [Header("Currencies")]
    [SerializeField] int points = 500;
    public int GetPoints() { return points; }
    [SerializeField] int scrap = 0;
    public int GetScrap() { return scrap; }

    // Mod Methods
    private List<Action> scrapMethods = new List<Action>();
    private List<Action> killMethods = new List<Action>();

    void Start()
    {
        Script_UIManager.Instance.pointsText.text = "Points: " + points;
        Script_UIManager.Instance.scrapText.text = "Scrap: " + scrap;
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void AddPointsRpc(int value){
        if (points + value < 0)
            points = 0;
        else
            points += value;

        if (IsLocalPlayer)
        {
            Script_UIManager.Instance.pointsText.text = "Points: " + points;
            EnemyKillMethods();
        }
    }

    public void AddBonusPoints(int value)
    {
        points += value;

        if (IsLocalPlayer)
            Script_UIManager.Instance.pointsText.text = "Points: " + points;
    }

    public void AddScrap(int value){
        foreach (Action action in scrapMethods)
        {
            action();
        }

        if (scrap + value < 0)
            scrap = 0;
        else
            scrap += value;

        if (IsLocalPlayer)
            Script_UIManager.Instance.scrapText.text = "Scrap: " + scrap;
    }

    public void RemoveScrap(int value)
    {
        scrap -= value;

        if (IsLocalPlayer)
            Script_UIManager.Instance.scrapText.text = "Scrap: " + scrap;
    }

    public void AddBonusScrap(int value)
    {
        scrap += value;

        if (IsLocalPlayer)
            Script_UIManager.Instance.scrapText.text = "Scrap: " + scrap;
    }

    public void AddScrapMethod(Action method)
    {
        scrapMethods.Add(method);
    }

    public void RemoveScrapMethod(Action method)
    {
        scrapMethods.Remove(method);
    }

    public void AddKillMethod(Action method)
    {
        killMethods.Add(method);
    }

    public void RemoveKillMethod(Action method)
    {
        killMethods.Remove(method);
    }

    public void EnemyKillMethods()
    {
        foreach (Action action in killMethods)
        {
            action();
        }
    }
}
