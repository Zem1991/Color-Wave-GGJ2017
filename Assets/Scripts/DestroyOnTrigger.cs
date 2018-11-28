using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour {

    CubeManager cubeManager;

    private void Start()
    {
        cubeManager = FindObjectOfType<CubeManager>();
    }

    void OnTriggerEnter(Collider collider)
    {
        ColorShooterController playerController = collider.GetComponent<ColorShooterController>();
        if (playerController)
        {
            playerController.RespawnPlayer();
            cubeManager.ResetAllCubes();
        }
        else if( !collider.CompareTag("Player") )
            Destroy(collider.gameObject);
    }
}
