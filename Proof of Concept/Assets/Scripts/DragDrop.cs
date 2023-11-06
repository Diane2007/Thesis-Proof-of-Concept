using UnityEngine;

public class DragDrop : MonoBehaviour
{
    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition();
    }

    //get the mouse position in game world
    Vector2 GetMouseWorldPosition()
    {
        Vector2 mousePos = Input.mousePosition;
        //mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}