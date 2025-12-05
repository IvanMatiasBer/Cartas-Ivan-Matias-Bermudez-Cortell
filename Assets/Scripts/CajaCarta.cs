using Unity.Netcode;
using UnityEngine;

public class CartaCaja : NetworkBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite spriteCerrado;
    public Sprite[] cartasSprites;
    public NetworkVariable<int> cartaID = new NetworkVariable<int>(-1);
    public NetworkVariable<bool> cartaAbierta = new NetworkVariable<bool>(false);

    void Start()
    {
        cartaAbierta.OnValueChanged += OnCartaAbiertaChanged;
        spriteRenderer.sprite = spriteCerrado;
    }

    void OnDestroy()
    {
        cartaAbierta.OnValueChanged -= OnCartaAbiertaChanged;
    }

    void OnCartaAbiertaChanged(bool oldVal, bool newVal)
    {
        spriteRenderer.sprite = newVal ? cartasSprites[cartaID.Value] : spriteCerrado;
    }

    public void CerrarCarta()
    {
        if (IsServer)
        {
            cartaAbierta.Value = false;
        }
    }

    public void AsignarCarta(int id)
    {
        cartaID.Value = id;
        spriteRenderer.sprite = spriteCerrado;
    }
}
