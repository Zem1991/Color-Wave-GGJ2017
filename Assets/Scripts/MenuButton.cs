using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public float waveSpeed = 2f;
    public float waveScale = 15f;
    public float selectedSpeed = 2.5f;
    public float selectedScale = 1.4f;
    public float selectedSmoothing = 0.2f;
    Image image;

    MenuController menuController;
    float randomWave;

    public enum Button {
        start = 0,
        instructions =1,
        credits =2,
        ggj =3,
        back = 4,
    }
    public Button button;

    private bool active = false;

    private Camera mainCamera;
    
    private float speed;

    public void OnPointerEnter(PointerEventData eventData)
    {
        active = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        active = false;
    }

    public void OnPointerClick(PointerEventData evd)
    {
        menuController.Action(button);
    }

    void Start () {
        mainCamera = Camera.main;
        image = GetComponent<Image>();
        speed = waveSpeed;
        randomWave = Random.value;
        menuController = FindObjectOfType<MenuController>();
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = Quaternion.Euler(0f, 0f, waveScale * Mathf.SmoothStep(0f, 1f, Mathf.PingPong((Time.time+randomWave) * speed, 1f)));
        if (active)
        {
            speed = Mathf.Lerp(speed, selectedSpeed, selectedSmoothing);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(selectedScale, selectedScale, selectedScale), selectedSmoothing);
        }
        else
        {
            speed = Mathf.Lerp(speed, waveSpeed, selectedSmoothing);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), selectedSmoothing);
        }
	}


}
