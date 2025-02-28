using UnityEngine;

public interface I_Mods
{
    public Color modColor {get; set;}
    public string modName {get; set;}
    public string modDescription {get; set;}

    public void Activate();
    public void Deactivate();
}
