using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 30f;
    public float panBoardThickness = 10f;
    public bool doMovement = true;
    public float ScrollSpeed = 5f;
    public float minX = 10f;
    public float minY = 10f;
    public float minZ = 10f;
    public float maxX = 80f;
    public float maxY = 80f;
    public float maxZ = 80f;

    public Vector3 resetTransform;
    // Start is called before the first frame update
    void Start()
    {
        resetTransform = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            doMovement = !doMovement;
        }
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            transform.position = resetTransform;
        }

        if (!doMovement) return; 

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBoardThickness)
        {
            transform.Translate(Vector3.forward * panSpeed *Time.deltaTime, Space.World);
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= panBoardThickness)
        {
            transform.Translate(Vector3.back * panSpeed* Time.deltaTime, Space.World);
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBoardThickness)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= panBoardThickness)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y -= scroll * 1000 * ScrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
       // pos.x = Mathf.Clamp(pos.x, minX, maxX);
       // pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        transform.position = pos;
    }
}
