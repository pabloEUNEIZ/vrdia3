using UnityEngine;
using UnityEngine.InputSystem; // Necesario para el Input System
using UnityEngine.SceneManagement; // Necesario para cargar escenas
using UnityEngine.UI; // Necesario si quieres interactuar con botones u otros elementos UI directamente

public class UIManagerPablo : MonoBehaviour
{

    public InputActionReference customButton;

    public GameObject panel;
    public GameObject gameManager;

    void Start()
    {
        customButton.action.started += PulsadoMenu;
        
    }

    void PulsadoMenu(InputAction.CallbackContext context)
    {
        Debug.Log("Pulsado");
        panel.SetActive(!panel.gameObject.activeSelf);
        gameManager.SetActive(!gameManager.gameObject.activeSelf);
        
    }


    public void PulsadoRenudar() 
    {
        panel.SetActive(!panel.gameObject.activeSelf);
        gameManager.SetActive(!gameManager.gameObject.activeSelf);
    } 

    public void PulsadoSalir() 
    {
        SceneManager.LoadScene(0);
    } 
}
