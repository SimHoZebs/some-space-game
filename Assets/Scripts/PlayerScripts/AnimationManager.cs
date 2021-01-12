using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AnimationManager : NetworkBehaviour
{
    // Start is called before the first frame update
    private Animator _animator;
    private MovementControl movementControl;
    [SerializeField] private float animationTransition = 0.1f;

    [Client]
    void Start()
    {
        if (!isLocalPlayer){ return;}
        _animator = GetComponent<Animator>();
        movementControl = GetComponent<MovementControl>();
    }

    // Update is called once per frame
    [Client]
    void Update()
    {
        if (!isLocalPlayer){ return;}

        var inputHorizontal = Input.GetAxis("Horizontal");
        var inputVertical = Input.GetAxis("Vertical");

        var sideVelocity = movementControl.walkSpeed * inputHorizontal;
        var forwardVelocity = movementControl.walkSpeed * inputVertical;

        _animator.SetBool("isWalking", inputHorizontal == 0 && inputVertical == 0? false:
        true);

        _animator.SetFloat("velX", sideVelocity);
        _animator.SetFloat("velZ", forwardVelocity);
    }
}
