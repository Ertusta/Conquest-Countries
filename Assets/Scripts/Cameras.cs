using UnityEngine;

public class Cameras : MonoBehaviour
{
    public float dragSpeed = 2f;
    public float minX = -10f;  // Sol limit
    public float maxX = 10f;   // Sa� limit

    private Vector3 lastMousePosition;

    void Update()
    {
        // E�er parma��n/d��menin bas�l� oldu�u an
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        // Parma��n� s�r�kl�yorsan
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