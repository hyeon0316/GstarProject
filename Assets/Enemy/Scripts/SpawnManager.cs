using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] points;

    public GameObject monsterPrefab;

    public int maxMonster = 10;

    public float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        points = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();

        StartCoroutine("SpawnMonster");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnMonster()
    {
        while(true)
        {
            int monsterCount = (int)GameObject.FindGameObjectsWithTag("Enemy").Length;

            if(monsterCount < maxMonster)
            {
                yield return new WaitForSeconds(spawnTime);

                int rand = Random.Range(1, points.Length);

                
                Instantiate(monsterPrefab, points[rand].position, points[rand].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }
}
