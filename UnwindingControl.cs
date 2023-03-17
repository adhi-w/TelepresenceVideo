using System;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.Video;
//using UnityEngine.EventSystems;
//using System.IO;

public class UnwindingControl : MonoBehaviour
{
    public string filePath = "Assets\\IMUdata\\Q8K_TW19_0119_1952.txt";
    ReadIMU imu = new ReadIMU();

    public Vector3 rotation;

    private VideoPlayer vidPlayer;
    int startFrame = 0; // 4 - for video waypoints_nav_in_Tellus_short_vid_1
    public int frameLength, imuLength, imuLine;

    int imuFramerate = 125;
    public int jumpImuLine = 0;  //29904 my setup, 29975 end - ..._short_vid 2      //It is used when video uses certain start lines of imu data
    public float seconds = 0;
    public double minV, maxV;

    // Start is called before the first frame update
    void Start()
    {
        vidPlayer = GetComponent<VideoPlayer>();   
       
        imu.Read(filePath);
        
    }


    // Update is called once per frame
    void Update()
    {
        frameLength = (int)vidPlayer.frameCount; // Video frame length
        imuLength = imu.length - 1; // 1 is a header, so it's not counted

        if (vidPlayer.frame >= startFrame)
        {
            // Sync video frame & imu data
            int vidFrame = (int)vidPlayer.frame;
            float gap = imuFramerate / vidPlayer.frameRate;

            minV = Math.Floor(gap * vidFrame);
            maxV = Math.Ceiling(gap * vidFrame);

            imuLine = (int)maxV + jumpImuLine;

            //imuLine = Mathf.RoundToInt(gap * vidFrame) + jumpImuLine;            

            // Unwinding the GameObject
            float xx = (-imu.rot[imuLine].x - imu.rot[0].x)*-1;
            float yy = (imu.rot[imuLine].y - imu.rot[0].y);
            float zz = (imu.rot[imuLine].z - imu.rot[0].z)*-1;

            rotation = new Vector3(imu.rot[imuLine].x, imu.rot[imuLine].y, imu.rot[imuLine].z); // Show imu rotation reading

            //transform.rotation = Quaternion.Euler(xx, yy, zz);
            transform.rotation = Quaternion.Euler(transform.localRotation.x, yy, transform.localRotation.z);
            //transform.rotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z);

            seconds = imu.dateTime[imuLine].Second + imu.dateTime[imuLine].Millisecond / 1000f;

        }
    }

    public class ReadIMU
    {
        // Create lists
        public List<int> sequence = new List<int>();
        public List<Rotation> rot = new List<Rotation>();
        public List<Velocity> vel = new List<Velocity>();
        public List<Acceleration> acc = new List<Acceleration>();
        public List<DateTime> dateTime = new List<DateTime>();

        public int length;

        public void Read(string _filePath)
        {
            // Read the content of text file as individual lines
            string[] lines = System.IO.File.ReadAllLines(@_filePath);
            length = lines.Length;

            // Split each row into column data
            for (int i = 1; i < lines.Length; i++)
            {
                // Splitting is based on comma delimeter
                string[] data = lines[i].Split(',');

                // Convert string to integer and store data to the list
                int seq = Convert.ToInt32(data[0]);
                sequence.Add(seq);

                // Convert string to float
                float.TryParse(data[1], out float rx);  //Roll
                float.TryParse(data[2], out float rz);  //Pitch
                float.TryParse(data[3], out float ry);  //Yaw
                float.TryParse(data[4], out float vx);
                float.TryParse(data[5], out float vy);
                float.TryParse(data[6], out float vz);
                float.TryParse(data[7], out float ax);
                float.TryParse(data[8], out float ay);
                float.TryParse(data[9], out float az);

                // Use class object and store data to the lists
                // Rotation, Velocity, Acceleration lists
                var r = new Rotation() { x = rx, y = ry, z = rz };                
                rot.Add(r);

                var v = new Velocity() { x = vx, y = vy, z = vz };
                vel.Add(v);

                var a = new Acceleration() { x = ax, y = ay, z = az };
                acc.Add(a);

                // Parse date & time to be classified individually
                // the string of mm.dd.yy hh:mm:ss.ms will be identified
                DateTime dt = DateTime.Parse(data[10]);
                dateTime.Add(dt);    
                
            }
        }
    
        public class Rotation
        {
            public float x { get; set; }
            public float y { get; set; }
            public float z { get; set; }
        }

        public class Velocity
        {
            public float x { get; set; }
            public float y { get; set; }
            public float z { get; set; }
        }

        public class Acceleration
        {
            public float x { get; set; }
            public float y { get; set; }
            public float z { get; set; }
        }
    }

}
