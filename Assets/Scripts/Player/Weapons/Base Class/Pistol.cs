using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol : MonoBehaviour
{
    [Header("Basic Stats")]
    [SerializeField] GameObject pistolModel;
    [SerializeField] float headshotMultiplier;
    [SerializeField] float initDamage;
    [SerializeField] float initFireRate;
    [SerializeField] public int clipSize;
    [SerializeField] public int currentAmmoAmount;

    // Animation Variables
    private bool isReloading;
    [SerializeField] private bool isShooting;
    [SerializeField] private bool canNotShoot;

    // Input Variables
    private Input_Controller _input;

    // Variables for upgrades
    private float currentDamage;
    private float currentFireRate;

    private Camera FPCamera;


    public void Awake(){
        currentAmmoAmount = clipSize;
    }

    private void Start(){
        FPCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        currentDamage = initDamage;
        currentFireRate = initFireRate;
        _input = GetComponentInParent<Input_Controller>();
    }

    private void FixedUpdate() {
        ButtonReload(false);
        ButtonShoot();
    }

    private void ButtonShoot(){
        if(_input.fire){
            if(!isShooting && !canNotShoot){
                isShooting = true;
                Shoot();
            }
        }
        else{
            isShooting = false;
        }
    }

    public void Shoot(){
        if(currentAmmoAmount != 0)
        {
            if(!isReloading && isShooting){
                currentAmmoAmount--;
                Debug.Log("Shot Gun, Current Ammo: " + currentAmmoAmount);
                RaycastHit hit;
                Vector3 direction = GetShootingDirection();
                Physics.Raycast(FPCamera.transform.position, direction, out hit);
                canNotShoot = true;
                StartCoroutine("CanNotShoot");
                if(hit.transform != null){
                    Debug.Log("Hit Object: " + hit.transform.gameObject.name);
                    Script_BasicEnemy enemy = null;

                    float tempDamage = currentDamage;
                    int points = 0;

                    if(hit.transform.tag == "Enemy Head"){
                        tempDamage *= headshotMultiplier;
                        points = 100;
                        enemy = hit.transform.GetComponentInParent<Script_BasicEnemy>();
                    }
                    else if (hit.transform.tag == "Enemy"){
                        points = 50;
                        enemy = hit.transform.GetComponent<Script_BasicEnemy>();
                    }

                    if(enemy != null){
                        Debug.Log(tempDamage);
                        enemy.TakeDamage(tempDamage, points);
                    }
                }
            }
        }
        else if(!isReloading){
            ButtonReload(true);
        }
    }

    public void Reload(){
        Debug.Log("Gun Reloaded");
        currentAmmoAmount = clipSize;
        isReloading = false;
        _input.reload = false;
    }

    public void ButtonReload(bool autoReload){
        if (_input.reload || autoReload ){
            if (currentAmmoAmount < clipSize && !isReloading){
                isReloading = true;

                pistolModel.GetComponent<Animator>().SetTrigger("Reload");
                GetComponentInParent<Script_BaseStats>().TriggerReloadMethods();
                _input.reload = false;
            }
            else
                _input.reload = false;
        }
    }

    IEnumerator CanNotShoot(){
        Debug.Log("Current fire rate: " + currentFireRate);
        yield return new WaitForSeconds((float)(1.0/currentFireRate));
        canNotShoot = false;
    }

    Vector3 GetShootingDirection(){
        Vector3 targetPos = FPCamera.gameObject.GetComponent<Transform>().position + FPCamera.gameObject.GetComponent<Transform>().forward;
        Vector3 direction = targetPos - FPCamera.gameObject.GetComponent<Transform>().position;
        return direction.normalized;
    }

    public void Disable(){
        this.gameObject.SetActive(false);
    }

    public void StopReload()
    {
        isReloading = false;
    }

    public void UpgradeDamage(float percentIncrease){
        currentDamage = initDamage * percentIncrease;
    }

    public void UpgradeFireRate(float percentIncrease)
    {
        currentFireRate = initFireRate * percentIncrease;
    }

    public void UpgradeReloadSpeed(float percentIncrease)
    {
        pistolModel.GetComponent<Script_WeaponAnimHandling>().SpeedUpReload(percentIncrease);
    }
}
