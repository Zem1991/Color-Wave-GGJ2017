using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCube : MonoBehaviour {

    [Header("Color Reaction")]
    public float sizeSmoothing = 0.1f;
    public float reactionGrowth = 0.4f;
    public float growthTime = 1.5f;
    public float reactionDelay = 0.8f;
    [Header("Interaction Parameters")]
    public float shrinkTime = 2f;
    public float shrinkSize = 0.2f;
    public float shrinkSmoothing = 0.1f;
    public float riseTime = 2f;
    public float jumpSpeed = 20f;
    public float growTime = 2f;
    public float growMax = 8f;
    [Header("Properties")]
    public CubeColor cubeColor = CubeColor.white;
    public LayerMask cubeLayers;


    public enum CubeColor
    {
        white = 0,
        green = 1,
        blue = 2,
        yellow = 3,
        red = 4,
    }

    public ColorCube[] neighbours;
    public bool canRise = true, canGrow = false, blockGrow = false;
    public Vector3 growDirection;
    public float growReach;

    private int colorReactionStrength = 0, actionReactionStrength = 0;
    private float lastShrink = -999f, lastRise = -999f, lastGrow = -999f, timeTime = 0f;
    private float colorReactionTime = -999f, actionReactionTime = -999f, lastWave = -1f;
    private bool shrink = false, rise = false, grow = false;
    private Vector3 startPosition;
    private CubeColor startColor;


    private int whiteLayer, redLayer, blueLayer, greenLayer, yellowLayer;

    //References
    private BoxCollider boxCollider;
    private Transform model;
    private MeshRenderer meshRenderer;
    private Material material;

    private Color color;
	
	void Start () {

        boxCollider = GetComponent<BoxCollider>();

        meshRenderer = GetComponentInChildren<MeshRenderer>();
        material = new Material(meshRenderer.material);
        meshRenderer.material = material;
        model = meshRenderer.transform;

        startPosition = transform.position;
        startColor = cubeColor;

        whiteLayer = LayerMask.NameToLayer("Cube");
        redLayer = LayerMask.NameToLayer("Red");
        blueLayer = LayerMask.NameToLayer("Blue");
        greenLayer = LayerMask.NameToLayer("Green");
        yellowLayer = LayerMask.NameToLayer("Yellow");
        string[] layers = new string[5] { "Cube", "Red", "Blue", "Green", "Yellow" };
        cubeLayers = LayerMask.GetMask(layers);

        UpdateColor();
    }

	void Update () {
        timeTime = Time.time;

        //Color reaction
        if (colorReactionStrength > 0 && timeTime > colorReactionTime + reactionDelay)
        {
            ColorReaction();
            colorReactionStrength = 0;
        }

        //Action reaction
        if (actionReactionStrength > 0 && timeTime > actionReactionTime + reactionDelay)
        {
            ActionReaction();
            actionReactionStrength = 0;
        }

        //Wave visual effect
        float lastReactionTime = Mathf.Max(actionReactionTime, colorReactionTime);
        if (timeTime < lastReactionTime + growthTime)
        {
            model.localScale = Vector3.Lerp(model.localScale, new Vector3(1f,1f,1f) *(1+reactionGrowth * (growthTime - (timeTime - lastReactionTime)) / growthTime), sizeSmoothing);
        }
        else if(!shrink)
            model.localScale = Vector3.Lerp(model.localScale, new Vector3(1f, 1f, 1f), sizeSmoothing);


        //Shrink effect
        if (shrink)
        {
            boxCollider.enabled = false;
            model.localScale = Vector3.Lerp(model.localScale, new Vector3(1f, 1f, 1f) * shrinkSize, shrinkSmoothing);
            if (cubeColor != CubeColor.red || timeTime > lastShrink + shrinkTime)
                shrink = false;
        }
        else
            boxCollider.enabled = true;

        if (cubeColor == CubeColor.red || cubeColor == CubeColor.white || (cubeColor == CubeColor.blue && !grow))
            transform.position = Vector3.Lerp(transform.position, startPosition, sizeSmoothing);
        if (cubeColor != CubeColor.blue || !grow)
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), sizeSmoothing);
        
        /*
        //Rise effect
        if (rise)
        {
            if (canRise)
            {
                //transform.position += new Vector3(0f, riseSpeed * Time.deltaTime, 0f);//Vector3.Lerp(transform.position, startPosition + new Vector3(0f, riseHeight, 0f), riseSmoothing);
                //if (transform.position.y - startPosition.y > riseHeight)
                    //transform.position = startPosition + new Vector3(0f, riseHeight, 0f);
            }
            if (cubeColor != CubeColor.green || timeTime > lastRise + riseTime)
                rise = false;

        }
        else
        {
            if (cubeColor == CubeColor.green)
                transform.position = Vector3.Lerp(transform.position, startPosition, riseSmoothing);
        }
        */

        //Grow effect
        if (grow)
        {
            if(canGrow)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, (growDirection.x==0?0.98f:0.99f), 1f)+ new Vector3(Mathf.Abs(growDirection.x), 0f, Mathf.Abs(growDirection.z)) *growReach, sizeSmoothing);
                float largestScale = Mathf.Max(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.z));
                transform.position = Vector3.Lerp(transform.position, startPosition+ growDirection*((growReach/2f)), sizeSmoothing);
            }
            if (cubeColor != CubeColor.blue || timeTime > lastGrow + growTime)
                grow = false;
        }

    }

    void ColorReaction()
    {
        for(int i = 0; i< neighbours.Length; i++)
        {
            if( neighbours[i] )
                neighbours[i].ColorHit(cubeColor, colorReactionStrength-1, lastWave);
        }
    }

    void ActionReaction()
    {
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i] && neighbours[i].cubeColor == cubeColor && neighbours[i].CanDoAction())
            {
                neighbours[i].ActionHit(actionReactionStrength - 1, lastWave);
            }
        }
    }

    public void ColorHit(CubeColor newColor, int strength, float newWave)
    {
        if (lastWave != newWave)
        {
            cubeColor = newColor;
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.position = startPosition;
            UpdateColor();
            colorReactionTime = timeTime;
            colorReactionStrength = strength;
            lastWave = newWave;
        }
    }

    public void ActionHit(int strength, float newWave)
    {
        if (lastWave != newWave)
        {
            actionReactionTime = timeTime;
            actionReactionStrength = strength;
            lastWave = newWave;

            switch(cubeColor)
            {
                case CubeColor.red:
                    lastShrink = timeTime;
                    shrink = true;
                    break;
                case CubeColor.green:
                    lastRise = timeTime;
                    rise = true;
                    break;
                case CubeColor.blue:
                    lastGrow = timeTime;
                    grow = true;
                    break;
            }
        }
    }

    void UpdateColor()
    {
        switch (cubeColor)
        {
            case CubeColor.white: color = Color.white; gameObject.layer = whiteLayer; break;
            case CubeColor.green: color = Color.yellow; gameObject.layer = greenLayer; break;
            case CubeColor.blue: color = Color.blue; gameObject.layer = blueLayer; break;
            case CubeColor.yellow: color = Color.yellow; gameObject.layer = yellowLayer; break;
            case CubeColor.red: color = Color.red; gameObject.layer = redLayer; break;
        }
        material.color = color;
    }

    public bool CanDoAction()
    {
        return (cubeColor!=CubeColor.white && !shrink && !grow && (cubeColor!=CubeColor.blue || canGrow));
    }

    public bool KeepActive()
    {
        if (grow)
        {
            lastGrow = timeTime;
            return true;
        }
        return false;
    }

    public void ResetColor()
    {
        cubeColor = startColor;
        UpdateColor();
    }
}
