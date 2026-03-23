using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class Flight : MonoBehaviour
{
    public float speed = 50.0f;
    public float rotationSpeed = 100f;
    public Transform cameraTransform; //Posición de la cámara
    public Vector2 movementImput; //Vector de movimiento

    //Control de iteraciones
    public int turbulenceIterations = 1000000;
    
    //Lista de vectores de posición calculados
    private List<Vector3> turbulenceForces = new List<Vector3>();

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
        if(cameraTransform == null)
        {
            Debug.LogError("No hay cámara asignada");
            return;
        }

        //Actividad 1: Calculo pesado. Proceso que consume recursos. Simulación de la turbulencia
        SimulateTurbulence();

        //Mover la nave de forma lineal
        Vector3 moveDirection = cameraTransform.forward * movementImput.y * speed * Time.deltaTime;
        this.transform.position += moveDirection;

        //Mover la nave en rotación
        float yaw = movementImput.x * rotationSpeed * Time.deltaTime;
        this.transform.Rotate(0, yaw, 0);
    }

    public void SimulateTurbulence()
    {
        turbulenceForces.Clear();//Limpia la lista

        //Repeticiones
        for (int i=0; i<turbulenceIterations; i++)
        {
            Vector3 force = new Vector3(//Vectores con tres características
                    Mathf.PerlinNoise(i * 0.001f, Time.time)* 2-1,
                    Mathf.PerlinNoise(i*0.002f, Time.time)* 2-1,
                    Mathf.PerlinNoise(i*0.003f, Time.time)* 2-1
                );
        }

    }

}
