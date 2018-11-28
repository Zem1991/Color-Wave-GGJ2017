using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitReaction : MonoBehaviour {

    public ColorCube.CubeColor color = ColorCube.CubeColor.red;
    public int reactionStrength = 10;
    private bool hit = false;
    public float soundVolume = 1f;

    private MeshRenderer meshRenderer;
    private Material material;

    ColorShooterController playerController;

    // Use this for initialization
    void Start () {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        material = new Material(meshRenderer.material);
        meshRenderer.material = material;
        playerController = FindObjectOfType<ColorShooterController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if (!hit && !collision.gameObject.CompareTag("Player"))
        {
            ColorCube colorCube = collision.gameObject.GetComponent<ColorCube>();
            if (colorCube)
            {
                hit = true;
                colorCube.ColorHit(color, reactionStrength, Time.time);
            }
            playerController.audioSource.PlayOneShot(playerController.waveSound, soundVolume * Mathf.Clamp01( playerController.waveSoundDistance/Vector3.Distance(transform.position, playerController.transform.position)));
            Destroy(gameObject);
        }
    }

    public void UpdateColor(ColorCube.CubeColor cubeColor)
    {
        if (!material)
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            material = new Material(meshRenderer.material);
            meshRenderer.material = material;
        }
        Color color = Color.white;
        switch (cubeColor)
        {
            case ColorCube.CubeColor.white: color = Color.white; break;
            case ColorCube.CubeColor.green: color = Color.yellow; break;
            case ColorCube.CubeColor.blue: color = Color.blue; break;
            case ColorCube.CubeColor.yellow: color = Color.yellow; break;
            case ColorCube.CubeColor.red: color = Color.red; break;
        }
        material.color = color;
    }
}
