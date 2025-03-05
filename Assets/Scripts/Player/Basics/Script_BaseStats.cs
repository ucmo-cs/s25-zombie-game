using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Script_BaseStats : MonoBehaviour
{
    [Header("Basic Stats")]
    [SerializeField] float health = 150;
    [SerializeField] float regenTimer = 2f;

    private float maxHealth;
    private Coroutine lastRegenTimer;
    private Coroutine lastRegen;

    void Start()
    {
        Script_UIManager.Instance.healthBar.maxValue = health;
        Script_UIManager.Instance.healthBar.value = health;
        maxHealth = health;
    }

    public void TakeDamage(float damage){
        health -= damage;
        Script_UIManager.Instance.healthBar.value = health;

        if (lastRegenTimer != null)
        {
            StopCoroutine(lastRegenTimer);
        }
        
        if (lastRegen != null)
        {
            StopCoroutine(lastRegen);
        }

        StartRegen();

        if (health <= 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void StartRegen()
    {
        if (health < maxHealth)
        {
            lastRegenTimer = StartCoroutine(RegenTimer());
        }
    }

    IEnumerator RegenTimer()
    {
        yield return new WaitForSeconds(regenTimer);
        lastRegen = StartCoroutine(Regen());
    }

    IEnumerator Regen()
    {
        if (health < maxHealth)
        {
            if (health + 10 >= maxHealth)
            {
                health = maxHealth;
            }
            else
                health += 10;


            Script_UIManager.Instance.healthBar.value = health;
            yield return new WaitForSeconds(0.1f);
            lastRegen = StartCoroutine(Regen());
        }
    }

    public void UpgradeHealth(float value)
    {
        maxHealth += value;
        health = maxHealth;
        Script_UIManager.Instance.healthBar.maxValue = health;
        Script_UIManager.Instance.healthBar.value = health;
    }

    public void UpgradeRegenTime(float value)
    {
        regenTimer = regenTimer - (regenTimer * value);
    }
}
