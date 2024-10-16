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
    [SerializeField] PlayerController player;
    [SerializeField] List<IInteractable> heldItems = new List<IInteractable>();

    private void Start()
    {
        player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRad, _colliders, (int)_interactableMask);
    }
    public void Interact(InputAction.CallbackContext context)
    {
        if (_numFound > 0)
        {
            var interactable = _colliders[0].GetComponent<IInteractable>();
            if ((interactable != null) && (context.action.triggered))
            {
                interactable.Interact(this);
                player.OnGather(interactable);
                if (interactable.Tag() == "PickUp") heldItems.Add(interactable);
            }
        }
    }
    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(_interactionPoint.transform.position, _interactionPointRad);
    //}
}
