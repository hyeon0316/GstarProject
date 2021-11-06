using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : LivingEntity
{
    public static Player inst = null;
    //인벤토리
    [SerializeField]
    public Inventory inventory;


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
    public bool isSkill;
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

    float qMana;
    float wMana;
    float eMana;
    float rMana;


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

    public bool isSkillQ;
    public bool isSkillW;
    public bool isSkillE;
    public bool isSkillR;
    public bool isSkillTP;

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
    public Quest questIng = null;

    public GameObject TrapTarget; //2페이지 이후 플레이어 머리 위에 해골표시

    public Text manaCaution;
    public Text coolCaution;
    private bool cautionTime;

    public Text levelText;
    int level;
    public float exp = 0;
    public float startingEx;
    public Slider exSlider; //경험치 슬라이더
    public Text exText;//경험치 표시

    public Text questTitleText;
    public Text questProText;
    public Rigidbody rigidbody;

    public GameObject mousePoint;
    public bool chest400;
    public bool chest500;
    public bool chest600;
    public bool chest700;
    private void Awake()
    {
        if (inst == null) // 싱글톤
        {
            inst = this;
        }
        rigidbody = GetComponent<Rigidbody>();
        isTalk = false;
        animator = GetComponentInChildren<Animator>();
        camera = Camera.main;
        attack = false;
        dP = startingDP;
        power = startingPower;
        exp = startingEx;

        time_Q = 5f;
        time_W = 30f;
        time_E = 12f;
        time_R = 30f;
        isGotM = false;
        time_Q_1 = 2f;
        isSkillQ = true;
        isSkillW = true;
        isSkillE = true;
        isSkillR = true;
        isSkillTP = true;
        tpDis = 5f;
        level = 1;
        qMana = 0;
        wMana = 0;
        eMana = 0;
        rMana = 0;
        isSkill = false;
        mousePoint.SetActive(false);

        chest400 = false;
        chest500 = false;
        chest600 = false;
        chest700 = false;

    }
    void Start()
    {
        QuestManager.inst.CheckQuest();
    }
    // Update is called once per frame
    void Update()
    {
        NpcS();
        if (!gameManager.isAction&&!isSkill)
        {
            GetPos();
            Move();
            Tp();
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
        //SetLevel();
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
                    mousePoint.SetActive(false);
                    if (hit.collider.gameObject.GetComponent<ObjData>().isNpc ==
                        true && Vector3.Distance(transform.position,
                        hit.collider.transform.position) < 5f)
                    {
                        Vector3 npcVector = transform.position - hit.collider.gameObject.transform.position;
                        npcVector.x = 0;
                        npcVector.z = 0;
                        npcVector.Normalize();
                        hit.collider.gameObject.transform.LookAt(this.transform.position);
                        Quaternion q = hit.collider.gameObject.transform.rotation;
                        q.x = 0;
                        q.z = 0;
                        hit.collider.gameObject.transform.rotation = q;
                        mousePoint.SetActive(false);
                        isMove = false;
                        animator.SetBool("isMove", false);
                        npcCam.SetActive(true);
                        
                        gameManager.Action(hit.collider.gameObject);
                    }
                    else if (Vector3.Distance(transform.position,
                        hit.collider.transform.position) < 7f)
                    {
                        isMove = false;
                        animator.SetBool("isMove", false);
                        mousePoint.SetActive(false);
                        gameManager.Action(hit.collider.gameObject);
                    }
                }
                else if(hit.collider.tag == "Chest" && Vector3.Distance(transform.position,
                        hit.collider.transform.position) < 7f)
                {
                    isMove = false;
                    animator.SetBool("isMove", false);
                    mousePoint.SetActive(false);
                    gameManager.Action(hit.collider.gameObject);
                    int check1 = hit.collider.gameObject.GetComponent<ObjData>().id;
                    ObjData hitObjData = hit.collider.gameObject.GetComponent<ObjData>();
                    if (check1 == 400 && !chest400)
                    {
                        inventory.AcquireItem(hitObjData._item[0]);
                        chest400 = true;
                    }
                    if (check1 == 500 && !chest500)
                    {
                        inventory.AcquireItem(hitObjData._item[0]);
                        inventory.AcquireItem(hitObjData._item[1]);
                        chest500 = true;
                    }
                    if (check1 == 600 && !chest600)
                    {
                        inventory.AcquireItem(hitObjData._item[0]);
                        inventory.AcquireItem(hitObjData._item[1]);
                        chest600 = true;
                    }
                    if (check1 == 700 && !chest700)
                    {
                        inventory.AcquireItem(hitObjData._item[0]);
                        inventory.AcquireItem(hitObjData._item[1]);
                        chest700 = true;
                    }
                }

            }
        }
    }

    void SetLevel()
    {
        levelText.text = string.Format("LV. {0}", level);
        exText.text = string.Format("{0}/{1}", exp, startingEx);
        exSlider.maxValue = startingEx;
        exSlider.value = exp;
    }
    void SetHpMp()
    {
        playerHpBarSlider.maxValue = startingHealth;
        playerHpBarSlider.value = health;
        playerMpBarSlider.maxValue = startingMana;
        playerMpBarSlider.value = mana;
        playerHpText.text = string.Format("{0}/{1}", health, startingHealth);
        playerMpText.text = string.Format("{0}/{1}", mana, startingMana);

        if (health < 0)
            health = 0;
        else if (mana < 0)      
            mana = 0;
        
    }
    private void OnTriggerEnter(Collider other)//아이템 획득
    {
        if (other.gameObject.tag.Equals("Item"))
        {
            inventory.AcquireItem(other.transform.GetComponent<ItemPickUp>().item);
            if (questIng != null)
            {
                questProText.text = "";
                string questText;
                questText = "";
                foreach (var obj in questIng.collectObjectives)
                {
                    obj.UpdateItemCount();
                    questText += obj.item.itemName + "\n" + obj.currentAmount + " / " + obj.amount + "\n";
                }
                questProText.text = questText;
                if (questIng.IsCompleteObjectives)
                    QuestManager.inst.questActionIndex = questIng.qusetComplte;
            }
            Destroy(other.gameObject);
        }
    }
    
    bool RectCheck(Vector3 _vector,float x1,float y1,float x2,float y2)
    {
        if(_vector.x>x1&&_vector.y<y1&&_vector.x<x2&&_vector.y>y2)
            return true;
        return false;
    }
    
    void GetPos()
    {
        if (Input.GetMouseButton(1))
        {
            if (Inventory.inventoryActivated && RectCheck(Input.mousePosition, 1230, 821, 1645, 251))
            {
                Debug.Log("inventory" + Input.mousePosition);
                return;
            }
            if (Information.informationActivated && RectCheck(Input.mousePosition, 215, 823, 629, 250))
            {
                Debug.Log("Information" + Input.mousePosition);
                return;
            }
            Debug.Log("userSlot" + Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                mousePoint.SetActive(true);
                mousePoint.transform.position = hit.point;
                    SetDestination(hit.point);
            }
        }
    }
    void Attack()
    {
        if ((Input.GetKey(KeyCode.A) || Input.GetMouseButton(0)) && Time.time >= SpawnProjectilesScript.inst.timeToFire)
        {
            if (Inventory.inventoryActivated && RectCheck(Input.mousePosition, 1230, 821, 1645, 251))
            {
                Debug.Log("inventory" + Input.mousePosition);
                return;
            }
            if (Information.informationActivated && RectCheck(Input.mousePosition, 215, 823, 629, 250))
            {
                Debug.Log("Information" + Input.mousePosition);
                return;
            }

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
    IEnumerator Caution(Text caution)//마나부족 혹은 스킬 재사용 경고
    {
        caution.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        caution.gameObject.SetActive(false);
        cautionTime = false;
    }

    void SkillQ()
    {
        if (Input.GetKeyDown(KeyCode.Q) && isSkillQ && mana >= qMana)
        {
            mana -= qMana;
            isSkillQ = false;
            StartCoroutine(SkillQCount(time_Q));
            coolTimeQ.GetComponent<CoolTime>().Reset_CoolTime(time_Q);
        }  
        else if(Input.GetKeyDown(KeyCode.Q) && !isSkillQ)
        {
            if (!cautionTime)
            {
                StartCoroutine(Caution(coolCaution));
                cautionTime = true;
                return;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (mana < qMana && !cautionTime)
            {
                StartCoroutine(Caution(manaCaution));
                cautionTime = true;
                return;
            }
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
            skillQFP.transform.forward = dir;
        }
        else
        {
            QQ = Instantiate(skill_Q, skillQFP.transform.position, Quaternion.identity);
        }
        animator.SetTrigger("SkillQ");
        isMove = false;
        animator.SetBool("isMove", false);
        isSkill = true;
        yield return new WaitForSeconds(1.2f);
        isSkill = false;
        
        QQ = Instantiate(skill_Q, skillQFP.transform.position, Quaternion.identity);
        QQ.transform.forward = skillQFP.transform.forward;
        yield return new WaitForSeconds(2.5f);
        Destroy(QQ.gameObject);
        yield return new WaitForSeconds(dealy - 4.5f);

        isSkillQ = true;
        coolTimeQ.GetComponent<CoolTime>().End_CoolTime();
    }

    void SkillW()
    {
        if (Input.GetKeyDown(KeyCode.W) && isSkillW && mana >= wMana)
        {          
            mana -= wMana;
            isSkillW = false;
            isGotM = true;
            skill_W.SetActive(true);
            StartCoroutine(SkillWCount(time_W));
            coolTimeW.GetComponent<CoolTime>().Reset_CoolTime(time_W);
        }
        else if (Input.GetKeyDown(KeyCode.W) && !isSkillW)
        {
            if (!cautionTime)
            {
                StartCoroutine(Caution(coolCaution));
                cautionTime = true;
                return;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (mana < wMana && !cautionTime)
            {
                StartCoroutine(Caution(manaCaution));
                cautionTime = true;
                return;
            }
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
       
        if (Input.GetKeyDown(KeyCode.E) && isSkillE && mana >= eMana)
        {
            mana -= eMana;
            isSkillE = false;
            StartCoroutine(SkillECount(time_E));
            coolTimeE.GetComponent<CoolTime>().Reset_CoolTime(time_E);
        }
        else if (Input.GetKeyDown(KeyCode.E) && !isSkillE)
        {
            if (!cautionTime)
            {
                StartCoroutine(Caution(coolCaution));
                cautionTime = true;
                return;
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (mana < eMana && !cautionTime)
            {
                StartCoroutine(Caution(manaCaution));
                cautionTime = true;
                return;
            }
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
        }
        isMove = false;
        animator.SetBool("isMove", false);
        animator.SetTrigger("SkillE");
        isSkill = true;
        yield return new WaitForSeconds(2f);
        
        QQ = Instantiate(skill_E, hit.point, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        isSkill = false;

        yield return new WaitForSeconds(3f);
        Destroy(QQ.gameObject);
        yield return new WaitForSeconds(dealy - 9f);

        isSkillE = true;
        coolTimeE.GetComponent<CoolTime>().End_CoolTime();
    }
    void SkillR()
    {      
        if (Input.GetKeyDown(KeyCode.R) && isSkillR && mana >= rMana)
        {            
            mana -= rMana;
            isSkillR = false;
            StartCoroutine(SkillRCount(time_R));
            coolTimeR.GetComponent<CoolTime>().Reset_CoolTime(time_R);
        }
        else if (Input.GetKeyDown(KeyCode.R) && !isSkillR)
        {
            if (!cautionTime)
            {
                StartCoroutine(Caution(coolCaution));
                cautionTime = true;
                return;
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (mana < rMana && !cautionTime)
            {
                StartCoroutine(Caution(manaCaution));
                cautionTime = true;
                return;
            }
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
            mousePoint.SetActive(false);
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
        TrapTarget.SetActive(false);

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
   
    public void TakeOffEffect(Item _item)
    {
        dP -= _item.itemDp;
        power -= _item.itemPower;
        startingHealth -= _item.startingHp;
        health -= _item.startingHp;
        startingMana -= _item.startingMp;
        mana -= _item.startingMp;
    }

    public void ExpPlus(float exp2)
    {
        exp += exp2;
        SetLevel();
        Debug.Log(exp);
    }
}

