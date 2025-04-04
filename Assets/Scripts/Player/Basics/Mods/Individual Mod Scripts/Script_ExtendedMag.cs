using UnityEngine;

public class Script_ExtendedMag : MonoBehaviour, I_Mods
{
    public I_Mods.Rarity rarity { get => _rarity; set => _rarity = value; }
    public string modName { get => _modName; set => _modName = value; }
    public string modDescription { get => _modDescription; set => _modDescription = value; }

    string _modName;
    string _modDescription;
    [SerializeField] I_Mods.Rarity _rarity;

    private int sizeIncrease;

    private void Start()
    {
        switch (rarity)
        {
            case I_Mods.Rarity.Common:
                sizeIncrease = 2;
                break;
            case I_Mods.Rarity.Rare:
                sizeIncrease = 4;
                break;
            case I_Mods.Rarity.Epic:
                sizeIncrease = 6;
                break;
            case I_Mods.Rarity.Legendary:
                sizeIncrease = 8;
                break;
        }

        modName = "Extended Mag";
        modDescription = "Increases Clip Size by " + sizeIncrease + "!";
    }

    public void Activate()
    {
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<Pistol>().clipSize += sizeIncrease;
    }

    public void Deactivate()
    {
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<Pistol>().clipSize -= sizeIncrease;
    }
}
