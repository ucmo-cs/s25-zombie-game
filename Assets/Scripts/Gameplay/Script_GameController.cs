using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] GameObject scrapPrefab;

    [Header("UI Settings")]
    [SerializeField] TMP_Text roundUI;
    [SerializeField] TMP_Text roundTimerUI;
    [SerializeField] GameObject networkUI;
    [SerializeField] GameObject backgroundUI;

    private int round = 0;
    private int currentSpawns = 0;
    private int enemiesLeft = 0;
    private bool isTransitioning = false;
    private List<GameObject> players;
    private List<GameObject> alivePlayers;
    private List<GameObject> deadPlayers = new List<GameObject>();
    public List<GameObject> GetPlayers() { return alivePlayers; }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnectedCallback;
    }

    private void ClientConnectedCallback(ulong obj)
    {
        if (NetworkManager.Singleton.LocalClientId != 0)
        {
            networkUI.SetActive(false);
            backgroundUI.GetComponentInChildren<TMP_Text>().enabled = true;
            NetworkManager.LocalClient.PlayerObject.GetComponent<CharacterController>().enabled = false;
            NetworkManager.LocalClient.PlayerObject.transform.position = new Vector3(NetworkManager.LocalClientId * 2, 10, 0);
            NetworkManager.LocalClient.PlayerObject.GetComponent<CharacterController>().enabled = true;
        }

    }

    [Rpc(SendTo.ClientsAndHost)]
    public void StartGameRpc()
    {
        StartRound();
        networkUI.SetActive(false);
        NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Script_OtherControls>().EnableInput();
        players = GameObject.FindGameObjectsWithTag("Player").ToList<GameObject>();
        players.Add(GameObject.FindGameObjectWithTag("LocalPlayer"));
        alivePlayers = new List<GameObject>(players);
        backgroundUI.SetActive(false);
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
            StartRoundRpc(round);
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void StartRoundRpc(int newRound)
    {
        roundUI.text = newRound.ToString();
        foreach (GameObject player in deadPlayers)
        {
            player.SetActive(true);
            player.GetComponent<Script_BaseStats>().Revive();
            deadPlayers.Remove(player);
            alivePlayers.Add(player);
        }
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
        GameObject enemyGameObject = enemy;
        Transform scrapPosition = enemyGameObject.transform;

        Debug.Log("Enemy has died");

        GameObject playerCredit = playerThatKilled;
        playerCredit.GetComponent<Script_PlayerUpgrades>().AddPointsRpc(pointsAdded);

        Destroy(enemyGameObject);
        if (UnityEngine.Random.Range(1, 10) <= 2)
        {
            Debug.Log("Enemy has dropped scrap");
            SpawnScrapRpc(scrapPosition.position);
        }

        enemiesLeft--;

        if (enemiesLeft <= 0)
        {
            StopCoroutine(SpawnCycle());
            Debug.Log("Round ended");
            StartRoundTransitionRpc();
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void SpawnScrapRpc(Vector3 position)
    {
        Instantiate(scrapPrefab, position, Quaternion.identity);
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

    [Rpc(SendTo.ClientsAndHost)]
    public void EndRoundTransitionEarlyRpc()
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

    public void PlayerDeath(GameObject player)
    {
        if (alivePlayers.Contains(player))
            alivePlayers.Remove(player);
        if (!deadPlayers.Contains(player))
            deadPlayers.Add(player);
    }
}