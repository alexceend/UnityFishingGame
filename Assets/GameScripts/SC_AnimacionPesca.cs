using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_AnimacionPesca : MonoBehaviour
{
    public Transform puntoDePesca; // Posici�n donde se fijar� la c�mara al pescar
    public Camera camaraJugador; // La c�mara del jugador
    public GameObject ca�aPrefab; // Prefab de la ca�a de pescar
    public Transform puntoLanzamiento; // Donde aparece la ca�a
    public GameObject player; // Referencia al jugador
    public Transform cameraPos; // Referencia al hijo "CameraPos" del jugador


    public GameObject gameManager;
    GameManager gameManagerScript;

    private bool estadoAnterior = false; // Estado anterior de pesca
    GameObject ca�aInstanciada;

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

        if (ca�aPrefab != null && puntoLanzamiento != null)
        {
            ca�aInstanciada = Instantiate(ca�aPrefab, puntoLanzamiento.position, puntoLanzamiento.rotation);
        }
    }

    void finishAnimation()
    {
        if (camaraJugador != null && cameraPos != null)
        {
            // Restaurar la c�mara a la posici�n y rotaci�n de CameraPos
            camaraJugador.transform.position = cameraPos.position;
            camaraJugador.transform.rotation = cameraPos.rotation;
        }

        if (ca�aInstanciada != null)
        {
            Destroy(ca�aInstanciada);
            ca�aInstanciada = null; // Limpiar referencia
        }
    }
}
