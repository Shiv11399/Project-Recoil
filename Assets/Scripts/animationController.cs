using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerController))]
public class animationController : MonoBehaviour
{
    public Animator animator;//put in animator component manually

    // Start is called before the first frame update
    void Start()
    {
        if (animator == null)
            Debug.Log("No animator here");
    }
    public void Animate(float horiVelocity, float vertiVelocity)
    {
        if (animator == null)
            return;
            animator.SetFloat("isWalking", vertiVelocity);
        animator.SetFloat("isSideWalking", horiVelocity);
        
    }
}
