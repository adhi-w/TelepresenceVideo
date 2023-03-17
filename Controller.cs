using UnityEngine;

public class Controller : MonoBehaviour
{
    float sc, dis = 1;    
    float px, py, pz;

    bool viewpoint_flag = false;

    public GameObject camera_anchor;
    public Vector3 calibrateCamera;
    public Vector3 addHeight, _camera;

    

    // Start is called before the first frame update
    void Start()
    {
        sc = transform.localScale.x;
        viewpoint_flag = true;     
        
    }

    // Update is called once per frame
    void Update()
    {
       
        UseKeyboard();

        CalibrateViewpoint(viewpoint_flag);
        viewpoint_flag = false;

        transform.localPosition = calibrateCamera + new Vector3(px, py, pz);        
        transform.localScale = new Vector3(sc, transform.localScale.y, -sc);

        _camera = camera_anchor.transform.localPosition;
        //addHeight = new Vector3(px, -py, pz);

    }

    private void CalibrateViewpoint(bool flag)
    {
        if (flag || Input.GetKeyDown(KeyCode.C))
        {
            calibrateCamera = camera_anchor.transform.localPosition;           
        }               
    }

    void UseKeyboard()
    {

        //----------Change the POSITION of Object (related to user viewpoint)----------
        if (Input.GetKeyUp(KeyCode.DownArrow))         // Y Position    --> Taller / Shorter
        {
            py += dis;          
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            py -= dis;           
        }

        //if (Input.GetKeyUp(KeyCode.A))         // X Position
        //{
        //    px += dis;
        //}
        //if (Input.GetKeyUp(KeyCode.D))
        //{
        //    px -= dis;
        //}

        //if (Input.GetKeyUp(KeyCode.S))         // Z Position
        //{
        //    pz += dis;
        //}
        //if (Input.GetKeyUp(KeyCode.W))
        //{
        //    pz -= dis;
        //}


        ////----------------Change the SCALE of Sphere---------------
        if (Input.GetKeyDown(KeyCode.M))         // Scale
        {
            sc += 1;   
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            sc -= 1;    
        }

        // ------------------------------------------

        if (Input.GetKeyDown(KeyCode.Escape))
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif
        }
    }

}
