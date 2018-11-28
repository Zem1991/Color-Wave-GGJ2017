using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BigBadBurguerController : MonoBehaviour {

    public float moveSmoothing = 0.1f;
    public Vector3 playerRelativePosition = new Vector3(0f, 200f, 200f);
    public Transform targetBurguerEnd;
    public float distanceGoToEnd = 200f;
    public float distanceEnd = 20f;
    public string endScene;

    private ColorShooterController playerController;


    // Use this for initialization
    void Start () {
        playerController = FindObjectOfType<ColorShooterController>();

    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(playerController.transform.position, targetBurguerEnd.position) < distanceGoToEnd)
        {
            transform.position = Vector3.Lerp(transform.position, targetBurguerEnd.position, moveSmoothing);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetBurguerEnd.rotation, moveSmoothing);
            if (Vector3.Distance(playerController.transform.position, targetBurguerEnd.position) < distanceEnd)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene(endScene);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, playerController.transform.position + playerRelativePosition, moveSmoothing);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(playerController.transform.position - transform.position), moveSmoothing);
        }
	}
}
