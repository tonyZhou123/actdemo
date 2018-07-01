using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {
    public Transform targetTr;
    public float dist = 10.0f;
    public float height = 3.0f;
    public float dampTrace = 20.0f;

    private Transform tr;
	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();
        tr.LookAt(targetTr.position);
    }
	
	// Update is called once per frame
	void LateUpdate () {
     
        tr.position = Vector3.Lerp(tr.position
            , targetTr.position
            - Vector3.forward * dist
            + Vector3.up * height
            , Time.deltaTime * dampTrace
            );

        //tr.LookAt(targetTr.position);
	}
}
