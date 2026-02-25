using UnityEngine;

public class VRWalk : MonoBehaviour
{
    //Atributos/variables de clase
    public Transform vrCamera;
    public float angulo = 30.0f;
    public float speed = 3.0f;
    public bool move;

    private CharacterController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    //Solo se ejecuta una vez
    void Start()
    {
        controller = GetComponent<CharacterController>();//El objeto de la escena que contiene esta clase, tiene esta propiedad

    }

    // Update is called once per frame
    
    //Se ejecuta en cada cambio de frame
    void Update()
    {
        if (vrCamera.eulerAngles.x >= angulo && vrCamera.eulerAngles.x < 60.0f) 
        {
            move = true;
        }
        else
        {
            move = false;
        }

        if (move) 
        {
            Vector3 direccion = vrCamera.TransformDirection(Vector3.forward); //Forward es para indicar el vector hacia adelante
            controller.SimpleMove(direccion*speed); //Incremento vectorial en esa dirección con esa velocidad. 
        }

    }
}
