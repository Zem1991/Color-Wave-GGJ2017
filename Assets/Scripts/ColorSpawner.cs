using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSpawner : MonoBehaviour {

    public ColorCube.CubeColor color;
    public float spawnInterval = 3f;
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    public float bulletSpeed = 1f;

    private float lastSpawn = 0f;

    private MeshRenderer meshRenderer;
    private Material material;

    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        material = new Material(meshRenderer.material);
        meshRenderer.material = material;
        UpdateColor(color);
    }

    void Update()
    {
        if (Time.time > lastSpawn + spawnInterval)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);

            HitReaction bulletScript = bullet.GetComponent<HitReaction>();
            if (bulletScript)
            {
                bulletScript.color = color;
                bulletScript.UpdateColor(color);
                bulletScript.soundVolume = 0.5f;
            }

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb)
            {
                bulletRb.velocity =  bulletPosition.forward * bulletSpeed;
                bulletRb.useGravity = true;
            }
            lastSpawn = Time.time;
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
