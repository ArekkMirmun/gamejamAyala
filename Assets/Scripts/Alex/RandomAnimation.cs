using UnityEngine;

public class RandomAnimation : MonoBehaviour
{
    //Sets the current animation at a random frame
    public void Start()
    {
        Animator animator = GetComponent<Animator>();
        //get the animation playing
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        //get the length of the animation
        float length = clipInfo[0].clip.length;
        //get a random time in the animation
        float randomTime = Random.Range(0, length);
        //set the time of the animation to the random time
        animator.Play(0, 0, randomTime);
    }
}
