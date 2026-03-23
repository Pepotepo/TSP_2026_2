using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
//Librerías para trabajar en el hilo secundario
using System.Threading;
using Unity.VisualScripting;

public class FlightThread : MonoBehaviour
{
    public float speed = 50.0f;
    public float rotationSpeed = 100f;
    public Transform cameraTransform; //Posición de la cámara
    public Vector2 movementImput; //Vector de movimiento

    //Control de iteraciones
    public int turbulenceIterations = 1000000;

    //Lista de vectores de posición calculados
    private List<Vector3> turbulenceForces = new List<Vector3>();

    //Variables para manipular el hilo secundario
    private Thread turbulenceThread; //La instancia del hilo secundario
    private bool isTurbulenceRunning = false;//Bandera para saber si sigue el cálculo
    private bool stopTurbulenceThread = false;//Bandera para saber si el hilo terminó
    private float capturedTime;//Variable para almacenar el tiempo transcurrido

    //Método para vomer la nave
    public void OnMovement(InputValue value)
    {
        movementImput = value.Get<Vector2>();//Guarda el valor del vector que nosotros mandamos para mover la nave
    }

    void Start()
    {

    }


    void Update()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("No hay cámara asignada");
            return;
        }

        //Actividad 1: Calculo pesado. Proceso que consume recursos. Simulación de la turbulencia
        
        //Captura del timepo transcurrido
        capturedTime = Time.time;

        //Proceso pesado en hilo secundario
        if (!isTurbulenceRunning)
        {
            isTurbulenceRunning=true;
            stopTurbulenceThread = false;
            turbulenceThread = new Thread(() => SimulateTurbulence(capturedTime));
            turbulenceThread.Start();
        }

        //Mover la nave de forma lineal
        Vector3 moveDirection = cameraTransform.forward * movementImput.y * speed * Time.deltaTime;
        this.transform.position += moveDirection;

        //Mover la nave en rotación
        float yaw = movementImput.x * rotationSpeed * Time.deltaTime;
        this.transform.Rotate(0, yaw, 0);
    }

    public void SimulateTurbulence(float time)
    {
        turbulenceForces.Clear();//Limpia la lista

        //Repeticiones
        for (int i = 0; i < turbulenceIterations; i++)
        {
            //Verificar si se debe detener el hilo
            if (stopTurbulenceThread)
            {
                break;
            }
            Vector3 force = new Vector3(//Vectores con tres características
                    Mathf.PerlinNoise(i * 0.001f, time) * 2 - 1,
                    Mathf.PerlinNoise(i * 0.002f, time) * 2 - 1,
                    Mathf.PerlinNoise(i * 0.003f, time) * 2 - 1
                );

            turbulenceForces.Add(force);
        }

        //Señal en consola de inicio del hilo
        Debug.Log("Iniciando simulación de turbulencia");

        //Simulación completa
        isTurbulenceRunning = false;

    }

    private void OnDestroy()//Destruye la instancia del hilo
    {
        //Indicar el cierre del hilo secundario
        stopTurbulenceThread= true;
        //Verificar si el hilo existe y se esta ejecutando
        if(turbulenceThread != null && turbulenceThread.IsAlive)
        {
            //Lo unimos al hilo principal y cerramos la ejecución
            turbulenceThread.Join();
        }
    }

}
