using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player_Animation : MonoBehaviour
{
    [SerializeField] private Player_Movement movement;

    private Animator animator;

    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int castHash = Animator.StringToHash("Cast");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat(speedHash, movement.MoveInput.sqrMagnitude);
    }

    public void PlayCast()
    {
        animator.SetTrigger(castHash);
    }

}