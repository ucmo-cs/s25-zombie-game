using System.Collections;
using TMPro;
using UnityEngine;

public class Script_GameController : MonoBehaviour
{
    [Header("Gameplay Settings")]
    [SerializeField] float spawnIntervals = 1f;
    [SerializeField] int spawnAmount = 6;
    [SerializeField] int spawnIncreaseIncrements = 2;
    [SerializeField]
    Script_Spawner[] spawns = new Script_Spawner[0];
    [SerializeField] Script_Saloon saloon;

    [Header("Enemy Settings")]
    [SerializeField] GameObject enemyPrefab;

    [Header("UI Settings")]
    [SerializeField] TMP_Text roundUI;
    [SerializeField] TMP_Text roundTimerUI;
    [SerializeField] GameObject networkUI;

    private int round = 0;
    private int currentSpawns = 0;
    private int enemiesLeft = 0;
    private bool isTransitioning = false;

    public void StartGame()
    {
        StartRound();
        networkUI.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Script_OtherControls>().ToggleInput(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Script_OtherControls>().ToggleCursor(true);
    }

    public void StartRound()
    {
        spawnAmount += spawnIncreaseIncrements;
        round++;
        currentSpawns = 0;
        enemiesLeft = spawnAmount;
        roundUI.text = round.ToString();
        StartCoroutine(SpawnCycle());
    }

    IEnumerator SpawnCycle()
    {
        if (currentSpawns < spawnAmount)
        {
            Script_Spawner spawner = spawns[Random.Range(0, spawns.Length)];
            if (spawner != null)
            {
                spawner.SpawnEnemy(enemyPrefab);
                currentSpawns++;
            }

            yield return new WaitForSeconds(spawnIntervals);
            StartCoroutine(SpawnCycle());
        }
    }

    public void EnemyDeath()
    {
        enemiesLeft--;

        if (enemiesLeft <= 0)
        {
            StopCoroutine(SpawnCycle());
            Debug.Log("Round ended");
            StartRoundTransition();
        }
    }

    public void StartRoundTransition()
    {
        float time = Time.time + 60;
        saloon.DoorToggle(true);
        isTransitioning = true;
        roundTimerUI.enabled = true;
        StartCoroutine(RoundTransition(time));
    }

    public void EndRoundTransitionEarly()
    {
        if (isTransitioning)
        {
            roundTimerUI.enabled = false;
            isTransitioning = false;
            saloon.DoorToggle(false);
            saloon.KickOutPlayer();
            StartRound();
        }
    }

    IEnumerator RoundTransition(float endTime)
    {
        if (isTransitioning)
        {
            if (Time.time >= endTime)
            {
                roundTimerUI.enabled = false;
                isTransitioning = false;
                saloon.DoorToggle(false);
                saloon.KickOutPlayer();
                StartRound();
            }
            else
            {
                roundTimerUI.text = (endTime - Time.time).ToString("F2");
                yield return new WaitForEndOfFrame();
                StartCoroutine(RoundTransition(endTime));
            }
        }
    }
}