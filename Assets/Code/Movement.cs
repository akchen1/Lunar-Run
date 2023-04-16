using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Transform target;

    //public void Move(Vector2 direction)
    //{
    //    Vector3 mappedDirection = new Vector3(direction.x, 0, direction.y);


    //    RaycastHit hit;
    //    Vector3 gravityDirection = (target.position - transform.position);
    //    Debug.DrawRay(transform.position, gravityDirection, Color.red);
    //    //Debug.Log(gravityDirection);
    //    //transform.up = -gravityDirection;
    //    float xRot = transform.eulerAngles.x;
    //    float zRot = transform.eulerAngles.z;
    //    //if (mappedDirection.x != 0)
    //    //{
    //        zRot = -Mathf.Atan2(-gravityDirection.x, -gravityDirection.y) * Mathf.Rad2Deg;
    //        zRot = (zRot + 360) % 360;

    //    //}
    //        xRot = Mathf.Atan2(-gravityDirection.z, -gravityDirection.y) * Mathf.Rad2Deg;
    //        //xRot = (xRot + 360) % 360;
    //        Debug.Log(xRot);
        
    //    //transform.rotation = Quaternion.Euler(xRot, 0, zRot);
    //    transform.eulerAngles = new Vector3(xRot, 0, zRot);
    //    if (Physics.Raycast(transform.position, (target.position - transform.position).normalized, out hit, 1f, 1 << 3))
    //    {
    //        Vector3 projectedPlane = Vector3.ProjectOnPlane(mappedDirection, hit.normal);
    //        Debug.DrawRay(hit.point, projectedPlane, Color.cyan);
    //        transform.position = hit.point;
    //    }

    //    transform.position += (transform.right * direction.x + transform.forward * direction.y).normalized * 4 * Time.deltaTime;
    //    //transform.RotateAround(target.position, mappedDirection, 20 * Time.deltaTime);
    //}


    public void Move(Vector2 direction)
    {
        transform.RotateAround(target.position, transform.right * direction.y, 40 * Time.deltaTime);
        transform.RotateAround(transform.position, transform.up * direction.x, 100 * Time.deltaTime);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, (target.position - transform.position).normalized, out hit, 1f, 1 << 3))
        {
            
            transform.position = hit.point;
        }
    }

}
