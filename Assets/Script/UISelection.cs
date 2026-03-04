//Necesarias
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
//Opcionales
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public class UISelection : MonoBehaviour
{
    public static bool gazedAt; //Guarda si se está viendo el botón
    [SerializeField]//La línea inmediata que está después de esto lo puedo modificar con el inspector pero está privado
    float fillTime = 5f; //Tiempo de relleno
    public Image radialImage;
    public UnityEvent onFileComplete; //Evento genérico que se asigna al terminar la carga

    //Proceso asíncrono
    private Coroutine fillCoroutine;

    void Start()
    {
        gazedAt = false;
        radialImage.fillAmount = 0; //fillAmount va de cero a uno, indica la cantidad de relleno
    }

    public void OnPointerEnter()
    {
        gazedAt = true;
        //Concurrencia (Asíncrono, hilos, paralelo)
        //Paralelo se ejecuta al mismo tiempo, depende de los núcleos
        //Hillos 
        //Asíncrono se dividen los instantes de tiempo en los que se ejecutan 

        if(fillCoroutine != null)//Si ya exite la corutina la vas a reiniciar
        {
            StopCoroutine(fillCoroutine);//Al ser programación orientada a objetos, esto detiene el OnPointerEnter y ya no se ejecuta lo que está fuera del if
        }
        fillCoroutine = StartCoroutine(FillRadial());

    }

    public void OnPointerExit()
    {
        gazedAt = false;
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);//Detiene el llanado
            fillCoroutine = null;

        }
        radialImage.fillAmount = 0f;//Reinicia el llenado a 0
    }

    private IEnumerator FillRadial() //Establece que todo lo que está a dentro sea contable
    {
        float elapasedTime = 0f; //Cuenta el tiempo
        while (elapasedTime < fillTime)
        {
            if (!gazedAt)//Dejamos de ver el botón
            {
                yield break;//Deja de contar y me saca del método
            }
            elapasedTime += Time.deltaTime; //Time es una clase que permite acceder al tiempo de la computadora. deltaTime hace una diferencia entre el tiempo en que empezó a contar y el tiempo actual (cuánto transcurrió desde que la mandé a llamar)

            radialImage.fillAmount = Mathf.Clamp01(elapasedTime/fillTime);

            yield return null;
        
        }

        //El evento a ejecutar
        onFileComplete?.Invoke();//Si se completó el evento se invoca al método

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
