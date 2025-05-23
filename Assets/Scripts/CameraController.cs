using UnityEngine;

public class CameraController : MonoBehaviour
{

    GameObject player;

    void Start()
    {
        player = GameObject.Find("NinjaPlayer");
        
    }

    void Update()
    {
        float positionX = player.transform.position.x;
        float positionY = player.transform.position.y;
        float positionZ = transform.position.z;
        transform.position = new Vector3(positionX, positionY, positionZ);
    }
}
