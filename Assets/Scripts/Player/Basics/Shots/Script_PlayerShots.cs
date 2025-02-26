using UnityEngine;

public class Script_PlayerShots : MonoBehaviour
{
    [SerializeField] ScriptableObject_Shot[] shots;

    private float currentDamageScale = 1;

    public ScriptableObject_Shot[] GetShots(){
        return shots;
    }

    public void Whiskey(float percentIncrease, GameObject entry){
        currentDamageScale += percentIncrease;
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Pistol>().UpgradeDamage(currentDamageScale);
        entry.GetComponent<Script_ShotInformation>().ShotBought();
    }
}
