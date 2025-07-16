using System.Collections;
using System.Xml;
using Unity.Cinemachine;
using Unity.VisualScripting;
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
    float cameraposz = -10f;
    public float speed;
    public AnimatorController animatorController;
    GameObject instance;
    ParticleSystem ps;
    public IEnumerator ZoomSingleOpp(Transform hitter,Transform hitted,  bool isAlly, string skill)
    {
        
        if (isAlly)
        {
            allyAttackCam.ForceCameraPosition(new Vector3(hitter.position.x, hitter.position.y, cameraposz), allyAttackCam.transform.rotation);
            animator.SetTrigger("AllyAttack");
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("AllyAttackZoom"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill), new Vector3(hitter.transform.position.x, hitter.transform.position.y, hitter.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();
            
             yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);
            
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyZoom2"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);

            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill + "ed"), new Vector3(hitted.transform.position.x, hitted.transform.position.y, hitted.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();
            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);
            


            yield return animatorController.WaitForAnimationEnd(animator, "EnemyZoom2", instance);

            yield return new WaitForSeconds(1f);
        }
        else
        {
            enemyAttackCam.ForceCameraPosition(new Vector3(hitter.position.x, hitter.position.y, cameraposz), enemyAttackCam.transform.rotation);
            animator.SetTrigger("EnemyAttack");
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyAttackZoom"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill), new Vector3(hitter.transform.position.x, hitter.transform.position.y, hitter.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();

            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("AllyZoom2"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);

            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill + "ed"), new Vector3(hitted.transform.position.x, hitted.transform.position.y, hitted.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();
            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);
            yield return animatorController.WaitForAnimationEnd(animator, "AllyZoom2", instance);
            yield return new WaitForSeconds(1f);
        }

    }

    public IEnumerator ZoomMultiOpp(Transform hitter, GameObject[] hitted, bool isAlly, string skill)
    {
        yield return null;
    }
    public IEnumerator ZoomSingleIde(Transform hitter, Transform hitted, bool isAlly, string skill)
    {
        yield return null;
    }
    public IEnumerator ZoomMultiIde(Transform hitter, GameObject[] hitted, bool isAlly, string skill)
    {
        yield return null;
    }


}
