using UnityEngine;
using UnityEngine.UI;

public class KunaiController : MonoBehaviour
{
    private string direccion = "Derecha";
    Rigidbody2D rb;
    SpriteRenderer sr;
    PlayerController player;
    GameRepository gameRepository;
    public int damage = 1;
    public float size_kunai = 1f;
    GameData gameData;

    void Start()
    {
        // Initialize the Kunai object
        gameRepository = GameRepository.GetInstance();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gameData = gameRepository.GetData();
        // Set the size of the Kunai object
        Debug.Log("Tamaño kunai: " + size_kunai);
        transform.localScale = new Vector3(size_kunai, size_kunai, size_kunai);
        Destroy(this.gameObject, 5f);
    }
    void Update()
    {
        // Update the Kunai object
        if (direccion == "Derecha")
        {
            rb.linearVelocityX = 15;
            sr.flipY = false;

        }
        else if (direccion == "Izquierda")
        {
            rb.linearVelocityX = -15;
            sr.flipY = true;
        }

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle collision with the Kunai object
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            ZombieController zombie = collision.gameObject.GetComponent<ZombieController>();

            if (zombie == null) return;

            zombie.puntosVida -= damage;

            if (zombie.puntosVida <= 0)
            {
                Destroy(collision.gameObject);
            }
            Destroy(this.gameObject);
            gameData.EnemigosMuertos++;
            gameRepository.SaveData();
        }


    }
    public void SetDirection(string direction)
    {
        this.direccion = direction;
    }
}
