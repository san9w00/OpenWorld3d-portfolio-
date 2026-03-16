using UnityEngine;

public class MirrorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform mirrorPivot;
    [SerializeField] private float rotateAngle = 45f;
    [SerializeField] private float rotateSpeed = 5f;

    private bool rotating;
    private Quaternion targetRotation;

    public void Interact()
    {
        if (rotating) return;

        targetRotation = mirrorPivot.rotation * Quaternion.Euler(0, rotateAngle, 0);

        StartCoroutine(Rotate());
    }

    private System.Collections.IEnumerator Rotate()
    {
        rotating = true;

        Quaternion start = mirrorPivot.rotation;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * rotateSpeed;
            
            mirrorPivot.rotation = Quaternion.Lerp(start, targetRotation, t);

            yield return null;
        }

        rotating = false;
    }

    public bool CanInteract()
    {
        return !rotating;
    }

    public string GetInteractText()
    {
        return "Rotate Mirror 45'";
    }  
}
