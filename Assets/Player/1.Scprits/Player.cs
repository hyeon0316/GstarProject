using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : LivingEntity
{
    //인벤토리
    [SerializeField]
    private Inventory inventory;

    //public static Player inst;
    [SerializeField]
    private Transform chBody;

    public Slider playerHpBarSlider;
    public Text playerHpText;

    private Camera camera;

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

    private float startingDP = 0;
    private float startingPower = 0;

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

    private void Awake()
    {
        /*if (!inst) // 싱글톤
        {
            inst = this;
        }
        */
        isTalk = false;
        animator = GetComponentInChildren<Animator>();
        camera = Camera.main;
        attack = false;
        dP = startingDP;
        power = startingPower;
        power = 0;
        time_Q = 5f;
        time_W = 30f;
        time_E = 5f;
        time_R = 30f;
        isGotM = false;
        time_Q_1 = 2f;
        isSkillQ = true;
        isSkillW = true;
        isSkillE = true;
        isSkillR = true;
        isSkillTP = true;

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
            Attack();

            SkillQ();
            SkillW();
            SkillE();
        }
       // SetHpMp();
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
                        hit.collider.transform.position) < 10)
                    {
                        isMove = false;
                        animator.SetBool("isMove", false);
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
        playerHpText.text = string.Format("{0}/{1}", health, startingHealth);
    }
    private void OnTriggerEnter(Collider other)//아이템 획득
    {
        if (other.gameObject.tag.Equals("Item"))
        {
            inventory.AcquireItem(other.transform.GetComponent<ItemPickUp>().item);
            Destroy(other.gameObject);
        }
    }

    void GetPos()
    {
        if (Input.GetMouseButton(1))
        {
            
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit,npcLayer))
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
            isSkillQ = false;
            StartCoroutine(SkillQCount(time_Q));
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

    }
    void SkillW()
    {
        if (Input.GetKeyDown(KeyCode.W) && isSkillW)
        {
            isSkillW = false;
            isGotM = true;
            skill_W.SetActive(true);
            StartCoroutine(SkillWCount(time_W));
        }
    }
    IEnumerator SkillWCount(float dealy)
    {
        yield return new WaitForSeconds(3f);
        skill_W.SetActive(false);
        isGotM = false;
        yield return new WaitForSeconds(dealy);

        isSkillW = true;

    }
    void SkillE()
    {
        if (Input.GetKeyDown(KeyCode.E) && isSkillE)
        {
            isSkillE = false;
            StartCoroutine(SkillECount(time_E));
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

    }
    void Tp()
    {
        if (Input.GetKey(KeyCode.F) && isSkillTP)
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                var dir = hit.point - animator.transform.position;
                dir.y = 0;
                animator.transform.forward = dir;
                transform.position += dir.normalized * 2f;
                isMove = false;
                animator.SetBool("isMove", false);
                StartCoroutine(SkillTPCount());
            }
        }
    }
    IEnumerator SkillTPCount()
    {
        isSkillTP = false;
        yield return new WaitForSeconds(1f);
        isSkillTP = true;

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
        Debug.Log("you Die");
        base.Die();
    }
    public override void OnDamage(float damage)
    {
        //
        damage -= dP;
        base.OnDamage(damage);
    }

    public void HealHp(float plusHealth) //체력포션 사용
    {
        health += plusHealth;
        if (health > startingHealth)
            health = startingHealth;
    }

    public void HealMp(float plusMana)//마나포션 사용
    {
        mana += plusMana;
        if (mana > startingMana)
            mana = startingMana;
    }

    public void EquipEffect(Item _item)
    {
        dP += _item.itemDp;
        power += _item.itemPower;
	}

    public void TakeOffEffect(Item _item)
    {
        dP -= _item.itemDp;
        power -= _item.itemPower;
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