using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour
{
    [SerializeField]
    private float tumble;

    void Start()
    {
        GetComponentInParent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble * (Player.Instance.freezeTime ? 0 : 1);
    }
}