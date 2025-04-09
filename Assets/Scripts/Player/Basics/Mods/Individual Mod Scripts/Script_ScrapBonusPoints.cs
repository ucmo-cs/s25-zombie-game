using UnityEngine;

public class Script_ScrapBonusPoints : MonoBehaviour, I_Mods
{
    public I_Mods.Rarity rarity { get => _rarity; set => _rarity = value; }
    public string modName { get => _modName; set => _modName = value; }
    public string modDescription { get => _modDescription; set => _modDescription = value; }

    string _modName;
    string _modDescription;
    [SerializeField] I_Mods.Rarity _rarity;

    int bonus = 0;

    private void Start()
    {
        switch (rarity)
        {
            case I_Mods.Rarity.Common:
                bonus = 100;
                break;
            case I_Mods.Rarity.Rare:
                bonus = 250;
                break;
            case I_Mods.Rarity.Epic:
                bonus = 500;
                break;
            case I_Mods.Rarity.Legendary:
                bonus = 1000;
                break;
        }

        modName = "Gold Wiring";
        modDescription = "On scrap pickup, gain a extra " + bonus + " points!";
    }

    public void Activate()
    {
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<Script_PlayerUpgrades>().AddScrapMethod(delegate { BonusScrap(); });
    }

    public void Deactivate()
    {
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<Script_PlayerUpgrades>().RemoveScrapMethod(delegate { BonusScrap(); });
    }

    public void BonusScrap()
    {
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Script_PlayerUpgrades>().AddBonusPoints(bonus);
    }
}
