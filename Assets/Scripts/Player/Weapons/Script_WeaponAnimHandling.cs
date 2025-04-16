using UnityEngine;

public class Script_WeaponAnimHandling : MonoBehaviour
{
    private Pistol pistol;
    private float reloadSpeed = 1;

    public void SpeedUpReload(float percentIncrease)
    {
        reloadSpeed += percentIncrease;
        GetComponent<Animator>().SetFloat("ReloadSpeed", reloadSpeed);
    }
    public void ReloadWeapon()
    {
        if (pistol == null)
        {
            pistol = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponentInChildren<Pistol>();
        }

        Debug.Log("Sending pistol reload");
        pistol.Reload();
    }
}
