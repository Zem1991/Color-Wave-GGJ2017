using UnityEngine;
using System.Collections;

public class DestroyOverTime : MonoBehaviour {
    public Vector2 lifeTime = new Vector2(1f, 1f);
    public AnimationCurve sizeOverTime = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));
    private float startTime;
    private float lifeAmmount;

    void Start()
    {
        startTime = Time.time;
        lifeAmmount = Random.Range(lifeTime.x, lifeTime.y);
    }

	void Update () {
        transform.localScale = new Vector3(1f, 1f, 1f) * sizeOverTime.Evaluate((Time.time-startTime)/ lifeAmmount);
        if (Time.time > startTime + lifeAmmount)
            Destroy(gameObject);
	}
}
