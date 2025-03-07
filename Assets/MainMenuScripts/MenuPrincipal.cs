using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Main"); // Cambia por la escena del juego
    }

    public void AbrirOpciones()
    {
        Debug.Log("Abrir opciones"); // Aquí puedes abrir un nuevo panel de opciones
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
