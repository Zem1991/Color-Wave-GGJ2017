using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    public ColorCube.CubeColor color;
    public float spinSpeed = 5f;

    // Use this for initialization
    void Update()
    {
        transform.Rotate(new Vector3(0f,1f,0f), spinSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collider)
    {
        ColorShooterController playerController = collider.GetComponent<ColorShooterController>();
        if(playerController && collider.gameObject.layer == gameObject.layer)
        {
            playerController.hasWeapon = true;
            switch (color)
            {
                case ColorCube.CubeColor.blue: playerController.canShootBlue = true; break;
                case ColorCube.CubeColor.green: playerController.canShootGreen = true; break;
                case ColorCube.CubeColor.red: playerController.canShootRed = true; break;
                case ColorCube.CubeColor.yellow: playerController.canShootYellow = true; break;
            }
            playerController.ammoType = color;
            playerController.UpdateWeapon();
            Destroy(gameObject);
        }
    }
}
