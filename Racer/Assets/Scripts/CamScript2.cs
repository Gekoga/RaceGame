using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript2 : MonoBehaviour {
    public Transform target;

    public float distance = 10.0F;
    public float height = 5.0F;

    public float heightDamping = 2.0F;
    public float rotationDamping = 3.0F;
    public float damping = 6.0F;
    public bool smooth = true;

    [Header("cameraArray")]
    public int currentCam;
    public GameObject[] cameraArray;
    public bool carOne;
    public Camera cam;
    void Start() {
        currentCam++;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!target)
        {
            return;
        }

        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.fixedDeltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.fixedDeltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        //transform.LookAt(target);

        if (target != null)
        {
            if (smooth)
            {
                Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * damping);

            }
            else {
                transform.LookAt(target);
            }
        }
    }
    void Update()
    {
        if (currentCam > cameraArray.Length - 1)
            currentCam = 0;
        else if (currentCam < 0)
            currentCam = cameraArray.Length - 1;


        if (carOne)
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
                currentCam++;
        }

        else {
            if (Input.GetKeyDown(KeyCode.Q))
                currentCam++;

        }
        for (int i = 0; i < cameraArray.Length; i++)
        {
            if (currentCam == i)
            {
                cameraArray[i].SetActive(true);
            }
            else
                cameraArray[i].SetActive(false);


        }
    }
}

