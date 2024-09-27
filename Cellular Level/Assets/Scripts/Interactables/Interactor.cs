using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRad = 0.5f;
    [SerializeField] private LayerMask _interactableMask;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;
    PlayerController playerController;
    [SerializeField] GameObject player;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRad, _colliders, (int)_interactableMask);
        if (_numFound > 0)
        {
            var interactable = _colliders[0].GetComponent<IInteractable>();
            if ((interactable != null) && (playerController.GetInput().Player.Fire.IsPressed()))
            {
                interactable.Interact(this);
            } 
        }
    }
}
