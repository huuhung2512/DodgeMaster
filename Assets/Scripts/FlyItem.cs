using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyItem : MonoBehaviour
{
    [SerializeField] private float flyForce = 10f;
    [SerializeField] private float flyDuration = 2f;

    void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.FlyUp(flyForce, flyDuration);
            Destroy(gameObject); // Xoá item sau khi dùng
        }
    }

}
