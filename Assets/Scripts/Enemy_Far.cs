using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Far : MonoBehaviour
{
    private Transform playerTr;
    private Transform enemyTr;

    public Transform bulletPos;
    public GameObject bullet;
    private NavMeshAgent nvAgent;
    // Start is called before the first frame update
    void Start()
    {
        enemyTr = gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        
        nvAgent = gameObject.GetComponent<NavMeshAgent>();
        StartCoroutine("BulletCo");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerLookAt();
        
        nvAgent.destination = playerTr.position;
        
    }

    private IEnumerator BulletCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            BulletStart();
        }
    }
    void PlayerLookAt()
    {
        transform.LookAt(playerTr);
    }
    void BulletStart()
    {
        Instantiate(bullet, bulletPos.transform.position, bulletPos.transform.rotation);
    }
}
