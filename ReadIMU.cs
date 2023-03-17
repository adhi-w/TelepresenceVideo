using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReadIMU : MonoBehaviour
{
    
    public string filePath= "Assets\\IMUdata\\imu_1006_1827.txt";
   

    // Start is called before the first frame update
    public void Start()
    {
        // Read the content of text file as individual lines
        string[] lines = System.IO.File.ReadAllLines(@filePath);

        // Create lists
        var sequence = new List<int>();
        List<Rotation> rot = new List<Rotation>();
        List<Velocity> vel = new List<Velocity>();
        List<Acceleration> acc = new List<Acceleration>();
        List<DateTime> dateTime = new List<DateTime>();

        // Split each row into column data
        for (int i = 1; i < 10; i++)
        {
            // Splitting is based on comma delimeter
            string[] data = lines[i].Split(',');

            // Convert string to integer and store data to the list
            int seq = Convert.ToInt16(data[0]);
            sequence.Add(seq);

            // Convert string to float
            float.TryParse(data[1], out float rx);
            float.TryParse(data[2], out float ry);
            float.TryParse(data[3], out float rz);
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
