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
    [SerializeField] Slider healthBar;

    private float maxHealth;
    private Coroutine lastRegenTimer;
    private Coroutine lastRegen;

    // Input Variables
    private Input_Controller _input;

    void Start()
    {
        _input = GetComponent<Input_Controller>();
        healthBar.maxValue = health;
        healthBar.value = health;
        maxHealth = health;
    }

    private void Update()
    {
        if (_input != null)
        {
            SkipTimer();
        }
    }

    public void TakeDamage(float damage){
        health -= damage;
        healthBar.value = health;

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


            healthBar.value = health;
            yield return new WaitForSeconds(0.1f);
            lastRegen = StartCoroutine(Regen());
        }
    }

    public void SkipTimer()
    {
        if (_input.endRound == true)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>().EndRoundTransitionEarly();
            _input.endRound = false;
        }
    }
}
