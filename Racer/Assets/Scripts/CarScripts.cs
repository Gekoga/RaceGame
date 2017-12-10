using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
    
}

public class CarScripts : MonoBehaviour
{

    [System.Serializable]
    public struct PlayerStats
    {
        //valuebles for counting the amount of laps
        public int currentLap;
        public int currentCheckpoint;
        public int currentScore;
        public int currentTotal;
    }
    public PlayerStats stats;

    //turning the car back up again
    [Header("Info")]
    public bool carOne;
    public bool carTwo;
    public Rigidbody car;

    //controlling the car
    [Header("Other car funtions")]
    public Vector3 centerOfMassModification;
    float motor;
    float steering;
    public GameControler controller;

    [Header("Display speed")]
    float player;
    public Text textPlayer;

    [Header("Display laps")]
    public Text lapsText;
    public Text checkpointText;

    [Header("Display Score")]
    public int points;
    public Text pointsText;

    [Header("Regulate speed")]
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    
    // finds the corresponding visual wheel
    // correctly applies the transform

	public void Start() {
       GetComponent<Rigidbody>().centerOfMass += centerOfMassModification;
	}

    public void ApplyLocalPositionToVisuals(WheelCollider collider) {
        if (collider.transform.childCount == 0) {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate() {
        //motor for car one
        if (carOne && controller.startable == true) {
            motor = maxMotorTorque * Input.GetAxis("Vertical");
            steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        }
        //motor for car two
       else if (carTwo && controller.startable == true)
        {
            motor = maxMotorTorque * Input.GetAxis("WS");
            steering = maxSteeringAngle * Input.GetAxis("AD");

        }
        //steering
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
        //turn car one back up again 
		if (Input.GetKeyDown(KeyCode.KeypadEnter) && carOne) {
			transform.rotation = Quaternion.Euler (new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, 0));
			car.angularVelocity = Vector3.zero;
			car.velocity = Vector3.zero;
            points -= 10;
			}
        //turn car two bakc up again
		if (Input.GetKeyDown(KeyCode.R) && !carOne) {
			transform.rotation = Quaternion.Euler (new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, 0));
			car.angularVelocity = Vector3.zero;
			car.velocity = Vector3.zero;
            points -= 10;
		}		
    }

    public void Update() {
        //display speed
        player = car.velocity.magnitude * 3.6f;
        textPlayer.text = player.ToString("00.0" + "KM/H");

        //display laps
        lapsText.text = "Laps : " + stats.currentLap;
        checkpointText.text = "Checkpoint : " + stats.currentCheckpoint;
        pointsText.text = "Points : " + points;

        //Finish
        if (stats.currentLap == 3) {
            controller.ButtonMainMenu();
        }

    }

}