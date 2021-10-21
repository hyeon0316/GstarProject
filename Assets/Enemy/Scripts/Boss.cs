using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : LivingEntity
{
    private Text bossHpText;
    public GameObject bossHpBarPrefab;
    public Canvas enemyHpBarCanvas;
    public Slider bossHpBarSlider;

    public Vector3 hpBarOffset = new Vector3(0, 10f, 0);

    public LayerMask whatIsTarget; //추적대상 레이어

    private LivingEntity targetEntity;//추적대상
    private NavMeshAgent pathFinder; //경로 계산 AI 에이전트

    private Animator bossAnimator;

    public float damage = 20f; //공격력
    public float attackDelay = 2f; //공격 딜레이
    private float lastAttackTime; //마지막 공격 시점
    private float dist; //추적대상과의 거리

    public Transform tr;

    private float attackRange = 3f;

    private bool hasTarget
    {
        get
        {
            //추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            //그렇지 않다면 false
            return false;
        }
    }

    private bool canMove;
    private bool canAttack;

    private void Awake()
    {
        //게임 오브젝트에서 사용할 컴포넌트 가져오기
        pathFinder = GetComponent<NavMeshAgent>();
        bossAnimator = GetComponent<Animator>();
    }


    void Start()
    {
        SetHpBar();
        //게임 오브젝트 활성화와 동시에 AI의 탐지 루틴 시작
        StartCoroutine(UpdatePath());
        tr = GetComponent<Transform>();
    }

    void SetHpBar()
    {
        enemyHpBarCanvas = GameObject.Find("EnemyHpBarCanvas").GetComponent<Canvas>();
        GameObject bossHpBar = Instantiate<GameObject>(bossHpBarPrefab, enemyHpBarCanvas.transform);

        var _hpbar = bossHpBar.GetComponent<BossHpBar>();
        bossHpBarSlider = _hpbar.GetComponent<Slider>();
        bossHpText = GameObject.Find("EnemyHpBarCanvas").transform.GetChild(0).transform.GetChild(2).GetComponent<Text>();
        bossHpText.text = string.Format("{0}", health);
    }
    // Update is called once per frame
    void Update()
    {
        bossAnimator.SetBool("CanMove", canMove);
        bossAnimator.SetBool("CanAttack", canAttack);

        if (hasTarget)
        {
            //추적 대상이 존재할 경우 거리 계산은 실시간으로 해야하니 Update()
            dist = Vector3.Distance(tr.position, targetEntity.transform.position);
        }       
    }

 
    //추적할 대상의 위치를 주기적으로 찾아 경로 갱신
    private IEnumerator UpdatePath()
    {
        //살아 있는 동안 무한 루프
        while (!dead)
        {
            if (hasTarget)
            {
                Attack();
            }
            else
            {
                //추적 대상이 없을 경우, AI 이동 정지
                pathFinder.isStopped = true;
                canAttack = false;
                canMove = false;

                //반지름 20f의 콜라이더로 whatIsTarget 레이어를 가진 콜라이더 검출하기
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);

                //모든 콜라이더를 순회하면서 살아 있는 LivingEntity 찾기
                for (int i = 0; i < colliders.Length; i++)
                {
                    //콜라이더로부터 LivingEntity 컴포넌트 가져오기
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    //LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아 있다면
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        //추적 대상을 해당 LivingEntity로 설정
                        targetEntity = livingEntity;

                        //for문 루프 즉시 정지
                        break;
                    }
                }
            }
            //0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    //추적 대상과의 거리에 따라 공격 실행
    public virtual void Attack()
    {
        //자신이 사망X, 추적 대상과의 거리이 공격 사거리 안에 있다면
        if (!dead && dist < attackRange)
        {
            pathFinder.isStopped = true;

            //공격 반경 안에 있으면 움직임을 멈춘다.
            canMove = false;

            //추적 대상 바라보기
            this.transform.LookAt(targetEntity.transform);

            //최근 공격 시점에서 attackDelay 이상 시간이 지나면 공격 가능
            if (lastAttackTime + attackDelay <= Time.time)
            {
                canAttack = true;
            }

            //공격 반경 안에 있지만, 딜레이가 남아있을 경우
            else
            {
                canAttack = false;
            }
        }

        //공격 반경 밖에 있을 경우 추적하기
        else
        {
            canMove = true;
            canAttack = false;
            //계속 추적
            pathFinder.isStopped = false; //계속 이동
            pathFinder.SetDestination(targetEntity.transform.position);
        }
    }

    //유니티 애니메이션 이벤트로 휘두를 때 데미지 적용시키기
    public void OnDamageEvent()
    {
        //공격 대상을 지정할 추적 대상의 LivingEntity 컴포넌트 가져오기
        LivingEntity attackTarget = targetEntity.GetComponent<LivingEntity>();

        //공격 처리(플레이어에게)
        attackTarget.OnDamage(damage);

        //최근 공격 시간 갱신
        lastAttackTime = Time.time;     
    }


    //데미지를 입었을 때 실행할 처리(재정의)
    public override void OnDamage(float damage)
    { 
        //LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage); //base, 부모클래스에 접근하는 기능   

        bossHpBarSlider.value = health;
        bossHpText.text = string.Format("{0}", health);
    }

    //사망 처리
    public override void Die()
    {
        bossHpBarSlider.gameObject.SetActive(false);
        //다른 AI를 방해하지 않도록 자신의 모든 콜라이더를 비활성화
        Collider[] enemyColliders = GetComponents<Collider>();
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        //AI추적을 중지하고 네비메쉬 컴포넌트를 비활성화
        pathFinder.isStopped = true;
        pathFinder.enabled = false;


        canMove = false;
        canAttack = false;

        //사망 애니메이션 재생
        bossAnimator.SetTrigger("doDie");

        //LivingEntity의 DIe()를 실행하여 기본 사망 처리 실행
        base.Die();
    }

    //bossHpBarSlider 활성화
    protected override void OnEnable()
    {
        //LivingEntity의 OnEnable() 실행(상태초기화)
        base.OnEnable();

        bossHpBarSlider.gameObject.SetActive(true);
        bossHpBarSlider.maxValue = startingHealth;
        //체력 슬라이더의 값을 현재 체력값으로 변경
        bossHpBarSlider.value = health;
        
    }
}