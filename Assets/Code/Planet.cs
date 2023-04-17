using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public static Planet Instance { get; private set; }

    private static float planetScale = 10f;
    //private static float planetScale = 20f;

    private static float radius = 0.990762f;
    //private static float radius = 0.5f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //transform.localScale = new Vector3(planetScale, planetScale, planetScale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector3 GetRandomPointOnPlanet()
    {
        return Random.onUnitSphere * planetScale * radius;
    }
}
