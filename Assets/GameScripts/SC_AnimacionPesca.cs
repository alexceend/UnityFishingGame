using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_AnimacionPesca : MonoBehaviour
{
    public Transform puntoDePesca; // Posición donde se fijará la cámara al pescar
    public Camera camaraJugador; // La cámara del jugador
    public GameObject cañaPrefab; // Prefab de la caña de pescar
    public Transform puntoLanzamiento; // Donde aparece la caña
    public GameObject player; // Referencia al jugador
    public Transform cameraPos; // Referencia al hijo "CameraPos" del jugador


    public GameObject gameManager;
    GameManager gameManagerScript;

    private bool estadoAnterior = false; // Estado anterior de pesca
    GameObject cañaInstanciada;

    void Start()
    {

        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    void Update()
    {
        bool fishing = gameManagerScript.isFishing();

        if (fishing != estadoAnterior)
        {
            if (fishing)
            {
                startAnimation();
            }
            else
            {
                finishAnimation();
            }

            estadoAnterior = fishing; // Actualizar estado
        }
    }

    void startAnimation()
    {
        if (camaraJugador != null && puntoDePesca != null)
        {
            camaraJugador.transform.position = puntoDePesca.position;
            camaraJugador.transform.rotation = puntoDePesca.rotation;
        }

        if (cañaPrefab != null && puntoLanzamiento != null)
        {
            cañaInstanciada = Instantiate(cañaPrefab, puntoLanzamiento.position, puntoLanzamiento.rotation);
        }
    }

    void finishAnimation()
    {
        if (camaraJugador != null && cameraPos != null)
        {
            // Restaurar la cámara a la posición y rotación de CameraPos
            camaraJugador.transform.position = cameraPos.position;
            camaraJugador.transform.rotation = cameraPos.rotation;
        }

        if (cañaInstanciada != null)
        {
            Destroy(cañaInstanciada);
            cañaInstanciada = null; // Limpiar referencia
        }
    }
}
