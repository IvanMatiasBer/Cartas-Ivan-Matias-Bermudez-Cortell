using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class GestionConectar : MonoBehaviour
{
   
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    public void conectarComoCliente()
    {
        NetworkManager.Singleton.StartClient();

    }


    public void conectarComoHost()
    {

        NetworkManager.Singleton.StartHost();
    }
    public void Offline()
    {
        NetworkManager.Singleton.Shutdown();
    }
}
