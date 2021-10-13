using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject haBarPrefab;
    public Vector3 hpBarOffset = new Vector3(-0.5f, 2.4f, 0);

    public Canvas enemyHpBarCanvas;
    public Slider enemyHpBarSlider;

    private Transform playerTr;
    private Transform enemyTr;
    private NavMeshAgent nvAgent;

    private Rigidbody rigid;

    private Animator anim;

    bool isEnemyNav;
    bool isAttack;


    public BoxCollider attackRange;

     
    // Start is called before the first frame update
    void Awake()
    {
        enemyTr = gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();

        nvAgent = gameObject.GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        rigid = gameObject.GetComponent<Rigidbody>();
        Invoke("EnemyNavStart", 1);
    }

    private void Start()
    {
        SetHpBar();
    }
    // Update is called once per frame
    void Update()
    {
        if (isEnemyNav)
            nvAgent.destination = playerTr.position;
        Attacking();
        //FreezeVelocity();
    }

    void SetHpBar()
    {
        enemyHpBarCanvas = GameObject.Find("EnemyHpBarCanvas").GetComponent<Canvas>();
        GameObject hpBar = Instantiate<GameObject>(haBarPrefab, enemyHpBarCanvas.transform);

        var _hpBar = hpBar.GetComponent<EnemyHpBar>();
        _hpBar.enemyTr = this.gameObject.transform;
        _hpBar.offset = hpBarOffset;
    }
    void EnemyNavStart()
    {
        isEnemyNav = true;
        anim.SetBool("isWalk", true);
    }

    void EnemyDie()
    {
        isEnemyNav = false;
        nvAgent.enabled = false;
        anim.SetTrigger("doDie");
        Destroy(gameObject, 3);
    }

    void Attacking()
    {
        float targetRadius = 1.5f;
        float targetRange = 2f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
            targetRadius,
            transform.forward,
            targetRange,
            LayerMask.GetMask("Player"));

        if(rayHits.Length >0 && !isAttack)
        {
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack()
    {
        isEnemyNav = false; //정지한 다음 수행
        isAttack = true;
        anim.SetBool("isAttack", true);

        yield return new WaitForSeconds(0.2f);
        attackRange.enabled = true;

        yield return new WaitForSeconds(1f);
        attackRange.enabled = false;

        yield return new WaitForSeconds(0.5f);
        isEnemyNav = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    void FreezeVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
  
}
