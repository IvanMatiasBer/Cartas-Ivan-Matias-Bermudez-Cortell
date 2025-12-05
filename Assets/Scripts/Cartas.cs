using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsignadorDeParejas : MonoBehaviour
{
    public CartaCaja[] cartasEnEscena;

    IEnumerator Start()
    {
        yield return null; // Espera 1 frame

        List<int> IDs = new List<int>() { 0, 0, 1, 1, 2, 2, 3, 3,4,4,5,5,6,6,7,7,8,8,9,9,10,10,11,11,12,12,13,13,14,14,15,15 };
        for (int i = 0; i < IDs.Count; i++)
        {
            int rnd = Random.Range(0, IDs.Count);
            int temp = IDs[i];
            IDs[i] = IDs[rnd];
            IDs[rnd] = temp;
        }
        for (int i = 0; i < cartasEnEscena.Length; i++)
        {
            cartasEnEscena[i].AsignarCarta(IDs[i]);
        }
    }
}
