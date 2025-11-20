using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    class Wave
    {
        public GameObject[] units;
        public float waveCooldown;
        public float spawnCooldown;
    }

    [SerializeField] Wave[] waves;

    public static bool allEnemiesSpawned;
    public bool isLastSpawner;  // Set true on the spawner that finishes spawning last


    void Start()
    {
        allEnemiesSpawned = false;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        foreach (Wave wave in waves)  // for each wave
        {
            yield return new WaitForSeconds(wave.waveCooldown);  // wait for the interval to end
            foreach (GameObject unit in wave.units)  // spawn all enemies in the wave waiting inbetween
            {
                GameObject spawnedUnit = Instantiate(unit, transform.position, Quaternion.identity, transform);
                GameController.enemies.Add(spawnedUnit);
                yield return new WaitForSeconds(wave.spawnCooldown);
            }
        }

        if (isLastSpawner) 
        {
            allEnemiesSpawned = true;
        }
    }
}
