using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    public static BossAnimation instance;
    
    
    private void Start()
    {
        instance = this;
    }
    
    public void PauseAnimation()
    {
        GetComponent<Animator>().speed = 0;
    }
    
    public void ResumeAnimation()
    {
        GetComponent<Animator>().speed = 1;
    }
    
}
