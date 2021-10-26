using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillQ : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject playerC;
    float dmg;
    float startingDmg = 30f;
    void Awake()
    {
        playerC = GameObject.Find("PlayerC");
        dmg = playerC.GetComponent<Player>().power + startingDmg;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("[Enemy] , 데미지 :" + dmg);
            OnDamageEvent(other.gameObject);
        }
        if (other.tag == "Boss")
        {
            Debug.Log("[BOss] , 데미지 :" + dmg);
            OnDamageEvent(other.gameObject);
        }
    }

    public void OnDamageEvent(GameObject targetEntity)
    {
        //공격 대상을 지정할 추적 대상의 LivingEntity 컴포넌트 가져오기
        LivingEntity attackTarget = targetEntity.GetComponent<LivingEntity>();

        //공격 처리(플레이어에게)
        attackTarget.OnDamage(dmg);
    }
}
