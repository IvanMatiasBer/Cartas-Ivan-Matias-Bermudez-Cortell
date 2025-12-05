using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GestorCartas : NetworkBehaviour
{
    private List<CartaCaja> cartasAbiertas = new List<CartaCaja>();

    private JugadorController jugador; 

    void Awake()
    {
        jugador = GetComponent<JugadorController>();
    }

    public int CartasAbiertasCount()
    {
        return cartasAbiertas.Count;
    }

    public void IntentarVoltearCarta(CartaCaja carta)
    {
        if (!IsOwner) return;

        if (carta == null) return;
        if (CartasAbiertasCount() >= 2) return;

        IntentarVoltearCartaServerRpc(carta.NetworkObject);
    }

    [ServerRpc]
    private void IntentarVoltearCartaServerRpc(NetworkObjectReference cartaRef, ServerRpcParams rpcParams = default)
    {
        if (!cartaRef.TryGet(out NetworkObject netObj)) return;

        CartaCaja carta = netObj.GetComponent<CartaCaja>();
        if (carta == null) return;

        if (carta.cartaAbierta.Value) return;

        carta.cartaAbierta.Value = true;

        CartaVolteada(carta);
    }

    private void CartaVolteada(CartaCaja carta)
    {
        if (cartasAbiertas.Contains(carta)) return;
        cartasAbiertas.Add(carta);

        if (cartasAbiertas.Count == 2)
        {
            StartCoroutine(ComprobarPareja());
        }
    }

    private IEnumerator ComprobarPareja()
    {
        yield return new WaitForSeconds(1f);

        bool esPareja = cartasAbiertas[0].cartaID.Value == cartasAbiertas[1].cartaID.Value;

        if (esPareja)
        {
            //Quitar cartas
            if (IsServer)
            {
                cartasAbiertas[0].NetworkObject.Despawn(true);
                cartasAbiertas[1].NetworkObject.Despawn(true);

                
                if (jugador != null)
                {
                    jugador.score.Value += 1;
                }
            }
        }
        else
        {
            cartasAbiertas[0].CerrarCarta();
            cartasAbiertas[1].CerrarCarta();
        }

        cartasAbiertas.Clear();
    }
}
