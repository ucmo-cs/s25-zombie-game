using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Script_BaseStats : MonoBehaviour
{
    [Header("Basic Stats")]
    [SerializeField] float health = 150;
    [SerializeField] Slider healthBar;

    void Start()
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    public void TakeDamage(float damage){
        health -= damage;
        healthBar.value = health;

        if (health <= 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
