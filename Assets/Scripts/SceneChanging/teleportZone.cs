using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportZone : MonoBehaviour
{
    public int targetScene;
    public string sceneFrom;
    public GameObject teleportDataPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void teleport()
    {
        teleportData td = Instantiate(teleportDataPrefab).GetComponent<teleportData>();
        td.sceneToId = targetScene;
        td.sceneFrom = sceneFrom;
        td.startLoad();
    }
}
