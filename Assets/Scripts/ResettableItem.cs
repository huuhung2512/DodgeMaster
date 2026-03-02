using UnityEngine;

public class ResettableItem : MonoBehaviour
{
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    void Awake()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
    }

    public void ResetItem()
    {
        transform.localPosition = initialLocalPosition;
        transform.localRotation = initialLocalRotation;
        gameObject.SetActive(true);
    }
}
