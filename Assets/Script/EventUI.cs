using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEditor;

public class EventUI : MonoBehaviour
{
    //GameObject son la instancias que ya están en el juego
    public List<GameObject> listaInstrucciones;
    public int currentIndex = 0;

    public List<string> mensajesInstrucciones;
    public TextMeshProUGUI textMeshProUGUI;


    private void Awake()//Awake guarda las configuras que quieron que se guarden, se ejecuta antes del voidStart
    {
        DontDestroyOnLoad(this.gameObject);
    }


    void Start()
    {
        //Actualizar visibilidad de páneles
        UpdateVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Método para actualizar visibilidad de paneles
    private void UpdateVisibility()
    {
        for (int i=0; i < listaInstrucciones.Count; i++)//listaInstrucciones.Clount dice el número de elementos en la lista
        {
            //Solo el panel en el índice actual está activo
            listaInstrucciones[i].SetActive(i == currentIndex);//Para hacer que solo sea visible el del índice actual
        }
    }

    //Método para cambiar entre páneles
    public void CycleObjets()
    {
        //Incrementa el índice y vuelve al principio
        currentIndex = (currentIndex + 1) % listaInstrucciones.Count;
        UpdateVisibility();
    }

    //Método para actualizar el texto mostrado
    private void UpdateText()
    {
        if (mensajesInstrucciones.Count > 0)
        {

        }
    }

    //Método para cambiar de escena por nombre
    public void ChangeSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    //Método para salir de la aplicación
    public void ExitGame()
    {
        Debug.Log("Va a salir");
        Application.Quit();
        Debug.Log("Ya salio");
    }

}
