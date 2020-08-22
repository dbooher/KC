using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class teleportData : MonoBehaviour
{
    public string sceneFrom = "" ;
    public int sceneToId = 0 ;
    public bool loadingStarted = false;
    public bool loadingFinished = false;
    AsyncOperation loadingOperation;
    public GameObject playerPrefab;
    // Start is called before the first frame update

    private void Start()
    { 
        DontDestroyOnLoad(gameObject);
    }


    private void Update()
    {
        if(loadingStarted)
        {
            if(loadingOperation.isDone)
            {
                loadingFinished = true;
                teleportTarget[] targets = GameObject.FindObjectsOfType<teleportTarget>();
                foreach(teleportTarget t in targets)
                {
                    if(t.sceneFrom == sceneFrom)
                    {
                        if(GameObject.Find("Player"))
                        {
                            Destroy(GameObject.Find("Player"));
                        }
                        GameObject p = Instantiate(playerPrefab, t.transform.position, Quaternion.identity);
                        p.name = "Player";
                    }
                }
                Finish();
            }
        }
    }

    void Finish()
    {
        Destroy(gameObject);
    }

    public void startLoad()
    {
        loadingOperation = SceneManager.LoadSceneAsync(sceneToId);
        loadingStarted = true;
    }
}
