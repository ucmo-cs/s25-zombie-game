using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class Script_BasicEnemy : MonoBehaviour
{

    [Header("Basic Stats")]
    [SerializeField] float health = 150;
    [SerializeField] public float damage = 50;
    [SerializeField] float speed = 1;

    [Header("Drops")]
    [SerializeField] GameObject scrapPrefab;

    public bool hasHit = false;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        InitiateChase();
    }

    public void EndAttack(){
        hasHit = false;
        InitiateChase();
    }

    public void InitiateChase(){
        navMeshAgent.destination = GameObject.FindGameObjectWithTag("Player").transform.position;
        StartCoroutine(StartChaseCycle());
    }

    IEnumerator StartChaseCycle(){
        yield return new WaitForSeconds(0.1f);
        
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance){
            Debug.Log("Reached Attack Trigger");
            GetComponent<Animator>().SetTrigger("Attack");
        }

        else {
            navMeshAgent.destination = GameObject.FindGameObjectWithTag("Player").transform.position;

            StartCoroutine(StartChaseCycle());
        }
    }

    public void TakeDamage(float damageTaken, int pointsAdded){
        health -= damageTaken;

        if (health <= 0)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>().EnemyDeathRpc(new NetworkObjectReference(gameObject), new NetworkObjectReference(NetworkManager.Singleton.LocalClient.PlayerObject), pointsAdded);
        }
    }
}
