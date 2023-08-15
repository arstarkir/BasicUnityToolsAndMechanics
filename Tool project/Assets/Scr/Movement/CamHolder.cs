using UnityEngine;

public class CamHolder : MonoBehaviour
{
    public Transform cam;
    public Transform mCam;
    void Update()
    {
        cam.position = transform.position;
        transform.rotation = mCam.rotation;
    }
}
