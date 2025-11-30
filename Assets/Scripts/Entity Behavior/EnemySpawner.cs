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
    public Transform[] spawnPoints;
    private Transform parentTransform;

    void Start()
    {
        allEnemiesSpawned = false;
        parentTransform = GameObject.Find("Enemies").transform;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        foreach (Wave wave in waves)  // for each wave
        {
            yield return new WaitForSeconds(wave.waveCooldown);  // wait for the interval to end
            foreach (GameObject unit in wave.units)  // spawn all enemies in the wave waiting inbetween
            {
                Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;  // pick random spot to spawn from
                GameObject spawnedUnit = Instantiate(unit, spawnPosition, Quaternion.identity, parentTransform);
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
