using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Script_ScrapMenu : MonoBehaviour
{
    [Header("Mods")]
    [SerializeField] GameObject modHolder;
    [SerializeField] GameObject mechanic;
    [SerializeField] GameObject contentPrefab;

    [Header("Mod Colors")]
    [SerializeField] Color commonColor;
    [SerializeField] Color rareColor;
    [SerializeField] Color epicColor;
    [SerializeField] Color legendaryColor;
    public bool inMenu = false;

    private List<I_Mods> mods = new List<I_Mods>();
    private List<GameObject> entries = new List<GameObject>();
    private List<I_Mods> activeMods = new List<I_Mods>();

    private void Start()
    {
        mods = modHolder.GetComponents<I_Mods>().ToList();
        mechanic.GetComponent<Script_Mechanic>().CloseMenu();
    }

    public void InitMenu(){
        inMenu = true;
        Populate();
    }

    private void Populate()
    {
        List<I_Mods> modsThatCanBeSelected = mods.Except(activeMods).ToList();

        for (int i = 0; i < 3; i++)
        {
            if(modsThatCanBeSelected.Count <= 0){
                return;
            }

            I_Mods modSelected = modsThatCanBeSelected[new System.Random().Next(modsThatCanBeSelected.Count)];
            modsThatCanBeSelected.Remove(modSelected);

            Color modColor = Color.black;

            switch (modSelected.rarity)
            {
                case I_Mods.Rarity.Common:
                    modColor = commonColor;
                    break;
                case I_Mods.Rarity.Rare:
                    modColor = rareColor;
                    break;
                case I_Mods.Rarity.Epic:
                    modColor = epicColor;
                    break;
                case I_Mods.Rarity.Legendary:
                    modColor = legendaryColor;
                    break;
            }

            GameObject newContent = Instantiate(contentPrefab, transform);

            newContent.GetComponent<Script_ModInformation>().SetInformation(modSelected.modName, 
                modSelected.modDescription, modColor);

            newContent.GetComponentInChildren<Button>().onClick.AddListener( delegate { AddMod(modSelected); });
            newContent.GetComponentInChildren<Button>().onClick.AddListener( delegate { Close(); });

            entries.Add(newContent);
        }
    }

    private void AddMod(I_Mods modToAdd){
        activeMods.Add(modToAdd);
        modToAdd.Activate();
    }

    public void Close()
    {
        foreach (GameObject entry in entries){
            Destroy(entry);
        }
        entries.Clear();

        gameObject.GetComponentInParent<Image>().gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Script_OtherControls>().ToggleCursor();
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Script_OtherControls>().ToggleInput(true);
        mechanic.GetComponent<Script_Mechanic>().ReactivatePrompt();
        inMenu = false;
    }
}
