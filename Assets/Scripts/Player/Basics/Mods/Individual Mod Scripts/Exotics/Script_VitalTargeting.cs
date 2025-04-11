using UnityEngine;

public class Script_VitalTargeting : MonoBehaviour, I_Mods
{
    public I_Mods.Rarity rarity { get => _rarity; set => _rarity = value; }
    public string modName { get => _modName; set => _modName = value; }
    public string modDescription { get => _modDescription; set => _modDescription = value; }

    string _modName;
    string _modDescription;
    [SerializeField] I_Mods.Rarity _rarity;

    private void Start()
    {
        rarity = I_Mods.Rarity.EXOTIC;

        modName = "Vital Targeting";
        modDescription = "All damage is counted as headshot damage!";
    }

    public void Activate()
    {
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<Pistol>().vitalTargeting = true;
    }

    public void Deactivate()
    {
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<Pistol>().vitalTargeting = false;
    }
}
