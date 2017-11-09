using UnityEngine;
using System.Collections;


public enum VehicleType { biped, simpleCar}
public class MainCameraScript : MonoBehaviourSingleton<MainCameraScript>
{

    private Transform pivotTarget;
    private Vector3 offset;
    Transform cameraTransform;
    Transform lastTransform;
    public float moveLag = 5;
    public float rotLag = 10;
    private float zDistanceFromTarget;
    public float zoomLag;
 

    void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        pivotTarget = this.transform; //temporary
    }


    public void SetCameraTargets(Transform trans, Vector3 offset)
    {
        pivotTarget = trans;
        lastTransform = trans;
        zDistanceFromTarget = offset.z;
    }

    public void LateUpdate()
    {

        if (cameraTransform.localPosition.z <= -3)
        {
            transform.position = Vector3.Lerp(transform.position, pivotTarget.position, moveLag * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, pivotTarget.rotation, rotLag * Time.deltaTime);
        }
        else
        {
            transform.position = pivotTarget.position;
            transform.rotation = pivotTarget.rotation;
        }


        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            zDistanceFromTarget += Input.GetAxis("Mouse ScrollWheel") * 20;
        }
        if (zDistanceFromTarget != transform.localPosition.z)
        {
            Vector3 temp = cameraTransform.localPosition;
            temp.z = Mathf.Lerp(cameraTransform.localPosition.z, zDistanceFromTarget, zoomLag * Time.deltaTime);
            if (temp.z > 0) temp.z = 0;
            offset = temp;
            cameraTransform.localPosition = temp;
        }

    }
}



