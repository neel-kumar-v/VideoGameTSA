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

    // Update is called once per frame
    public void Follow()
    {
        if(sniper) {
            transform.position = centerPosition;
            return;  
        }
        Vector3 dPos = target.position + offset;
        Vector3 sPos = Vector3.SmoothDamp(transform.position, dPos, ref velocity, smoothSpeed);
        transform.position = sPos;
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