using Unity.Netcode;
using UnityEngine;

public class Script_Spawner : NetworkBehaviour
{
    public void SpawnEnemy(GameObject enemy)
    {
        GameObject newEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
        newEnemy.GetComponent<NetworkObject>().Spawn();
    }
}
