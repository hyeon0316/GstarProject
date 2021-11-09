using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{

    // Start is called before the first frame update
    public static SoundManager inst = null;
    public AudioClip[] bgList;
    public AudioSource bgSound;
    private void Awake()
    {
        if (inst == null)
            inst = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        
    }
    private void OnSceneLoaded(Scene arg0,LoadSceneMode arg1)
    {
        for(int i=0;i<bgList.Length;i++)
        {
            if (arg0.name == bgList[i].name)
            {
                BgSoundPlay(bgList[i]);
                Debug.Log(bgList[i].name);
            }
        }
        
    }
    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.1f;
        bgSound.Play();
    }
    public void SFXPlay(string sfxName,AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(go, clip.length);
    
    
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
