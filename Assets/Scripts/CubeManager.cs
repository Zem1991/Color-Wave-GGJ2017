using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class CubeManager : MonoBehaviour {

    ColorCube[] allCubes;

    public void Start()
    {
        allCubes = FindObjectsOfType<ColorCube>();
    }
    public void ResetAllCubes()
    {
        for (int i = 0; i < allCubes.Length; i++)
        {
            allCubes[i].ResetColor();
        }
    }

    /*
    // Use this for initialization
    public void SetupCubes()
    {
        allCubes = FindObjectsOfType<ColorCube>();

        for (int i = 0; i < allCubes.Length; i++)
        {
            Setup(allCubes[i]);
        }
    }
	

    public void Setup(ColorCube cube)
    {
        Undo.RecordObject(cube, "Reset cube rotation");
        cube.transform.rotation = Quaternion.identity;

        string[] layers = new string[5] { "Cube", "Red", "Blue", "Green", "Yellow" };
        Undo.RecordObject(cube, "Set cube collision layers");
        cube.cubeLayers = LayerMask.GetMask(layers);

        Undo.RecordObject(cube, "Changed neighbours size");
        cube.neighbours = new ColorCube[6];
        ColorCube[] allCubes = FindObjectsOfType<ColorCube>();

        cube.canRise = true;
        cube.canGrow = false;
        cube.blockGrow = false;
        cube.growDirection = new Vector3(0f, 0f, 0f);

        for (int i = 0; i < 6; i++)
        {
            Vector3 neighbourPosition = new Vector3(0f, 0f, 0f);
            switch (i)
            {
                case 0: neighbourPosition = new Vector3(0f, 1f, 0f); break;
                case 1: neighbourPosition = new Vector3(0f, -1f, 0f); break;
                case 2: neighbourPosition = new Vector3(1f, 0f, 0f); break;
                case 3: neighbourPosition = new Vector3(-1f, 0f, 0f); break;
                case 4: neighbourPosition = new Vector3(0f, 0f, 1f); break;
                case 5: neighbourPosition = new Vector3(0f, 0f, -1f); break;
            }

            bool hit = false;
            for (int j = 0; j < allCubes.Length; j++)
            {
                BoxCollider otherCollider = allCubes[j].GetComponent<BoxCollider>();
                //float distance = Vector3.Distance((transform.position + neighbourPosition), allCubes[j].transform.position);
                if (otherCollider.bounds.Contains(cube.transform.position + neighbourPosition))
                {
                    hit = true;
                    Undo.RecordObject(cube, "Added neighbour");
                    cube.neighbours[i] = (allCubes[j]);

                }
                else if (Physics.CheckSphere(cube.transform.position + neighbourPosition, 0.1f, cube.cubeLayers, QueryTriggerInteraction.Ignore))
                {
                    hit = true;
                }
            }
            if (hit)
            {
                if (i == 0)
                {
                    Undo.RecordObject(cube, "Changed can rise");
                    cube.canRise = false;
                }
            }
            else
            {
                //Set up block growth (blue power)
                if (i > 1 && !cube.blockGrow)
                {
                    if (cube.canGrow)
                    {
                        Undo.RecordObject(cube, "Changed block grow");
                        cube.blockGrow = true;
                        Undo.RecordObject(cube, "Changed can grow");
                        cube.canGrow = false;
                    }
                    else
                    {
                        Undo.RecordObject(cube, "Changed can grow");
                        cube.canGrow = true;
                        Undo.RecordObject(cube, "Changed grow direction");
                        cube.growDirection = neighbourPosition;
                        Ray ray = new Ray(transform.position, cube.growDirection);
                        RaycastHit rayHit = new RaycastHit();
                        bool growHit = Physics.Raycast(ray, out rayHit, cube.growMax, cube.cubeLayers, QueryTriggerInteraction.Ignore);
                        if (growHit)
                        {
                            if (rayHit.distance < 1f)
                            {
                                Undo.RecordObject(cube, "Changed can grow");
                                cube.canGrow = false;
                                Undo.RecordObject(cube, "Changed block grow");
                                cube.blockGrow = true;
                            }
                            else
                            {
                                Undo.RecordObject(cube, "Changed grow reach");
                                cube.growReach = rayHit.distance + 1.5f;
                            }
                        }
                        else
                        {
                            Undo.RecordObject(cube, "Changed grow reach");
                            cube.growReach = cube.growMax - 0.5f;
                        }
                    }
                }
            }
        }
    }
    */

    
}
