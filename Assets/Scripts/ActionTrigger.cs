using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrigger : MonoBehaviour {

    int reactionStrength;
    ColorShooterController playerController;
    Rigidbody rb;

    private void Start()
    {
        playerController = GetComponentInParent<ColorShooterController>();
        rb = playerController.GetComponent<Rigidbody>();
        reactionStrength = playerController.reactionStrength;
    }

    void OnTriggerStay(Collider collider)
    {
        ColorCube colorCube = collider.gameObject.GetComponent<ColorCube>();
        if (collider.gameObject.layer == gameObject.layer && colorCube)
        {
            if (!colorCube.KeepActive())
            {
                if (colorCube && colorCube.CanDoAction())
                {
                    colorCube.ActionHit(reactionStrength, Time.time);
                    if (colorCube.cubeColor == ColorCube.CubeColor.yellow)
                        playerController.ReverseGravity();
                    if(colorCube.cubeColor == ColorCube.CubeColor.green && rb.velocity.y <= 1f && colorCube.transform.position.y < playerController.transform.position.y)
                    {
                        rb.velocity = new Vector3(rb.velocity.x, playerController.highJump, rb.velocity.z);
                        playerController.audioSource.PlayOneShot(playerController.highJumpSound);
                    }
                    if(colorCube.cubeColor == ColorCube.CubeColor.blue)
                        playerController.audioSource.PlayOneShot(playerController.blueSound, Mathf.Clamp01(playerController.waveSoundDistance / Vector3.Distance(colorCube.transform.position, playerController.transform.position)));
                    else if (colorCube.cubeColor == ColorCube.CubeColor.red)
                        playerController.audioSource.PlayOneShot(playerController.redSound, Mathf.Clamp01(playerController.waveSoundDistance / Vector3.Distance(colorCube.transform.position, playerController.transform.position)));
                }
            }
        }


    }

}
