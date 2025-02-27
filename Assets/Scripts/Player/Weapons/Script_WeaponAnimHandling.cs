using UnityEngine;

public class Script_WeaponAnimHandling : MonoBehaviour
{
    private Pistol pistol;
    private float reloadSpeed = 1;

    private void Start()
    {
        pistol = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Pistol>();
    }

    public void SpeedUpReload(float percentIncrease)
    {
        reloadSpeed += percentIncrease;
        GetComponent<Animator>().SetFloat("ReloadSpeed", reloadSpeed);
    }
    public void ReloadWeapon()
    {
        pistol.Reload();
    }
}
