using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Script_ModInformation : MonoBehaviour
{
    private string modName;
    private string modDescription;
    private Color color;

    public void SetInformation(string newName, string newDesc, Color newColor){
        modName = newName;
        modDescription = newDesc;
        color = newColor;

        GetComponentInChildren<TMP_Text>().text = "<uppercase><b>" + modName + "</b></uppercase>\n" + modDescription;
        GetComponentInChildren<Button>().gameObject.GetComponent<Image>().color = color;
    }
}
