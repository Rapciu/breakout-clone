using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] GameObject sceneLoader;

    SceneLoader sceneLoaderComp;

    //int blockAmount;

    public void CheckBlocks()
    {
        if (GameObject.FindGameObjectsWithTag("BreakableBlock").Length == 0)
        {
            sceneLoaderComp.LoadNextScene();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //sceneLoaderComp = FindObjectOfType<SceneLoader>();
        sceneLoaderComp = sceneLoader.GetComponent<SceneLoader>();

        //blockAmount = GameObject.FindGameObjectsWithTag("BreakableBlock").Length;
    }

    // Update is called once per frame
    void Update()
    {
        CheckBlocks();
    }
}
