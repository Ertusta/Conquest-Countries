using UnityEngine;

public class Cameras : MonoBehaviour
{
    public float dragSpeed = 2f;
    public float minX = -10f;  // Sol limit
    public float maxX = 10f;   // Sað limit

    private Vector3 lastMousePosition;

    void Update()
    {
        // Eðer parmaðýn/düðmenin basýlý olduðu an
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        // Parmaðýný sürüklüyorsan
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float moveX = -delta.x * dragSpeed * Time.deltaTime;

            // Yeni pozisyonu hesapla
            Vector3 newPos = transform.position + new Vector3(moveX, 0, 0);

            // Sadece X ekseninde limitle
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

            transform.position = newPos;

            lastMousePosition = Input.mousePosition;
        }
    }
}