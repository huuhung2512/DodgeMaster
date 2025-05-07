using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] PlayerManager playerManager;
    private Vector3 direction;
    public float fowardSpeed = 8;
    public float maxSpeed;
    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;
    private int desiredLane = 1;
    public float laneDistance = 2.5f;
    public float jumpForce = 10;
    private bool isSliding = false;
    private bool isFalling = false;
    public float gravity = -20;
    public Transform positionItem;
    public Animator animator;
    private Coroutine slideCoroutine;

    private const float slideDuration = 1.1f;
    private const float groundCheckRadius = 0.15f;
    private const float speedIncreaseRate = 0.2f;
    private const float slideGravityMultiplier = 100;
    private bool isFlying = false;
    private float originalGravity;
    private bool isInvincible = false;
    private bool isMagnetActive = false;
    private float magnetRadius;
    #region Unity Contruct
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Kiểm tra nếu game đã bị tạm dừng hoặc kết thúc
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver || PlayerManager.isPaused)
            return;

        UpdateAnimation();

        if (fowardSpeed < maxSpeed)
        {
            fowardSpeed += speedIncreaseRate * Time.deltaTime;
        }

        direction.z = fowardSpeed;

        // Di chuyển trên làn đường
        if (SwipeManager.swipeRight)
        {
            desiredLane = Mathf.Min(desiredLane + 1, 2);
        }
        if (SwipeManager.swipeLeft)
        {
            desiredLane = Mathf.Max(desiredLane - 1, 0);
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            isFalling = false;
            if (SwipeManager.swipeUp)
            {
                HandleJumpWhileSliding();
            }
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
            isFalling = direction.y < 0;
        }

        if (SwipeManager.swipeDown && !isSliding)
        {
            direction.y += gravity * slideGravityMultiplier * Time.deltaTime;
            slideCoroutine = StartCoroutine(Slide());
        }
        MoveToDesiredLane();
    }

    private void FixedUpdate()
    {
        // Kiểm tra nếu game đã bị tạm dừng hoặc kết thúc
        if (PlayerManager.isPaused || PlayerManager.gameOver)
            return;

        if (PlayerManager.isGameStarted)
        {
            controller.Move(direction * Time.fixedDeltaTime);
        }
    }
    #endregion

    #region MoveController
    private void MoveToDesiredLane()
    {
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        controller.Move(moveDir.sqrMagnitude < diff.sqrMagnitude ? moveDir : diff);
    }

    private void UpdateAnimation()
    {
        SetAnimatorBool("isGameStarted", true);
        SetAnimatorBool("isGrounded", isGrounded);
        SetAnimatorBool("isFall", isFalling);
    }

    private void SetAnimatorBool(string parameter, bool value)
    {
        if (animator.GetBool(parameter) != value)
        {
            animator.SetBool(parameter, value);
        }
    }

    // Nếu player lướt lên sẽ nhảy luôn
    private void HandleJumpWhileSliding()
    {
        if (isSliding && slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
            ResetSlide();
            Jump();
            isSliding = false;
        }
        else
        {
            Jump();
        }
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (isInvincible)
            {
                isInvincible = false;
                ParticleManager.Instance.StopParticle(GameEnum.EParticle.ItemShield);
                AudioManager.Instance.PlaySound(GameEnum.ESound.OiHetKhienRoi);
            }
            else
            {
                PlayerManager.gameOver = true;
                animator.SetBool("isGameOver", true);
                playerManager.HandleGameOver();
                AudioManager.Instance.PlaySound(GameEnum.ESound.Death);
            }
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        isFalling = false;
        SetAnimatorBool("isSliding", true);
        controller.center = new Vector3(0, 0.225f, 0);
        controller.height = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < slideDuration)
        {
            if (SwipeManager.swipeUp && isGrounded)
            {
                ResetSlide();
                Jump();
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ResetSlide();
    }

    private void ResetSlide()
    {
        controller.center = new Vector3(0, 0.4f, 0);
        controller.height = 0.8f;
        isSliding = false;
        SetAnimatorBool("isSliding", false);
    }
    #endregion

    #region Other
    public void FlyUp(float force, float duration)
    {
        if (isFlying) // Ngăn chặn gọi FlyUp khi đang bay
            return;

        originalGravity = gravity;
        AudioManager.Instance.PlaySound(GameEnum.ESound.Fly);
        StartCoroutine(FlyRoutine(force, duration));
    }

    private IEnumerator FlyRoutine(float force, float duration)
    {
        isFlying = true;
        ParticleManager.Instance.PlayItemEffect(positionItem.position, GameEnum.EParticle.ItemFly);
        gravity = 0; // Tắt trọng lực khi bay

        float startHeight = transform.position.y;
        float targetHeight = startHeight + force; // Độ cao mục tiêu = vị trí hiện tại + force
        float elapsedTime = 0f;

        while (elapsedTime < duration * 0.5f) // Dùng nửa thời gian để bay lên
        {
            float t = elapsedTime / (duration * 0.5f);
            float newY = Mathf.Lerp(startHeight, targetHeight, t);

            Vector3 newPosition = transform.position;
            newPosition.y = newY;

            if (controller != null)
            {
                controller.Move(newPosition - transform.position);
            }
            else
            {
                transform.position = newPosition;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Vector3 finalPosition = transform.position;
        finalPosition.y = targetHeight;
        if (controller != null)
        {
            controller.Move(finalPosition - transform.position);
        }
        else
        {
            transform.position = finalPosition;
        }

        elapsedTime = duration * 0.5f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gravity = originalGravity;
        isFlying = false;
        ParticleManager.Instance.StopParticle(GameEnum.EParticle.ItemFly);
        if (!isGrounded)
        {
            direction.y = 0; // Reset vận tốc theo trục y để rơi tự nhiên
        }
    }
    #endregion
    #region Invincibility
    public void ActivateInvincibility()
    {
        if (isInvincible) 
            return;

        isInvincible = true;
        ParticleManager.Instance.PlayItemEffect(positionItem.position, GameEnum.EParticle.ItemShield);
        AudioManager.Instance.PlaySound(GameEnum.ESound.TaoCoKhien);
    }
    #endregion

    #region Magnet
    public void ActivateMagnet(float radius, float duration)
    {
        if (isMagnetActive)
            return;

        magnetRadius = radius;
        StartCoroutine(MagnetRoutine(duration));
    }

    private IEnumerator MagnetRoutine(float duration)
    {
        isMagnetActive = true;
        // Kích hoạt hiệu ứng ở chân nhân vật
        ParticleManager.Instance.PlayItemEffect(positionItem.position, GameEnum.EParticle.ItemCollectCoin);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // Tìm và hút vàng trong bán kính
            Collider[] nearbyGold = Physics.OverlapSphere(transform.position, magnetRadius);
            foreach (var collider in nearbyGold)
            {
                if (collider.CompareTag("Coin"))
                {
                    Coin gold = collider.GetComponent<Coin>();
                    if (gold != null)
                    {
                        gold.Attract(transform);
                    }
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isMagnetActive = false;
        ParticleManager.Instance.StopParticle(GameEnum.EParticle.ItemCollectCoin);
    }
    #endregion
}

