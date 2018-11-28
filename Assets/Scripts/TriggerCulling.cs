using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerCulling : MonoBehaviour {

    private BoxCollider boxCollider;
    private ColorShooterController playerController;
    private bool active = true;

	void Start () {
        boxCollider = GetComponent<BoxCollider>();
        playerController = FindObjectOfType<ColorShooterController>();
    }
	
	void Update () {
        if (boxCollider.bounds.Contains(playerController.transform.position))
        {
            if (!active)
            {
                active = true;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (active)
            {
                active = false;
                for(int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

    }
}
