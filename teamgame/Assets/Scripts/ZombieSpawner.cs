using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;
    public DayNightCycle dayNightCycle;
    public int dayZombies = 3;
    public int nightZombies = 10;

    private GameObject[] zombies;

    void Start()
    {
        SpawnZombies(dayZombies);
    }

    void Update()
    {
        if(dayNightCycle.IsDay() && zombies.Length != dayZombies)
            ResetZombies(dayZombies);
        else if(!dayNightCycle.IsDay() && zombies.Length != nightZombies)
            ResetZombies(nightZombies);
    }

    void SpawnZombies(int count)
    {
        zombies = new GameObject[count];
        for(int i=0; i<count; i++)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            zombies[i] = Instantiate(zombiePrefab, spawn.position, Quaternion.identity);
        }
    }

    void ResetZombies(int count)
    {
        foreach(var z in zombies) Destroy(z);
        SpawnZombies(count);
    }
}
