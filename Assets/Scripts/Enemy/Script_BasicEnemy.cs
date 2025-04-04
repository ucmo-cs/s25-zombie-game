using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class Script_BasicEnemy : NetworkBehaviour
{

    [Header("Basic Stats")]
    [SerializeField] float health = 150;
    [SerializeField] public float damage = 50;
    [SerializeField] float speed = 1;

    public bool hasHit = false;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = new Vector3(0, 0, 0);

        InitiateChase();
    }

    public void EndAttack(){
        hasHit = false;
        InitiateChase();
    }

    private void FindClosestPlayer()
    {

        GameObject closestPlayer = null;

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
        StartCoroutine(StartChaseCycle());
    }

    IEnumerator StartChaseCycle(){
        yield return new WaitForSeconds(0.1f);
        FindClosestPlayer();

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance){
            Debug.Log("Reached Attack Trigger");
            GetComponent<Animator>().SetTrigger("Attack");
        }

        else {
            StartCoroutine(StartChaseCycle());
        }
    }

    public void TakeDamage(float damageTaken, int pointsAdded){
        float newHealth = health;
        newHealth -= damageTaken;

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
}
