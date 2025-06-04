using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    Transform attackTransform;
    Transform targetTransform;
    Transform objectTransform;
    float cameraposx = 0f;
    float cameraposy = 0f;
    float cameraposz = -10f;
    public float speed;
    
    public void ZoomIn( Transform obj )
    {
        objectTransform = obj;
        mainCamera.orthographicSize = 2;
        mainCamera.transform.position = new Vector3(objectTransform.position.x, objectTransform.position.y, cameraposz);
    }
    public void ZoomOut()
    {
        mainCamera.orthographicSize = 5;
        mainCamera.transform.position = new Vector3(cameraposx,cameraposy, cameraposz);
    }
    public void CameraMove(Transform attacker , Transform hitted)
    {
        attackTransform = attacker;
        targetTransform = hitted;
        mainCamera.transform.position = Vector3.Lerp( new Vector3(attacker.position.x,attacker.position.y, cameraposz), new Vector3(hitted.position.x, hitted.position.y, cameraposz), 1f);
    }
}
