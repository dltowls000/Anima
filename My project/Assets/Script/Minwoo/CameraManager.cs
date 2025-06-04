using DG.Tweening;
using System.Collections;
using System.Xml;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    CinemachineCamera allyAttackCam;
    [SerializeField]
    CinemachineCamera enemyAttackCam;
    public Camera mainCamera;
    Transform objectTransform;
    float cameraposx = 0f;
    float cameraposy = 0f;
    float cameraposz = -10f;
    public float speed;
    public AnimatorController animatorController;
    public IEnumerator ZoomIn(Transform obj, bool isAlly)
    {
        objectTransform = obj;
        
        if (isAlly)
        {
            allyAttackCam.ForceCameraPosition(new Vector3(objectTransform.position.x, objectTransform.position.y, cameraposz), allyAttackCam.transform.rotation);
            animator.SetTrigger("AllyAttack");
            yield return animatorController.WaitForAnimationEnd();
            yield return new WaitForSeconds(3f);

        }
        else
        {
            enemyAttackCam.ForceCameraPosition(new Vector3(objectTransform.position.x, objectTransform.position.y, cameraposz), enemyAttackCam.transform.rotation);
            animator.SetTrigger("EnemyAttack");
            yield return animatorController.WaitForAnimationEnd();
            yield return new WaitForSeconds(3f);
        }

    }
}
