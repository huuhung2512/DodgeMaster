using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityItem : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ActivateInvincibility();
            Destroy(gameObject); // X�a item sau khi d�ng
        }
    }
}
