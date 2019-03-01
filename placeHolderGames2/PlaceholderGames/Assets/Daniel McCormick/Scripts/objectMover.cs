﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectMover : MonoBehaviour
    {
    public float throwForce = 5;
    public float stepSmoothing = 3f;
    public float rayDistance;
    int layermask = 1 << 9;
    public GameObject targetObject;
    private GameObject holdPosition;
    private bool holding = false;
    Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        holdPosition = GameObject.Find("holdPosition");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        ray.origin = transform.position;
        ray.direction = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(ray, out hit, rayDistance, layermask, QueryTriggerInteraction.UseGlobal))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
            targetObject = hit.collider.gameObject;
        }
        else if (holding == false)
        {
            targetObject = null;
        }


        if(Input.GetMouseButton(0) && targetObject != null)
        {
            holding = true;
            targetObject.GetComponent<Rigidbody>().useGravity = false;
            //targetObject.transform.rotation = transform.rotation;
            targetObject.GetComponent<Rigidbody>().position = 
                new Vector3(Mathf.SmoothStep(targetObject.GetComponent<Rigidbody>().position.x, holdPosition.transform.position.x, stepSmoothing),
                Mathf.SmoothStep(targetObject.GetComponent<Rigidbody>().position.y, holdPosition.transform.position.y, stepSmoothing),
                Mathf.SmoothStep(targetObject.GetComponent<Rigidbody>().position.z, holdPosition.transform.position.z, stepSmoothing));
            if (Input.GetKey(KeyCode.E))
            {
                targetObject.layer = 0;
                targetObject.GetComponent<Rigidbody>().useGravity = true;
                targetObject.GetComponent<Rigidbody>().velocity = transform.forward * throwForce;
                targetObject = null;
            }
        }
        if (Input.GetMouseButtonUp(0) && targetObject != null)
        {
            targetObject.GetComponent<Rigidbody>().useGravity = true;
            holding = false;
        }
    }

}
