using StarterAssets;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Script_SprintDamageBonus : MonoBehaviour, I_Mods, I_Mods_DamageBoost
{
    public I_Mods.Rarity rarity { get => _rarity; set => _rarity = value; }
    public string modName { get => _modName; set => _modName = value; }
    public string modDescription { get => _modDescription; set => _modDescription = value; }
    public float currentBonus { get => _currentBonus; set => _currentBonus = value; }

    string _modName;
    string _modDescription;
    [SerializeField] I_Mods.Rarity _rarity;

    float _currentBonus = 0;
    float maxBonus = 0;
    float boostBuffer = 0.01f;
    bool buffered = false;

    private void Start()
    {
        switch (rarity)
        {
            case I_Mods.Rarity.Common:
                maxBonus = 50;
                break;
            case I_Mods.Rarity.Rare:
                maxBonus = 100;
                break;
            case I_Mods.Rarity.Epic:
                maxBonus = 300;
                break;
            case I_Mods.Rarity.Legendary:
                maxBonus = Mathf.Infinity;
                break;
        }

        modName = "Momentum";
        modDescription = "Sprinting will build up to " + maxBonus + " bonus damage for one shot!";
    }

    public void Activate()
    {
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<FirstPersonController>().AddSprintMethod(delegate { SprintBonus(); });
    }

    public void Deactivate()
    {
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<FirstPersonController>().RemoveSprintMethod(delegate { SprintBonus(); });
    }

    public void SprintBonus()
    {
        if (currentBonus + 1 <= maxBonus && !buffered)
        {
            StartCoroutine(SprintBonusBuffer());
            GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<Pistol>().BoostDamage(-currentBonus);
            currentBonus++;
            GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<Pistol>().BoostDamage(currentBonus);
        }
    }

    IEnumerator SprintBonusBuffer()
    {
        buffered = true;
        yield return new WaitForSeconds(boostBuffer);
        buffered = false;
    }
}
