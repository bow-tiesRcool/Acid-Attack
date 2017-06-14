using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public static Spawner spawner;
    public string boss;
    public string[] enemyPrefabNames;
    public bool spawn = true;

    public GameObject[] prefabs;

    private Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();
    private Dictionary<string, List<GameObject>> pools = new Dictionary<string, List<GameObject>>();

    void Awake()
    {
        if (spawner == null) spawner = this;
    }

    void Start ()
    {
        Reset();   
    }

    public void Reset()
    {
        prefabDict = new Dictionary<string, GameObject>();
        pools = new Dictionary<string, List<GameObject>>();

        for (int i = 0; i < prefabs.Length; i++)
        {
            GameObject prefab = prefabs[i];
            prefabDict[prefab.name] = prefab;
            pools[prefab.name] = new List<GameObject>();
        }
    } 

    public static GameObject Spawn (string name, bool spawnActive = false)
    {
        GameObject spawn = null;

        List<GameObject> pool = spawner.pools[name];
        spawn = pool.Find((g) => !g.activeSelf);

        if (spawn == null)
        {
            spawn = Instantiate(spawner.prefabDict[name]);
            pool.Add(spawn);
        }

        spawn.SetActive(spawnActive);
        return spawn;
    }

    public static GameObject BossSpawn(bool spawnActive = false)
    {
        GameObject spawn = null;

        List<GameObject> pool = spawner.pools[spawner.boss];
        spawn = pool.Find((g) => !g.activeSelf);

        if (spawn == null)
        {
            spawn = Instantiate(spawner.prefabDict[spawner.boss]);
            pool.Add(spawn);
        }

        spawn.SetActive(spawnActive);
        return spawn;
    }

    public IEnumerator SpawnEnemiesCoroutine()
    {
        while (spawn == true)
        {
            yield return new WaitForSeconds(.25f);

            string enemyPrefabName = enemyPrefabNames[Random.Range(0, enemyPrefabNames.Length)];

            GameObject enemy = Spawner.Spawn(enemyPrefabName);
            Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, Random.Range(.1f, .9f), -Camera.main.transform.position.z));

            enemy.transform.position = pos;
            enemy.SetActive(true);
        }
    }
}
