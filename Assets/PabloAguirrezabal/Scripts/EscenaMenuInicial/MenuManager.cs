using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;   


public class MenuManager : MonoBehaviour
{
    public Dropdown m_Dropdown;
    private bool sableDoble;
    // Start is called before the first frame update
    void Start()
    {
        sableDoble = false;
        PlayerPrefs.SetInt("sableDoble", sableDoble ? 1 : 0);
        PlayerPrefs.SetInt("puntObj", 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void changeSableDoble()
    {
        sableDoble = !sableDoble;
        PlayerPrefs.SetInt("sableDoble", sableDoble ? 1 : 0);
        Debug.Log("sableDoble ahora es:" + sableDoble);
    }

    public void changePuntuacionMax()
    {
        Debug.Log(m_Dropdown.value);
        if (m_Dropdown.value==1)
            PlayerPrefs.SetInt("puntObj", 20);
        else if (m_Dropdown.value==2)
            PlayerPrefs.SetInt("puntObj", 30);
        else
            PlayerPrefs.SetInt("puntObj", 10);
    }
}
