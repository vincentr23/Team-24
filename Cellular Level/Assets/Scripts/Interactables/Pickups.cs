using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        gameObject.SetActive(false);
        return true;
    }
    private void Update()
    {
        transform.Rotate(0, 0.5f, 0, Space.World);
    }
}
