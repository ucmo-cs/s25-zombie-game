using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Script_GameController : NetworkBehaviour
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

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnectedCallback;
    }

    private void ClientConnectedCallback(ulong obj)
    {
        if (NetworkManager.Singleton.LocalClientId != 0)
        {
            networkUI.SetActive(false);
            NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Script_OtherControls>().EnableInput();
        }
    }
    public void StartGame()
    {
        StartRound();
        networkUI.SetActive(false);
        NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Script_OtherControls>().EnableInput();
    }

    public void StartRound()
    {
        if (NetworkManager.IsServer)
        {
            spawnAmount += spawnIncreaseIncrements;
            currentSpawns = 0;
            enemiesLeft = spawnAmount;
            round++;
            StartCoroutine(SpawnCycle());
        }
        StartRoundRpc(round);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void StartRoundRpc(int newRound)
    {
        roundUI.text = newRound.ToString();
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

    [Rpc(SendTo.Server)]
    public void EnemyDeathRpc(NetworkObjectReference enemy, NetworkObjectReference playerThatKilled, int pointsAdded)
    {
        Debug.Log("Enemy has died");
        if (UnityEngine.Random.Range(1, 10) <= 2)
        {
            Debug.Log("Enemy has dropped scrap");
        }

        GameObject playerCredit = playerThatKilled;
        playerCredit.GetComponent<Script_PlayerUpgrades>().AddPointsRpc(pointsAdded);

        GameObject enemyGameObject = enemy;
        Destroy(enemyGameObject);

        enemiesLeft--;

        if (enemiesLeft <= 0)
        {
            StopCoroutine(SpawnCycle());
            Debug.Log("Round ended");
            StartRoundTransitionRpc();
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void StartRoundTransitionRpc()
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