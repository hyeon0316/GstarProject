using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMa : MonoBehaviour
{
    public string sceneName;
    public Vector3 spwanVector;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = spwanVector;
            Player.inst.isMove = false;
            Player.inst.animator.SetBool("isMove", false);
            SceneManager.LoadScene(sceneName);
        }
    }
}
