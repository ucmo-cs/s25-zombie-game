using UnityEngine;

public class Script_Spawner : MonoBehaviour
{
    public void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, transform.position, Quaternion.identity);
    }
}
