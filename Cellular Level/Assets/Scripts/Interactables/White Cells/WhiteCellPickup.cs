using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCellPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    public float spin;
    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        gameObject.SetActive(false);
        return true;
    }
    private void Update()
    {
         transform.Rotate(
             Random.Range(0,spin), Random.Range(0, spin), 0f,
             Space.World);
    }
    public string Tag()
    {
        return tag;
    }
}
