using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Planet.GetRandomPointOnPlanet();
        transform.LookAt(Planet.Instance.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
