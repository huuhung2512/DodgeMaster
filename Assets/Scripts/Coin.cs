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
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < minDistance)
            {
                AudioManager.Instance.PlaySound(GameEnum.ESound.PickupCoin);
                ParticleManager.Instance.PlayGoldEffect(transform.position);
                PlayerManager.numberOfCoin += 1;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isAttracted)
        {
            AudioManager.Instance.PlaySound(GameEnum.ESound.PickupCoin);
            ParticleManager.Instance.PlayGoldEffect(transform.position);
            PlayerManager.numberOfCoin += 1;
            Destroy(gameObject);
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