using UnityEngine;
namespace Models
{
    public class Platform : MonoBehaviour
    {

    private float speed = 10f;
    private bool clampToScreen = true;
    private float xLimit = 7f; 
    
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
}
