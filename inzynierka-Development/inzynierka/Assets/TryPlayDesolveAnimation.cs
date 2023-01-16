using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  
/// </summary>
public class TryPlayDesolveAnimation : MonoBehaviour
{
    [field: SerializeField]
    private Animator FadeAnimation { get; set; }
    
    public bool play = false;

    private bool startedPlay = false;
    private static readonly int Start = Animator.StringToHash("Start");

    private void Update()
    {
        if (play == true)
        {
            this.FadeAnimation.SetTrigger(Start);
            this.play = false;
            this.startedPlay = true;
        }

        if (this.startedPlay)
        {
            if (FadeAnimation.GetCurrentAnimatorStateInfo(0).length > FadeAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                this.startedPlay = false;
                Debug.Log("end ani");
            }
        }
        
    }

}
