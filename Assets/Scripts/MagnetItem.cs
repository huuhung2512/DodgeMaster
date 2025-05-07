using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    [SerializeField] private float magnetRadius = 5f; // Bán kính hút vàng
    [SerializeField] private float magnetDuration = 5f; // Thời gian hút vàng

    void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ActivateMagnet(magnetRadius, magnetDuration);
            Destroy(gameObject); // Xóa item sau khi dùng
        }
    }
}