using StarterAssets;
using UnityEngine;

public class Script_ScrapHealth : MonoBehaviour, I_Mods
{
    public I_Mods.Rarity rarity { get => _rarity; set => _rarity = value; }
    public string modName { get => _modName; set => _modName = value; }
    public string modDescription { get => _modDescription; set => _modDescription = value; }

    string _modName;
    string _modDescription;
    [SerializeField] I_Mods.Rarity _rarity;

    int health = 0;

    private void Start()
    {
        switch (rarity)
        {
            case I_Mods.Rarity.Common:
                health = 5;
                break;
            case I_Mods.Rarity.Rare:
                health = 15;
                break;
            case I_Mods.Rarity.Epic:
                health = 25;
                break;
            case I_Mods.Rarity.Legendary:
                health = 50;
                break;
        }

        modName = "Recycled Parts";
        modDescription = "On scrap pickup, recover " + health + " health!";
    }

    public void Activate()
    {
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<Script_PlayerUpgrades>().AddScrapMethod(delegate { HealthBonus(); });
    }

    public void Deactivate()
    {
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<Script_PlayerUpgrades>().RemoveScrapMethod(delegate { HealthBonus(); });
    }

    public void HealthBonus()
    {
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Script_BaseStats>().AddHealth(health);
    }
}
