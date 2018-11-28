using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour {

    public GameObject loadFlag;
    public string levelPath;

	void Start () {
	}
	
	public void Update()
    {
        if(loadFlag.activeSelf)
        {
            SceneManager.LoadScene(levelPath);
        }

    }
}
