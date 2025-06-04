using System;
using System.Collections;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private bool animationEnded= false;
    public void AnimationEnded()
    {
        animationEnded = true;
    }
    public IEnumerator WaitForAnimationEnd()
    {
        animationEnded = false;
        yield return new WaitUntil(() => animationEnded == true);
        
    }
}
