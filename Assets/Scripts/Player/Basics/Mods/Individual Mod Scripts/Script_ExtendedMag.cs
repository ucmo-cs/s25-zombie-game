using UnityEngine;

public class Script_ExtendedMag : MonoBehaviour, I_Mods
{
    public Color modColor { get => _modColor; set => _modColor = value; }
    public string modName { get => _modName; set => _modName = value; }
    public string modDescription { get => _modDescription; set => _modDescription = value; }

    [SerializeField] Color _modColor;
    [SerializeField] string _modName;
    [SerializeField] string _modDescription;

    public void Activate()
    {
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<Pistol>().clipSize += 2;
    }

    public void Deactivate()
    {
        GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<Pistol>().clipSize -= 2;
    }
}
