using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.3f;
    public Vector3 offset;

    public bool sniper;
    [SerializeField] private Vector3 centerPosition;

    private Vector3 velocity = Vector3.zero;

    public Bounds bounds;
    public Camera cam;

    public Rect CalculateLimits(Camera aCam, Bounds aArea)
    {
        // Half the FOV angle in radians
        var angle = aCam.fieldOfView * Mathf.Deg2Rad * 0.5f;
 
        // half the size of the viewing frustum at a distance of "1" from the camera
        Vector2 tan = Vector2.one * Mathf.Tan(angle);
        tan.x *= aCam.aspect;
 
        // the center point of the area and it's ectents
        // the center point is taken from the bottom center of the bounding box
        Vector3 dim = aArea.extents;
        Vector3 center = aArea.center - new Vector3(0, aArea.extents.y, 0);
 
        // the maximum distance the camera can be above the area plane for each direction
        Vector2 maxDist = new Vector2(dim.x / tan.x, dim.z / tan.y);
 
        // actual distance of the camera above our plane
        float dist = aCam.transform.position.y - center.y;
 
        // the max movement range around the center of the plane
        dim.x *= 1f - dist / maxDist.x;
        dim.z *= 1f - dist / maxDist.y;
 
        // the min and max x and z coordinates the camera can be at the current distance.
        return new Rect(center.x - dim.x, center.z - dim.z, dim.x * 2, dim.z * 2);
    }

    // Update is called once per frame
    public void Follow()
    {
        if(sniper) {
            transform.position = centerPosition;
            return;  
        }
        Vector3 dPos = target.position + offset;
        Vector3 sPos = Vector3.SmoothDamp(transform.position, dPos, ref velocity, smoothSpeed);
        Rect rect = CalculateLimits(cam, bounds);
        if(rect.Contains(new Vector2(sPos.x, 0f))) {
            transform.position = new Vector3(sPos.x, sPos.y, transform.position.z);
        }
        if(rect.Contains(new Vector2(0f, sPos.z))) {
            transform.position = new Vector3(transform.position.x, sPos.y, sPos.z);
        }
    }

    // TODO: Make the camera stay in bound
}
// void RandomObstacleSecene() {
    // Debug.Log("run function)
//     if(SceneManager.sceneCount == 1) {
//         ReloadObstacleScene()
//         return
//     }

//     UnloadObstacleScene()
    
//     ReloadObstacleScene()
// }

// void ReloadObstacleScene() {
//     add scene
// }
// void UnloadObstacleScene(Scene scene) {
//     Destroy(GameObject.FInd("Obstacles")
// }

// Start -> Add  
// Next round -> delete obstacles from obstsce1, and load another scene like obstsce2 or obstsce1