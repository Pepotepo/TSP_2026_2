using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Concurrencia : MonoBehaviour
{
    [Header("Activa los métodos")]
    public bool useSincrono;
    public bool useThread;
    public bool useTask;
    public bool useCoroutine;

    [Header("Esfera a mover")]
    public Transform sincronoSphere;
    public Transform threadSphere;
    public Transform taskSphere;
    public Transform coroutineSphere;
    public Transform mainCube;

    //Acciones a ejecutar en el hilo secundario
    private Queue<Action> mainThreadActions = new Queue<Action>(); //Crea una cola de acciones

    void Start()
    {
        if(useSincrono) MoveSincrono();
        if(useThread) MoveWithThread();
        if (useTask) MoveWithTask();
        if (useCoroutine) StartCoroutine(MoveWithCoroutine());//Para inicializar la Corrutina se llama al StarCoroutine para ejecutar el método con la coroutine MoveWithCoroutine
        
    }

    
    void Update()
    {
        //Siempre gira el cubo de referencia
        mainCube.Rotate(Vector3.up, 50*Time.deltaTime);//Gira respecto a "y"
        
        //Ejecuta las acciones en el hilo principal
        lock (mainThreadActions)//Bloquea las accione del hilo secundario mientras termina las acciones del hilo principal
        {
            while (mainThreadActions.Count>0)
            {
                mainThreadActions.Dequeue().Invoke();//Saca la acción de la colección (Dequeue) y la ejecuta (Invoke)
            }
        }

    }

    //Método para herramientas de concurrencia
    void MoveSincrono()
    {
        for (int i=0; i<=100; i++)
        {
            sincronoSphere.position += Vector3.right * 0.05f;
        }

        Thread.Sleep(50);
    }

    //Movimiento con hilo secundario
    void MoveWithThread()
    {
        new Thread(() =>
        {
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(50);//Pausa el movimiento antes de la acción
                lock (mainThreadActions)//Bloquemos cualquier ejecución del hilo cuando acaba la actividad para evitar que consuma recursos
                {
                    mainThreadActions.Enqueue(() => //Enqueue añade cosas a la colección
                    {
                        threadSphere.position += Vector3.right * 0.05f;
                    });
                }
            }
        }).Start(); //=> tal que . Todo lo que este en los paréntesis es como si estuviera en una solo línea con el =>
    }


    //Métodos con task asíncrono
    async void MoveWithTask()
    {
        //await Pausa un momento en la ejecución de la tarea, no puede pausar el hilo principal porque estamos en ese
        await Task.Run(() =>
        {
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(50);
                lock (mainThreadActions)//Te quedas aquí hasta que termine la siguiente acción
                {
                    mainThreadActions.Enqueue(() =>
                    {
                        taskSphere.position += Vector3.right * 0.05f;
                    });
                }
            }
        });
    }


    //Coroutina
    IEnumerator MoveWithCoroutine()
    {
        for (int i = 0; i <= 100; i++)
        {
            coroutineSphere.position += Vector3.right * 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }



}
