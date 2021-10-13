using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{

    float moveSpeed = 5.0f;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.position += new Vector3(h, 0, v) * moveSpeed * Time.deltaTime;
    }
}
