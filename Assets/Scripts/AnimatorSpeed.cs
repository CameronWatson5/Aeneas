using UnityEngine;

public class AnimatorSpeed : MonoBehaviour
{
    public float animationSpeed = 0.5f; // Adjust this value to change speed

    void Start()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed = animationSpeed;
        }
    }
}