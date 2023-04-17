using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] private float meteorSpeed;
    [SerializeField] private GameObject ImpactVFXPrefab;
    [SerializeField] private GameObject ImpactIndicatorPrefab;
    private Vector3 targetPosition;

    private bool initialized;

    private Rigidbody rbody;

    private GameObject SpawnedIndicator;

    private float timeToImpact;
    private float timer;
    private MeshRenderer indicatorMesh;

    private void Start()
    {
        rbody = GetComponentInChildren<Rigidbody>();
        timer = 0;
    }

    public void Initialize(Vector3 target)
    {
        targetPosition = target;
        initialized = true;
        transform.LookAt(target);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (target - transform.position).normalized, out hit, float.MaxValue, 1 << 3))
        {
            SpawnedIndicator = Instantiate(ImpactIndicatorPrefab, hit.point, Quaternion.identity);
            SpawnedIndicator.transform.up = hit.normal;
            SpawnedIndicator.transform.position += SpawnedIndicator.transform.up * 0.01f;
            float distanceToImpact = (hit.point - transform.position).magnitude;
            timeToImpact = distanceToImpact / meteorSpeed;
            indicatorMesh = SpawnedIndicator.GetComponent<MeshRenderer>();
        } else
        {
            Destroy(this.gameObject);
        }
        
    }

    private void Update()
    {
        if (!initialized) return;
        rbody.velocity = transform.forward * meteorSpeed * (Player.Instance.freezeTime ? 0 : 1);
        float numSegments = indicatorMesh.material.GetFloat("_SegmentCount");
        float segPerSec = numSegments / timeToImpact;
        
        indicatorMesh.material.SetFloat("_RemovedSegments", numSegments - segPerSec * timer);
        timer += Time.deltaTime * (Player.Instance.freezeTime ? 0 : 1);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioController.Instance.PlaySFX("meteor");
        GameObject vfx = Instantiate(ImpactVFXPrefab, collision.GetContact(0).point, Quaternion.identity);
        vfx.transform.forward = collision.GetContact(0).normal;
        Destroy(this.gameObject);
        Destroy(SpawnedIndicator);
    }
}
