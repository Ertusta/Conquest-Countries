using System.Collections.Generic;
using UnityEngine;
public class Cameras : MonoBehaviour
{
    public float dragSpeed = 5f;
    public float minX = -10f;
    public float maxX = 10f;
    public float smoothTime = 0.1f; 

    private Vector3 lastMousePosition;
    private float targetX;
    private float currentVelocity = 0f;

    public GameObject goodLeader;
    public List<GameObject> badLeaders = new List<GameObject>();

    

    void Start()
    {
        targetX = transform.position.x;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float moveX = -delta.x * dragSpeed * Time.deltaTime;

            // Hedef X pozisyonunu güncelle
            targetX += moveX;
            targetX = Mathf.Clamp(targetX, minX, maxX);

            lastMousePosition = Input.mousePosition;
        }

        
        float newX = Mathf.SmoothDamp(transform.position.x, targetX, ref currentVelocity, smoothTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);



        if (Input.GetKeyDown(KeyCode.G))
        {
            FocusOnGoodLeader();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            FocusOnRandomBadLeader();
        }

    }


    public void FocusOnGoodLeader()
    {
        if (goodLeader == null) return;
        StartCoroutine(FocusAndBounce(goodLeader));
    }

    public void FocusOnRandomBadLeader()
    {
        if (badLeaders.Count == 0) return;

        GameObject randomLeader = badLeaders[Random.Range(0, badLeaders.Count)];
        StartCoroutine(FocusAndBounce(randomLeader));
    }



    private IEnumerator<UnityEngine.WaitForSeconds> FocusAndBounce(GameObject leader)
    {
        float focusDuration = 1f;
        float bounceHeight = 0.5f;
        float bounceDuration = 0.2f;

        Vector3 camStartPos = transform.position;
        Vector3 camTargetPos = new Vector3(leader.transform.position.x, camStartPos.y, camStartPos.z);

        // Hedef pozisyonu güncelle
        targetX = leader.transform.position.x;
        currentVelocity = 0f;

        float elapsed = 0f;

        while (elapsed < focusDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / focusDuration;
            transform.position = Vector3.Lerp(camStartPos, camTargetPos, t);
            yield return null;
        }

        // BOUNCE efekti
        Vector3 originalPos = leader.transform.position;
        Vector3 peakPos = originalPos + new Vector3(0, bounceHeight, 0);

        elapsed = 0f;
        while (elapsed < bounceDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / bounceDuration;
            leader.transform.position = Vector3.Lerp(originalPos, peakPos, t);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < bounceDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / bounceDuration;
            leader.transform.position = Vector3.Lerp(peakPos, originalPos, t);
            yield return null;
        }

        // Kamera yeni pozisyonda sabit kalacak
        targetX = transform.position.x;
        currentVelocity = 0f;
    }







}
