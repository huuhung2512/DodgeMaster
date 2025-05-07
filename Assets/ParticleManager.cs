using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    // Particle Prefabs cho vàng và item
    [System.Serializable]
    public class ParticleEffect
    {
        public GameEnum.EParticle tag; // Enum cho loại Particle
        public ParticleSystem prefab;
        public int size = 10; // Số lượng particle trong pool
    }

    public List<ParticleEffect> particleEffects;
    private Dictionary<GameEnum.EParticle, Queue<ParticleSystem>> particlePoolDictionary = new Dictionary<GameEnum.EParticle, Queue<ParticleSystem>>();
    // Lưu trữ Particle System đang chạy để dễ dàng dừng
    private Dictionary<GameEnum.EParticle, List<ParticleSystem>> activeParticles = new Dictionary<GameEnum.EParticle, List<ParticleSystem>>();

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        InitializeParticleEffects();
    }

    private void InitializeParticleEffects()
    {
        foreach (var particle in particleEffects)
        {
            Queue<ParticleSystem> pool = new Queue<ParticleSystem>();
            activeParticles.Add(particle.tag, new List<ParticleSystem>()); // Khởi tạo danh sách active particles

            for (int i = 0; i < particle.size; i++)
            {
                ParticleSystem obj = Instantiate(particle.prefab, transform);
                obj.gameObject.SetActive(false);
                pool.Enqueue(obj);
            }

            particlePoolDictionary.Add(particle.tag, pool);
        }
    }

    public void PlayGoldEffect(Vector3 position)
    {
        PlayParticle(GameEnum.EParticle.Gold, position);
    }

    public void PlayItemEffect(Vector3 position, GameEnum.EParticle tag)
    {
        PlayParticle(tag, position);
    }

    private void PlayParticle(GameEnum.EParticle tag, Vector3 position)
    {
        if (!particlePoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Particle effect {tag} not found!");
            return;
        }
        var pool = particlePoolDictionary[tag];
        ParticleSystem particle = GetFromPool(pool);

        if (particle != null)
        {
            particle.transform.position = position;
            particle.gameObject.SetActive(true);
            particle.Play();

            // Thêm vào danh sách active particles
            activeParticles[tag].Add(particle);

            // Nếu không loop, trả lại pool sau khi chơi xong
            if (!particle.main.loop)
            {
                StartCoroutine(ReturnParticleToPool(particle, particle.main.duration, pool));
            }
        }
    }

    private ParticleSystem GetFromPool(Queue<ParticleSystem> pool)
    {
        foreach (var particle in pool)
        {
            if (!particle.gameObject.activeSelf)
            {
                return particle;
            }
        }

        Debug.LogWarning("Particle pool is empty, creating new...");
        return null;
    }

    private IEnumerator ReturnParticleToPool(ParticleSystem particle, float delay, Queue<ParticleSystem> pool)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (particle != null && particle.gameObject.activeSelf)
        {
            particle.Stop();
            particle.gameObject.SetActive(false);
            activeParticles[GetParticleTag(particle)].Remove(particle); // Xóa khỏi danh sách active
        }
    }

    // Hàm mới: Dừng Particle System theo tag
    public void StopParticle(GameEnum.EParticle tag)
    {
        if (!activeParticles.ContainsKey(tag))
        {
            Debug.LogWarning($"No active particles found for tag {tag}!");
            return;
        }

        // Dừng tất cả Particle System đang chạy với tag này
        foreach (var particle in activeParticles[tag].ToArray()) // ToArray để tránh lỗi khi xóa trong foreach
        {
            if (particle != null && particle.gameObject.activeSelf)
            {
                particle.Stop(); // Dừng Particle System
                particle.gameObject.SetActive(false); // Tắt GameObject
                activeParticles[tag].Remove(particle); // Xóa khỏi danh sách active
                particlePoolDictionary[tag].Enqueue(particle); // Trả lại pool
            }
        }
    }

    // Helper: Lấy tag của Particle System
    private GameEnum.EParticle GetParticleTag(ParticleSystem particle)
    {
        foreach (var pair in particlePoolDictionary)
        {
            if (pair.Value.Contains(particle))
            {
                return pair.Key;
            }
        }
        return GameEnum.EParticle.None; // Default nếu không tìm thấy
    }
}