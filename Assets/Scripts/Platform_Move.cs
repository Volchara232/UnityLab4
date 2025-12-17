using UnityEngine;

public class PlatformMouseControl : MonoBehaviour
{

    public float speed = 10f;
    public bool clampToScreen = true;
    public float xLimit = 7f; 
    
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
    }
    
    void Update()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPosition = new Vector3(mousePosition.x, transform.position.y, 0);
        
        if (clampToScreen)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, -xLimit, xLimit);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }
}