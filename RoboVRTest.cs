using UnityEngine;
using System.Collections;
using Microsoft;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

public class RoboVRTest : MonoBehaviour {

	OVRTracker mTracker = null;

	int test = 0;

	SerialPort[] mSerialPorts;

	bool revers = false;


	Byte[] mByteArray = new byte[2];

	// Use this for initialization
	void Start () {

		string[] ports = SerialPort.GetPortNames();

		Debug.Log("The following serial ports were found:");
		Debug.Log(SerialPort.GetPortNames().Count());

		mSerialPorts = new SerialPort[SerialPort.GetPortNames ().Count ()];

		int lPortId = 0;
		// Display each port name to the console.
		foreach(string port in ports)
		{
			Debug.Log(port);
			mSerialPorts [lPortId] = new SerialPort (port, 9600);
		}
		Byte[] mByteArray = new byte[2] {90, 90};
		foreach(SerialPort lPort in mSerialPorts) {
			lPort.Open ();
			lPort.Write (mByteArray, 0, 2);
		}
		mTracker = new OVRTracker ();
	}
	
	// Update is called once per frame
	void Update () {
		track ();
	}

	void track() {
		

		if (mTracker != null) {
			if (mTracker.isPresent && mTracker.isPositionTracked) {
				OVRPose lPose = mTracker.GetPose ();
				Quaternion lOrientation = lPose.orientation;

				double x = lOrientation.x;
				double y = lOrientation.y;
				double z = lOrientation.z;
//			double angle = lOrientation.w;

				double RotY = Math.Atan (x / z);
				double RotX = Math.Atan (y / z);

				if (RotX >= 180) {
					RotX = -180;
				} else if (RotX <= -180) {
					RotX = 180;
				}

				if (RotY >= 180) {
					RotY = -180;
				} else if (RotY <= -180) {
					RotY = 180;
				}

				if (RotX >= 90) {
					mByteArray [0] = 180;
				} else if (RotX <= -90) {
					mByteArray [0] = 0;
				} else {
					mByteArray [0] = (Byte) Math.Round(RotX + 90);
				}
				if (RotY >= 90) {
					mByteArray [1] = 180;
				} else if (RotY <= -90) {
					mByteArray [1] = 0;
				} else {
					mByteArray [1] = (Byte) Math.Round(RotY + 90);
				}

				//test
				if (test < 180 && !revers) {
					test++;
				}
				if (test > 0 && revers) {
					test--;
				}
				if (test == 180 || test == 0) {
					revers = !revers;
				}

				mByteArray [0] = (Byte)test;
				mByteArray [1] = (Byte)(180 - test);

				foreach(SerialPort lPort in mSerialPorts) {
					lPort.Write (mByteArray, 0, 2);
				}
			}
		} else {
			Debug.Log ("HMD not found");
		}
	}
}
