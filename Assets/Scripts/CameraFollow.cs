using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;                 // Player
    public Vector3 offset = new Vector3(8, 3, 0);  // lateral + un poco arriba
    public float smooth = 8f;

    float fixedY;            // altura fija de cámara
    Quaternion fixedRot;     // rotación fija de cámara

    void Start()
    {
        // Guardamos la altura y la rotación actuales para mantenerlas
        fixedY = transform.position.y;
        fixedRot = transform.rotation;
    }

    void LateUpdate()
    {
        if (!target) return;

        // Seguir en X/Z con el offset, pero BLOQUEAR Y
        Vector3 desired = target.position + offset;
        desired.y = fixedY;

        transform.position = Vector3.Lerp(transform.position, desired, smooth * Time.deltaTime);

        // Mantener la rotación fija (sin inclinar al saltar)
        transform.rotation = fixedRot;
        // (No usar LookAt aquí para que no “asome” el borde del fondo)
    }
}
