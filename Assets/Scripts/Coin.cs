using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool isAttracted = false;
    private Transform target;
    public float moveSpeed = 20f; // Tốc độ di chuyển về nhân vật
    public float minDistance = 0.1f; // Khoảng cách tối thiểu để "nhặt"

    void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0); 

        if (isAttracted && target != null)
        {
            // Increase speed over time for "swoosh" effect
            moveSpeed += 30f * Time.deltaTime; 
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < minDistance)
            {
                Collect();
            }
        }
    }

    private void Collect()
    {
        AudioManager.Instance.PlaySound(GameEnum.ESound.PickupCoin);
        ParticleManager.Instance.PlayGoldEffect(transform.position);
        PlayerManager.numberOfCoin += 1;
        gameObject.SetActive(false);
        // Reset state for pooling
        isAttracted = false; 
        moveSpeed = 20f; // Reset speed
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isAttracted)
        {
            Collect();
        }
    }
    public void Attract(Transform player)
    {
        if (!isAttracted)
        {
            isAttracted = true;
            target = player;
        }
    }
}