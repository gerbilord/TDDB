using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 5f;
    public float minY;
    public float maxY;

    
    public float panSpeed = 5f;
    public float panBorderThickness = 10f;
    public bool allowPanning;

    private void Update()
    {
        HandleZoom();

        if (allowPanning)
        {
            HandlePan();
        }
    }

    private void HandleZoom()
    {
        // Get the scroll wheel input
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        // if the scroll is 0, return
        if (scroll == 0)
        {
            return;
        }
        
        // If trying to zoom when at max or min zoom, return
        if (transform.position.y >= maxY && scroll < 0)
        {
            return;
        }
        
        if (transform.position.y <= minY && scroll > 0)
        {
            return;
        }
        
        // Change y position of camera by scroll amount
        transform.Translate(Vector3.forward * (scroll * zoomSpeed * Time.deltaTime), Space.Self);
    }

    private void HandlePan()
    {
        // If the mouse is at the edge of the screen, move the camera in that direction, relative to the world, account for the camera x and y angle
        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            // Rotate the vector by the camera's y angle
            Vector3 forward = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward;
         
            // Move the camera in the vector direction, relative to the world
            transform.Translate( forward * (panSpeed * Time.deltaTime), Space.World);
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            Vector3 back = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.back;
            transform.Translate(back * (panSpeed * Time.deltaTime), Space.World);
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            Vector3 right = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.right;
            transform.Translate(right * (panSpeed * Time.deltaTime), Space.World);
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            Vector3 left = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.left;
            transform.Translate(left * (panSpeed * Time.deltaTime), Space.World);
        }
        
    }
}