using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private float minSpawnRadius;
    [SerializeField] private float maxSpawnRadius;
    [SerializeField] private float targetRadius;
    [SerializeField] private float spawnFrequency;

    [SerializeField] private Meteor meteorPrefab;

    private float timer;

    private List<Vector3> spawnedMeteors = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.Instance.freezeTime) return;
        timer -= Time.deltaTime;
        if (timer > 0) return;
        timer = spawnFrequency;

        Vector3 impactLocation = CalculateImpactLocation();

        Vector3 spawnPosition = CalculateSpawnPosition(impactLocation);
        if (spawnPosition == Vector3.zero) return;
        Meteor meteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
        meteor.Initialize(impactLocation);
        spawnedMeteors.Add(spawnPosition);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(Player.Instance.transform.position, targetRadius);
    //    foreach (Vector3 pos in spawnedMeteors)
    //    {
    //        Gizmos.DrawSphere(pos, 1f);
    //    }
    //}

    private Vector3 CalculateImpactLocation()
    {
        Vector3 impactLocation = Player.Instance.transform.position + Player.Instance.transform.forward * 2;
        Vector2 offset = Random.insideUnitCircle * targetRadius;
        impactLocation.x += offset.x;
        impactLocation.z += offset.y;
        return impactLocation;
    }

    private Vector3 CalculateSpawnPosition(Vector3 impactLocation)
    {
        RaycastHit hit;
        Debug.DrawLine(impactLocation, Planet.Instance.transform.position, Color.blue, 5f);
        if (Physics.Raycast(impactLocation, (Planet.Instance.transform.position - impactLocation).normalized, out hit, 10f, 1 << 3))
        {
            Vector3 spawnAngle = Random.insideUnitSphere;
            if (Vector3.Dot(hit.normal, spawnAngle) < 0)
            {
                spawnAngle *= -1;
            }
            Vector3 spawnPosition = impactLocation + spawnAngle * Random.Range(minSpawnRadius, maxSpawnRadius);
            Debug.DrawLine(impactLocation, spawnPosition, Color.red, 5f);
            return spawnPosition;
        }
        return Vector3.zero;
        return Random.onUnitSphere * Random.Range(minSpawnRadius, maxSpawnRadius);
    }
}
