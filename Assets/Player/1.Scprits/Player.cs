﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : LivingEntity
{
    public static Player inst = null;
    //인벤토리
    [SerializeField]
    private Inventory inventory;


    [SerializeField]
    private Transform chBody;

    public Slider playerHpBarSlider;
    public Slider playerMpBarSlider;
    public Text playerHpText;
    public Text playerMpText;
    public bool townS;
    private Camera camera;
    public GameObject npcCam;
    public static bool slotCountClear = false;


    public bool isMove;
    private Vector3 destination;
    public Animator animator;
    public bool attack;
    public float attackSpeed;
    public GameObject firePoint;
    public GameObject skillQFP;

    public GameObject skill_Q;
    public GameObject skill_W;
    public GameObject skill_E;
    public GameObject skill_R;

    public float exp = 0;

    public GameObject skill_TP;
    private float startingDP = 0;
    private float startingPower = 20;

    public float dP;
    public float power;

    private float time_Q;
    private float time_W;
    private float time_E;
    private float time_R;

    private float time_Q_1;

    bool isSkillQ;
    bool isSkillW;
    bool isSkillE;
    bool isSkillR;
    bool isSkillTP;

    bool isGotM;

    float skill1Time = 10f;
    // Start is called before the first frame update
    public LayerMask npcLayer;
    public GameManager gameManager;
    public bool isTalk;
    public GameObject coolTimeQ;
    public GameObject coolTimeW;
    public GameObject coolTimeE;
    public GameObject coolTimeR;
    public GameObject coolTimeF;

    private GameObject tempSkill1;
    private GameObject tempSkill2;
    private float time_current;
    private float time_start;
    RaycastHit hit1;
    private int layerMask;
    float tpDis;
    public Quest questIng;
    private void Awake()
    {
        if (inst == null) // 싱글톤
        {
            inst = this;
        }

        isTalk = false;
        animator = GetComponentInChildren<Animator>();
        camera = Camera.main;
        attack = false;
        dP = startingDP;
        power = startingPower;

        time_Q = 5f;
        time_W = 30f;
        time_E = 7.5f;
        time_R = 30f;
        isGotM = false;
        time_Q_1 = 2f;
        isSkillQ = true;
        isSkillW = true;
        isSkillE = true;
        isSkillR = true;
        isSkillTP = true;
        tpDis = 5f;

    }
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
       
        NpcS();
        if (!gameManager.isAction)
        {
            GetPos();
            Move();
            Tp();
            if (!Inventory.inventoryActivated && !Information.informationActivated)
                Attack();

            if (SceneManager.GetActiveScene().name != "Town")
            {
                SkillQ();
                SkillW();
                SkillE();
                SkillR();
            }
        }
        SetHpMp();
    }
    void NpcS()
    {
        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, npcLayer))
            {
                if (hit.collider.tag == "NPC")
                {
                    if (hit.collider.gameObject.GetComponent<ObjData>().isNpc ==
                        true && Vector3.Distance(transform.position,
                        hit.collider.transform.position) < 5f)
                    {
                        Vector3 npcVector = transform.position - hit.collider.gameObject.transform.position;
                        npcVector.x = 0;
                        npcVector.z = 0;
                        npcVector.Normalize();
                        Debug.Log(npcVector);
                        hit.collider.gameObject.transform.LookAt(this.transform.position);
                        Quaternion q = hit.collider.gameObject.transform.rotation;
                        q.x = 0;
                        q.z = 0;
                        hit.collider.gameObject.transform.rotation = q;
                        isMove = false;
                        animator.SetBool("isMove", false);
                        npcCam.SetActive(true);
                        gameManager.Action(hit.collider.gameObject);
                    }
                    else if (Vector3.Distance(transform.position,
                        hit.collider.transform.position) < 5f)
                    {
                        gameManager.Action(hit.collider.gameObject);
                    }
                }

            }
        }
    }



    void SetHpMp()
    {
        playerHpBarSlider.maxValue = startingHealth;
        playerHpBarSlider.value = health;
        playerMpBarSlider.maxValue = startingMana;
        playerMpBarSlider.value = mana;
        playerHpText.text = string.Format("{0}/{1}", health, startingHealth);
        playerMpText.text = string.Format("{0}/{1}", mana, startingMana);
    }
    private void OnTriggerEnter(Collider other)//아이템 획득
    {
        if (other.gameObject.tag.Equals("Item"))
        {
            Debug.Log("dd");
            inventory.AcquireItem(other.transform.GetComponent<ItemPickUp>().item);
            Destroy(other.gameObject);
        }
    }
    void GetPos()
    {
        if (Input.GetMouseButton(1))
        {

            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, npcLayer))
            {
                SetDestination(hit.point);
            }
        }
    }
    void Attack()
    {
        if ((Input.GetKey(KeyCode.A) || Input.GetMouseButton(0)) && Time.time >= SpawnProjectilesScript.inst.timeToFire)
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                var dir = hit.point - animator.transform.position;
                dir.y = 0f;
                animator.transform.forward = dir;
                firePoint.transform.forward = dir;
            }
            isMove = false;
            animator.SetBool("isMove", false);
            animator.SetTrigger("attack");

            SpawnProjectilesScript.inst.timeToFire = Time.time + attackSpeed / SpawnProjectilesScript.inst.effectToSpawn.GetComponent<ProjectileMoveScript>().fireRate;
            SpawnProjectilesScript.inst.SpawnVFX();
        }
    }
    void SkillQ()
    {

        if (Input.GetKeyDown(KeyCode.Q) && isSkillQ)
        {

            mana -= 100;
            isSkillQ = false;
            StartCoroutine(SkillQCount(time_Q));
            coolTimeQ.GetComponent<CoolTime>().Reset_CoolTime(time_Q);
        }
    }
    IEnumerator SkillQCount(float dealy)
    {
        RaycastHit hit;
        GameObject QQ;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            var dir = hit.point - animator.transform.position;
            dir.y = 0;
            animator.transform.forward = dir;
            isMove = false;
            skillQFP.transform.forward = dir;
            QQ = Instantiate(skill_Q, skillQFP.transform.position, Quaternion.identity);
            QQ.transform.forward = skillQFP.transform.forward;
            animator.SetBool("isMove", false);
        }
        else
        {
            QQ = Instantiate(skill_Q, skillQFP.transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(2.5f);
        Destroy(QQ.gameObject);
        yield return new WaitForSeconds(dealy - 2.5f);

        isSkillQ = true;
        coolTimeQ.GetComponent<CoolTime>().End_CoolTime();
    }
    void SkillW()
    {
        if (Input.GetKeyDown(KeyCode.W) && isSkillW)
        {
            mana -= 100;
            isSkillW = false;
            isGotM = true;
            skill_W.SetActive(true);
            StartCoroutine(SkillWCount(time_W));
            coolTimeW.GetComponent<CoolTime>().Reset_CoolTime(time_W);
        }
    }
    IEnumerator SkillWCount(float dealy)
    {
        yield return new WaitForSeconds(3f);
        skill_W.SetActive(false);
        isGotM = false;
        yield return new WaitForSeconds(dealy);

        isSkillW = true;
        coolTimeW.GetComponent<CoolTime>().End_CoolTime();
    }

    void SkillE()
    {
        if (Input.GetKeyDown(KeyCode.E) && isSkillE)
        {
            mana -= 100;
            isSkillE = false;
            StartCoroutine(SkillECount(time_E));
            coolTimeE.GetComponent<CoolTime>().Reset_CoolTime(time_E);
        }
    }
    IEnumerator SkillECount(float dealy)
    {
        RaycastHit hit;
        GameObject QQ;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            var dir = hit.point - animator.transform.position;
            dir.y = 0;
            animator.transform.forward = dir;
            isMove = false;
            animator.SetBool("isMove", false);
            QQ = Instantiate(skill_E, hit.point, Quaternion.identity);
        }
        else
        {
            QQ = Instantiate(skill_E, transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(5f);
        Destroy(QQ.gameObject);
        yield return new WaitForSeconds(dealy - 2.5f);

        isSkillE = true;
        coolTimeE.GetComponent<CoolTime>().End_CoolTime();
    }
    void SkillR()
    {
        if (Input.GetKeyDown(KeyCode.R) && isSkillR)
        {
            mana -= 100;
            isSkillR = false;
            StartCoroutine(SkillRCount(time_R));
            coolTimeR.GetComponent<CoolTime>().Reset_CoolTime(time_R);
        }
    }
    IEnumerator SkillRCount(float dealy)
    {
        RaycastHit hit;
        GameObject QQ;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            var dir = hit.point - animator.transform.position;
            dir.y = 0;
            animator.transform.forward = dir;
            isMove = false;
            animator.SetBool("isMove", false);
            QQ = Instantiate(skill_R, hit.point, Quaternion.identity);
        }
        else
        {
            QQ = Instantiate(skill_R, transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(5f);
        Destroy(QQ.gameObject);
        yield return new WaitForSeconds(dealy - 2.5f);

        isSkillR = true;
        coolTimeR.GetComponent<CoolTime>().End_CoolTime();
    }
    void Tp()
    {
        if (Input.GetKey(KeyCode.F) && isSkillTP)
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                tempSkill1 = ObjectPoolManager.inst.GetObjectFromPool("TP", transform.position, Quaternion.Euler(-90, 0, 0));
                var dir = hit.point - animator.transform.position;
                dir.y = 0;
                animator.transform.forward = dir;
                layerMask = 1 << 10;
                Vector3 anipo = animator.transform.position;
                anipo.y += 1f; 
                if (Physics.Raycast(anipo, animator.transform.forward, out hit1, tpDis, layerMask))
                {
                    transform.position += dir.normalized * hit1.distance;
                }
                else
                {
                    transform.position += dir.normalized * tpDis;
                }
                tempSkill2 = ObjectPoolManager.inst.GetObjectFromPool("TP", transform.position, Quaternion.Euler(-90, 0, 0));
                isMove = false;
                animator.SetBool("isMove", false);
                StartCoroutine(SkillTPCount());
                coolTimeF.GetComponent<CoolTime>().Reset_CoolTime(1.5f);
            }
        }
    }
    IEnumerator SkillTPCount()
    {
        isSkillTP = false;
        yield return new WaitForSeconds(1.5f);
        isSkillTP = true;
        coolTimeF.GetComponent<CoolTime>().End_CoolTime();
        ObjectPoolManager.inst.ReturnObjectToPool("TP", tempSkill1);
        ObjectPoolManager.inst.ReturnObjectToPool("TP", tempSkill2);
    }

    private void Move()
    {
        attack = animator.GetBool("attack");
        if (isMove && !attack)
        {
            var dir = destination - transform.position;

            dir.y = 0f;

            transform.position += dir.normalized * Time.deltaTime * 10f;

            animator.transform.forward = dir;
            firePoint.transform.forward = dir;
        }

        if (GetDistance(transform.position.x, transform.position.z, destination.x, destination.z) < 0.1f)
        {
            isMove = false;
            animator.SetBool("isMove", false);
        }
    }
    float GetDistance(float x1, float y1, float x2, float y2)
    {
        // [과정1] 종점(x2, y2) - 시작점(x1, y1)
        float width = x2 - x1;
        float height = y2 - y1;

        // [과정2] 거리(크기)의 스칼라값을 구하기 위해 피타고라스 정리 사용
        float distance = width * width + height * height;
        distance = Mathf.Sqrt(distance);

        return distance;
    }

    private void SetDestination(Vector3 dest)
    {
        destination = dest;
        isMove = true;
        animator.SetBool("isMove", true);
    }
    public override void Die()
    {
        //
        Boss.inst.TrapTarget.SetActive(false);

        SceneManager.LoadScene("Town");
        health = 50; //수정해야함
        this.transform.position = new Vector3(-1.7f, 2f, 26);
        Debug.Log("you Die");
        base.Die();
    }
    public override void OnDamage(float damage)
    {
        //
        if (!isGotM)
        {
            damage -= dP;
            base.OnDamage(damage);
        }
    }

    public void CheckPotion(Item _item)
    {
        if (_item.itemName == "파워엘릭서")
        {
            health = startingHealth;
            mana = startingMana;
        }
        if (_item.itemName == "엘릭서")
        {
            health += (startingHealth / 2);
            mana += (startingMana / 2);
        }
    }
    public void HealHp(Item _item) //체력포션 사용
    {
        
        if (startingHealth <= health)
        {
            return;
        }
        CheckPotion(_item);
        slotCountClear = true;
        health += _item.itemHp;
        if (health > startingHealth)
            health = startingHealth;
    }

    public void HealMp(Item _item)//마나포션 사용
    {
        if (startingMana <= mana)
        {
            return;
        }
        CheckPotion(_item);
        slotCountClear = true;
        mana += _item.itemMp;
        if (mana > startingMana)
            mana = startingMana;
    }

    public void EquipEffect(Item _item)
    {
        dP += _item.itemDp;
        power += _item.itemPower;
        startingHealth += _item.startingHp;
        health += _item.startingHp;
        startingMana += _item.startingMp;
        mana += _item.startingMp;
    }
    public void ExpPlus(float exp2)
    {
        exp += exp2;
    }
    public void TakeOffEffect(Item _item)
    {
        dP -= _item.itemDp;
        power -= _item.itemPower;
        startingHealth -= _item.startingHp;
        health -= _item.startingHp;
        startingMana -= _item.startingMp;
        mana -= _item.startingMp;
    }
}

/*
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform chBody;
    [SerializeField]
    private Transform cameraArm;


    Rigidbody myRigid;

    Vector3 playerPosition;
    float hAxis, vAxis;

    static float moveSpeed = 10f;

    void Start()
    {
        myRigid = chBody.GetComponent<Rigidbody>();
        //Rigidbody객체를 만들고 거기에 큐브 오브젝트에 달려있는 
        //Rigidbody컴포넌트를 넣음

    }

    void Update()
    {
        Movement();
        LookAround();
        //cameraArm.transform.position = new Vector3(chBody.transform.position.x, chBody.transform.position.y+6, chBody.transform.position.z-6);

    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
    public void Movement()
    {

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        Vector2 moveInput = new Vector2(hAxis, vAxis);
        if (moveInput.magnitude != 0)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            chBody.forward = moveDir;
            transform.position += moveDir * Time.deltaTime * moveSpeed;
        }
    }
}

 * */