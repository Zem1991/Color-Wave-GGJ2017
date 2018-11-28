using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public MeshRenderer[] neonRenderer;
    public Material onMaterial, offMaterial;
    public GameObject onLight;

    private Transform spawnPosition;
    private bool active = false;
    private Material[] neonMaterial;
    private Checkpoint[] checkpoints;
    private ColorShooterController playerController;

    // Use this for initialization
    void Start () {
        onLight.SetActive(false);
        spawnPosition = transform.Find("SpawnPosition");
        neonMaterial = new Material[neonRenderer.Length];
        for (int i = 0; i < neonRenderer.Length; i++)
        {
            neonRenderer[i].material = offMaterial;
            neonMaterial[i] = offMaterial;
        }
        checkpoints = FindObjectsOfType<Checkpoint>();
        playerController = FindObjectOfType<ColorShooterController>();
    }

    void OnTriggerStay(Collider collider)
    {
        ColorShooterController playerController = collider.GetComponent<ColorShooterController>();
        if (playerController && collider.gameObject.layer == gameObject.layer && playerController.GetCheckpoint()!= spawnPosition.position)
        {
            playerController.Checkpoint(spawnPosition.position,spawnPosition.rotation);
            active = true;
            playerController.audioSource.PlayOneShot(playerController.checkpointSound);
            onLight.SetActive(true);
            for (int i = 0; i < neonRenderer.Length; i++)
            {
                neonRenderer[i].material = onMaterial;
            }
            for(int i = 0; i < checkpoints.Length; i++)
            {
                if(checkpoints[i].transform != transform)
                    checkpoints[i].Deactivate();
            }

        }
    }

    public void Deactivate()
    {
        onLight.SetActive(false);
        active = false;
        for (int i = 0; i < neonRenderer.Length; i++)
        {
            neonRenderer[i].material = offMaterial;
        }
    }

}
