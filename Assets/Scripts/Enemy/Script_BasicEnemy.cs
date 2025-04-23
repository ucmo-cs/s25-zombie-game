using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class Script_BasicEnemy : NetworkBehaviour
{

    [Header("Basic Stats")]
    [SerializeField] float health = 150;
    [SerializeField] public float damage = 50;
    [SerializeField] float speed = 5;
    private float speedIncrease = 1;
    [SerializeField] public GameObject scrapSpawnPoint;
    [SerializeField] float attackSpeed = 1;

    public bool hasHit = false;
    private NavMeshAgent navMeshAgent;
    public bool cantTakeDamage = false;
    [SerializeField] Rigidbody[] rigidbodies;
    [SerializeField] GameObject floatingDamageSpawnPoint;
    [SerializeField] GameObject floatingDamagePrefab;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = new Vector3(0, 0, 0);

        health += 50 * (GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>().GetRound() - 1);
        damage += 25 * (GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>().GetRound() / 5);
        speed += 1 * (GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>().GetRound() / 5);
        speedIncrease = 1 + (1 * (GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>().GetRound() / 5) / 5);
        attackSpeed += 0.25f * (GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>().GetRound() / 5);

        navMeshAgent.speed = speed;
        GetComponent<Animator>().SetFloat("AttackSpeed", attackSpeed);

        int randRunAnim = Random.Range(0, 3);
        GetComponent<Animator>().SetFloat("RandRun", randRunAnim);

        GetComponent<Animator>().SetFloat("MoveSpeed", speedIncrease);
        InitiateChase();
    }

    public void EndAttack(){
        hasHit = false;
        InitiateChase();
    }

    private void FindClosestPlayer()
    {

        GameObject closestPlayer = null;

        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>().GetPlayers().Count <= 0)
        {
            StopAllCoroutines();
            GetComponentInChildren<Animator>().SetBool("Game Ended", true);
            navMeshAgent.SetDestination(this.transform.position);
            return;
        }

        foreach (GameObject player in GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>().GetPlayers()) 
        {
            if (closestPlayer == null || Vector3.Distance(gameObject.transform.position, player.transform.position) < Vector3.Distance(gameObject.transform.position, closestPlayer.transform.position))
            {
                closestPlayer = player;
            }
        }

        if (closestPlayer != null)
        {
            navMeshAgent.destination = closestPlayer.transform.position;
        }

        else
            FindClosestPlayer();
    }

    public void InitiateChase(){
        FindClosestPlayer();
        StartCoroutine(StartChaseCycle());
    }

    IEnumerator StartChaseCycle(){
        yield return new WaitForSeconds(0.1f);
        FindClosestPlayer();

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            int randAttackAnim = Random.Range(0, 3);
            GetComponent<Animator>().SetFloat("RandAttack", randAttackAnim);

            Debug.Log("Reached Attack Trigger");
            GetComponentInChildren<Animator>().SetTrigger("Attack");
        }

        else
        {
            StartCoroutine(StartChaseCycle());
        }
    }

    public void TakeDamage(float damageTaken, int pointsAdded){
        if (cantTakeDamage)
        {
            return;
        }

        float newHealth = health;
        newHealth -= damageTaken;

        GameObject floatingDamageInstance = Instantiate(floatingDamagePrefab, floatingDamageSpawnPoint.transform.position, Quaternion.RotateTowards(floatingDamageSpawnPoint.transform.rotation, GameObject.FindGameObjectWithTag("LocalPlayer").transform.rotation, 360f), this.transform);
        floatingDamageInstance.GetComponentInChildren<TMP_Text>().text = damageTaken.ToString();
        Destroy(floatingDamageInstance, 3f);

        if (newHealth <= 0)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>().EnemyDeathRpc(new NetworkObjectReference(gameObject), new NetworkObjectReference(NetworkManager.Singleton.LocalClient.PlayerObject), pointsAdded);
        }

        else
            SetHealthRpc(newHealth);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void SetHealthRpc(float newHealth)
    {
        health = newHealth;
    }

    public IEnumerator ToggleKinematic(bool toggle, float timer)
    {
        yield return new WaitForSeconds(timer);

        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = toggle;
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void RagDollRpc()
    {
        cantTakeDamage = true;
        StopAllCoroutines();

        this.GetComponent<Animator>().enabled = false;
        navMeshAgent.enabled = false;

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        if (IsServer)
            Destroy(gameObject, 10f);
    }
}
