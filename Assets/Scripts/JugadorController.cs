using Unity.Netcode;
using UnityEngine;
using TMPro;

public class JugadorController : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float velocidad = 5f;

    private CartaCaja cartaCercana;
    private GestorCartas gestorCartas;

    public NetworkVariable<int> score = new NetworkVariable<int>(0);

    private TextMeshPro scoreText3D;

    void Start()
    {
        if (playerCamera != null)
            playerCamera.gameObject.SetActive(IsOwner);

        gestorCartas = GetComponent<GestorCartas>();

        CrearTextoPuntuacion();
        ActualizarScoreUI(score.Value);
        score.OnValueChanged += OnScoreChanged;
    }

    void OnDestroy()
    {
        score.OnValueChanged -= OnScoreChanged;
    }

    void CrearTextoPuntuacion()
    {
        //NUMERO DENTRO
        GameObject textoGO = new GameObject("ScoreText3D");
        textoGO.transform.SetParent(transform);
        
        textoGO.transform.localPosition = new Vector3(0f, 0.01f, 0f);
        textoGO.transform.localRotation = Quaternion.identity;textoGO.layer = 4;

        
        scoreText3D = textoGO.AddComponent<TextMeshPro>();
        scoreText3D.text = "0";
        scoreText3D.fontSize = 5f;
        scoreText3D.alignment = TextAlignmentOptions.Center;
        scoreText3D.color = Color.black;

       
        var renderer = scoreText3D.GetComponent<MeshRenderer>();
        renderer.sortingOrder = 10;
    }

    void OnScoreChanged(int oldValue, int newValue)
    {
        ActualizarScoreUI(newValue);
    }

    void ActualizarScoreUI(int valor)
    {
        if (scoreText3D != null)
            scoreText3D.text = valor.ToString();
    }

    void Update()
    {
        if (!IsOwner) return;

        Vector3 direccion = new Vector3();
        direccion.x = Input.GetAxisRaw("Horizontal");
        direccion.y = Input.GetAxisRaw("Vertical");
        direccion.Normalize();
        transform.position += direccion * velocidad * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && cartaCercana != null)
        {
            gestorCartas.IntentarVoltearCarta(cartaCercana);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var carta = other.GetComponent<CartaCaja>();
        if (carta != null)
            cartaCercana = carta;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var carta = other.GetComponent<CartaCaja>();
        if (carta != null && cartaCercana == carta)
            cartaCercana = null;
    }
}
