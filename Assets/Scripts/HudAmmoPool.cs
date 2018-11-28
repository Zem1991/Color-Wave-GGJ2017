using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudAmmoPool : MonoBehaviour {
    public ColorCube.CubeColor color;

    private ColorShooterController playerController;

    private GameObject label;
    private bool active = false, cooldown = false;
    private Color startColor;
    private Image image;

	void Start () {
        playerController = FindObjectOfType<ColorShooterController>();
        label = transform.parent.Find("Label").gameObject;
        image = GetComponent<Image>();
        startColor = image.color;
    }
	
	// Update is called once per frame
	void Update () {
        float scale = 1f;
        switch (color)
        {
            case ColorCube.CubeColor.red:
                active = playerController.canShootRed;
                cooldown = playerController.redCooldown;
                scale = playerController.redAmmo / playerController.maxAmmo;
                break;
            case ColorCube.CubeColor.green:
                active = playerController.canShootGreen;
                cooldown = playerController.greenCooldown;
                scale = playerController.greenAmmo / playerController.maxAmmo;
                break;
            case ColorCube.CubeColor.blue:
                active = playerController.canShootBlue;
                cooldown = playerController.blueCooldown;
                scale = playerController.blueAmmo / playerController.maxAmmo;
                break;
            case ColorCube.CubeColor.yellow:
                active = playerController.canShootYellow;
                cooldown = playerController.yellowCooldown;
                scale = playerController.yellowAmmo / playerController.maxAmmo;
                break;
        }

        transform.localScale = new Vector3(1f, scale, 1f);

        label.SetActive(active);
        if (active)
        {
            if (cooldown)
            {
                image.color = Color.Lerp(Color.yellow, new Color(255f, 120f, 0f), Time.time % 1f);
                Debug.Log(Time.time % 1f);
            }
            else
                image.color = startColor;
        }
        else
            image.color = Color.gray;

    }
}
