using UnityEngine;
using System.Collections;
using System;

public class UIRotation : MonoBehaviour
{
    public enum RotationDir
    {
        X, Y, Z
    };

    public float angle = -300f;

    public RotationDir rotationDir=RotationDir.Z;

    private Vector3 v;
	// Use this for initialization
	void Start () {

        switch (rotationDir)
        {
            case RotationDir.X:
                v = new Vector3(1, 0, 0);
                break;
            case RotationDir.Y:
                v = new Vector3(0, 1, 0);
                break;
            case RotationDir.Z:
                v = new Vector3(0, 0,1);
                break;
        
        }
	}
	

    void Update() {

        transform.Rotate(v, angle * Time.deltaTime);
    }
}
