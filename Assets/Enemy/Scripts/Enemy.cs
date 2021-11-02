using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI; //AI, 네비게이션 시스템 관련 코드 가져오기

public class Enemy : LivingEntity
{

    public GameObject[] _item;
    public float[] _dropP;
    //HpBarUi 추가 변수
    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset = new Vector3(-0.5f, 2.4f, 0);

    public Canvas enemyHpBarCanvas;
    public Slider enemyHpBarSlider;

    public LayerMask whatIsTarget; //추적대상 레이어

    private LivingEntity targetEntity;//추적대상
    private NavMeshAgent pathFinder; //경로 계산 AI 에이전트

    private bool isFreeze = false; //피격당했을때 잠깐 멈추기

    /*public ParticleSystem hitEffect; //피격 이펙트
    public AudioClip deathSound;//사망 사운드
    public AudioClip hitSound; //피격 사운드
    */

    private Animator enemyAnimator;
    //private AudioSource enemyAudioPlayer; //오디오 소스 컴포넌트

    public float damage = 20f; //공격력
    public float attackDelay = 1f; //공격 딜레이
    private float lastAttackTime; //마지막 공격 시점
    private float dist; //추적대상과의 거리

    public Transform tr;

    public float attackRange = 2.3f;

    public float LookatSpeed = 1f; //0~1

    //추적 대상이 존재하는지 알려주는 프로퍼티
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

    void SetNaviStop(bool val)
    {
        pathFinder.isStopped = val;
    }

    private void Awake()
    {
        //게임 오브젝트에서 사용할 컴포넌트 가져오기
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        //enemyAudioPlayer = GetComponent<AudioSource>();
    }
    private void OnDisable()
    {
        
    }
 

    void Start()
    {
        SetHpBar();
        //게임 오브젝트 활성화와 동시에 AI의 탐지 루틴 시작
        StartCoroutine(UpdatePath());
        tr = GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        enemyAnimator.SetBool("CanMove", canMove);
        enemyAnimator.SetBool("CanAttack", canAttack);

        if (hasTarget)
        {
            //추적 대상이 존재할 경우 거리 계산은 실시간으로 해야하니 Update()
            dist = Vector3.Distance(tr.position, targetEntity.transform.position);
        }
    }

    void SetHpBar()
    {
        enemyHpBarCanvas = GameObject.Find("EnemyHpBarCanvas").GetComponent<Canvas>();
        GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, transform.position, Quaternion.identity, enemyHpBarCanvas.transform);
        var _hpbar = hpBar.GetComponent<EnemyHpBar>();
        
        _hpbar.enemyTr = this.gameObject.transform;
        _hpbar.offset = hpBarOffset;
        
        enemyHpBarSlider = _hpbar.GetComponent<Slider>(); //체력감소시키기위해 getcomponent(게임 실행 시 연결이 안되었던 문제 해결)
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
                SetNaviStop(true);
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
            SetNaviStop(true);

            //공격 반경 안에 있으면 움직임을 멈춘다.
            canMove = false;

            //추적 대상 바라보기
            Vector3 dir = targetEntity.transform.position - this.transform.position;

            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(dir), Time.deltaTime * LookatSpeed);

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
            if(!isFreeze)
                SetNaviStop(false);//계속 이동
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
        /*사망하지 않을 상태에서만 피격 효과 재생
        if (!dead)
        {
            //공격 받은 지점과 방향으로 피격 효과 재생
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            //피격 효과음 재생
            enemyAudioPlayer.PlayOnShot(hitSound);
        }
        */       
       
        StartCoroutine(HitStop());

        //LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage); //base, 부모클래스에 접근하는 기능

        enemyHpBarSlider.value = health;
    }

    IEnumerator HitStop()
    {
        isFreeze = true;
        SetNaviStop(true);
        canMove = false;
        canAttack = false;
        enemyAnimator.SetTrigger("Hit");
        yield return new WaitForSeconds(0.75f);
        isFreeze = false; 
    }

    //사망 처리
    public override void Die()
    {
        enemyHpBarSlider.gameObject.SetActive(false);
        //다른 AI를 방해하지 않도록 자신의 모든 콜라이더를 비활성화
        Collider[] enemyColliders = GetComponents<Collider>();
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        //AI추적을 중지하고 네비메쉬 컴포넌트를 비활성화
        SetNaviStop(true);
        pathFinder.enabled = false;

        canMove = false;
        canAttack = false;

        //사망 애니메이션 재생       
        enemyAnimator.ResetTrigger("Hit");
        enemyAnimator.SetTrigger("doDie");

        /*//사망 효과음 재생
        enemyAudioPlayer.PlayOnShot(deathSound);
        */

        //LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        Invoke("DestroyEnemy", 2f);
        
    }

    //enemyHpBarSlider 활성화
    protected override void OnEnable()
    {

        //LivingEntity의 OnEnable() 실행(상태초기화)

        base.OnEnable();

        //체력 슬라이더 활성화
        enemyHpBarSlider.gameObject.SetActive(true);
        //체력 슬라이더의 최댓값을 기본 체력값으로 변경
        enemyHpBarSlider.maxValue = startingHealth;
        //체력 슬라이더의 값을 현재 체력값으로 변경
        enemyHpBarSlider.value = health;
    }

    private void DestroyEnemy()
    {
        GameObject[] newItem;
        newItem = new GameObject[_item.Length];
        for (int i=0;i<_item.Length;i++)
        {
            float k = Random.Range(0, 100);
            if(_dropP[i]>k)
                newItem[i] = Instantiate(_item[i], transform.position, Quaternion.identity);
        }
        GameObject.Destroy(gameObject);
    }
}