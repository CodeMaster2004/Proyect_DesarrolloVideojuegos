using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 10f;
    public float fuerzaSalto = 12.5f;

    public GameObject kunaiPrefab;
    public int kunaisDisponibles = 5;

    public Transform groundCheck;
    public LayerMask groundLayer;


    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;

    private bool isGrounded = true;
    private string direccion = "Derecha";
    private bool puedeMoverseVerticalMente = false;
    private float defaultGravityScale = 1f;
    private bool puedeSaltar = true;
    private bool puedeLanzarKunai = true;


    [Header("Parámetros de salto")]
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Coyote Time")]
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Jump Buffer")]
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    public int VidaPlayer;
    public Text VidaPlayerText;

    public int EnemigosVivos;

    private AudioSource ad;
    public AudioClip clip;
    GameData data;

    // Variables globales (deben estar definidas en tu clase)
    private float tiempoInicioPresion;
    private bool teclaKPresionada = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        ad = GetComponent<AudioSource>();

        defaultGravityScale = rb.gravityScale;

        EnemigosVivos = GameObject.FindGameObjectsWithTag("Enemigo").Length;

        VidaPlayer = 10;
        VidaPlayerT();
    }

    // Update is called once per frame
    void Update()
    {
        SetupMoverseHorizontal();
        SetupMoverseVertical();
        SetupSalto();
        SetUpLanzarKunai();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            ZombieController zombie = collision.gameObject.GetComponent<ZombieController>();
            // Debug.Log($"Colision con Enemigo: ${zombie.puntosVida}");
            Destroy(collision.gameObject);
            VidaPlayer--;
            VidaPlayerT();
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        // Debug.Log($"Trigger con: {other.gameObject.name}");
        if (other.gameObject.name == "Muro")
        {
            puedeMoverseVerticalMente = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        // Debug.Log($"Trigger con: {other.gameObject.name}");
        if (other.gameObject.name == "Muro")
        {
            puedeMoverseVerticalMente = false;
            rb.gravityScale = defaultGravityScale;
        }
    }
    void SetupMoverseHorizontal()
    {

        // Debug.Log($"isGrounded: {isGrounded}, {rb.linearVelocityY}");
        if (isGrounded && rb.linearVelocityY == 0)
        {
            animator.SetInteger("Estado", 0);
        }

        rb.linearVelocityX = 0;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.linearVelocityX = velocidad;
            sr.flipX = false;
            direccion = "Derecha";
            if (isGrounded && rb.linearVelocityY == 0)
                animator.SetInteger("Estado", 1);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocityX = -velocidad;
            sr.flipX = true;
            direccion = "Izquierda";
            if (isGrounded && rb.linearVelocityY == 0)
                animator.SetInteger("Estado", 1);
        }
    }
    void SetupMoverseVertical()
    {

        if (!puedeMoverseVerticalMente) return;
        rb.gravityScale = 0;
        rb.linearVelocityY = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.linearVelocityY = velocidad;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.linearVelocityY = -velocidad;
        }
    }
    void SetupSalto()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);

        // --- Coyote Time ---
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            if (rb.linearVelocityY > 5f)
                animator.SetInteger("Estado", 3);

        }

        // --- Jump Buffer ---
        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
            jumpBufferCounter -= Time.deltaTime;

        // --- Ejecutar salto ---
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocityY = jumpForce;
            jumpBufferCounter = 0f;
        }

        // --- Ajuste de gravedad para caída más rápida ---
        if (rb.linearVelocityY < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }
        else if (rb.linearVelocityY > 0 && !Input.GetKeyDown(KeyCode.W))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;

        }
    }

    void SetUpLanzarKunai()
    {
        if (!puedeLanzarKunai || kunaisDisponibles <= 0) return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            tiempoInicioPresion = Time.time;
            teclaKPresionada = true;
        }

        if (Input.GetKeyUp(KeyCode.K) && teclaKPresionada)
        {
            GameObject kunai = Instantiate(kunaiPrefab, transform.position, Quaternion.Euler(0, 0, -90));
            KunaiController kunaiCtrl = kunai.GetComponent<KunaiController>();

            float duracionPresion = Time.time - tiempoInicioPresion;
            teclaKPresionada = false;

            // Condiciones según el tiempo presionado
            if (duracionPresion >= 2f && duracionPresion < 3f)
            {
                Debug.Log("Tecla presionada por 2 segundos");
                kunaiCtrl.damage = 3;
                kunaiCtrl.size_kunai = 1.5f;
            }
            else if (duracionPresion >= 3f)
            {
                Debug.Log("Tecla presionada por 3 segundos o más");
                kunaiCtrl.damage = 2;
                kunaiCtrl.size_kunai = 2f;
            }
            else
            {
                Debug.Log("Tecla presionada menos de 2 segundos");
                kunaiCtrl.damage = 1;
                kunaiCtrl.size_kunai = 1f;
            }
            kunaiCtrl.SetDirection(direccion);
            kunaisDisponibles--;
            ad.PlayOneShot(clip);
        }
    }

    // Visualiza el groundCheck en el editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
        }
    }

    void VidaPlayerT()
    {
        VidaPlayerText.text = $"Vida: {VidaPlayer}";
    }
}
