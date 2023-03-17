/*
author: adhi.widagdo@oulu.fi
date: 2019/2020
update: 05/05/2021

 This program creates 3x2 FBcubemap


 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class QSphere : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] sphereVertices;
    private Vector2[] uv;

    public int size = 12;
    int roundness=0;
    float radius = 1;       //Only used for SetVertexToFullSphere 
    float tolerance = 28;
    //public float tolerance2 = 1; //Only for checking
    public bool Sphere, mirror;


    private void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Cube";

        CreateVertices();
        CreateTriangles();        
        //mesh.triangles = mesh.triangles.Reverse().ToArray(); // Flip normals --> this is for debugging
        mesh.RecalculateNormals();
        CreateUVFB();

        if (Sphere) roundness = size / 2;
        else roundness = 0;

    }

    private void CreateVertices()
    {
        int totalVertices = 6 * (size + 1) * (size + 1);   
        vertices = new Vector3[totalVertices]; 

        int s = size;
        int st = s + 1;

        for (int i = 0; i <=s; i++)
        {
            for(int j=0; j<=s; j++)
            {
                vertices[i * st + j] = new Vector3(0, i, j); // Left face
                vertices[i * st + st*st + j] = new Vector3(j, i, s); // Front face
                vertices[i * st + 2*st*st + j] = new Vector3(s, i, s-j); // Right face
                vertices[i * st + 3 * st * st + j] = new Vector3(s-j, i, 0); // Back face
                vertices[i * st + 4 * st * st + j] = new Vector3(j, s, s-i); // Top face
                vertices[i * st + 5 * st * st + j] = new Vector3(j, 0, i); // Bottom face
            }
        }


        for (int i = 0; i < vertices.Length; i++)
        {
            SetVertex(i, (int)vertices[i].x, (int)vertices[i].y, (int)vertices[i].z);

            //Move Origin  to the center of cube / Origin is the center of rotation (0, 0, 0)
            vertices[i].x = vertices[i].x - size / 2;
            vertices[i].y = vertices[i].y - size / 2;
            vertices[i].z = vertices[i].z - size / 2;
        }

        mesh.vertices = vertices; 
    }


    private void CreateTriangles()
    {
        int quads = 6 * size * size;    
        int[] triangles = new int[quads * 6];
        int n = 0;  //starting index
        int vl = 0; // starting vertex - left face
        int vf =(size+1)*(size+1); // starting vertex - front face
        int vr = 2*(size + 1) * (size + 1); // starting vertex - right face
        int vb = 3 * (size + 1) * (size + 1); // starting vertex - back face
        int vt = 4 * (size + 1) * (size + 1); // starting vertex - top face
        int vd = 5 * (size + 1) * (size + 1); // starting vertex - bottom face

        // Left face
        for (int i = 0; i<size; i++, vl++)
        {
            for (int j=0; j< size; j++, vl++)
            {
                n=SetQuad(triangles, n, vl, vl+1, vl+size+1, vl+size+2);    
            }
        }
        // Front face
        for (int i = 0; i < size; i++, vf++)
        {
            for (int j = 0; j < size; j++, vf++)
            {
                n = SetQuad(triangles, n, vf, vf + 1, vf + size + 1, vf + size + 2);   
            }
        }
        // Right face
        for (int i = 0; i < size; i++, vr++)
        {
            for (int j = 0; j < size; j++, vr++)
            {
                n = SetQuad(triangles, n, vr, vr + 1, vr + size + 1, vr + size + 2);    
            }
        }
        // Back face
        for (int i = 0; i < size; i++, vb++)
        {
            for (int j = 0; j < size; j++, vb++)
            {
                n = SetQuad(triangles, n, vb, vb + 1, vb + size + 1, vb + size + 2);    
            }
        }
        // Top face
        for (int i = 0; i < size; i++, vt++)
        {
            for (int j = 0; j < size; j++, vt++)
            {
                n = SetQuad(triangles, n, vt, vt + 1, vt + size + 1, vt + size + 2);    
            }
        }
        // Bottom face
        for (int i = 0; i < size; i++, vd++)
        {
            for (int j = 0; j < size; j++, vd++)
            {
                n = SetQuad(triangles, n, vd, vd + 1, vd + size + 1, vd + size + 2);    
            }
        }

        mesh.triangles = triangles;

    }
 
    private void CreateUVFB()
    {
        if (vertices == null)
        {
            return;
        }

        uv = new Vector2[vertices.Length];
        int s = size;
        int st = s + 1;

        if (!mirror)
        {
            for (int i = 0; i <= s; i++)
            {
                for (int j = 0; j <= s; j++)
                {
                    uv[i * st + j] = new Vector2((float)(j + s) / (3 * s), (float)(i + s) / (2 * s)); //Left
                    uv[i * st + st * st + j] = new Vector2((float)(j + s) / (3 * s), (float)i / (2 * s)); //Front
                    uv[i * st + 2 * st * st + j] = new Vector2((float)j / (3 * s), (float)(i + s) / (2 * s)); //Right
                    uv[i * st + 3 * st * st + j] = new Vector2((float)(j + 2 * s) / (3 * s), (float)i / (2 * s)); //Back
                    uv[i * st + 4 * st * st + j] = new Vector2((float)(j + 2 * s) / (3 * s), (float)(i + s) / (2 * s)); //Top
                    uv[i * st + 5 * st * st + j] = new Vector2((float)j / (3 * s), (float)i / (2 * s)); //Bottom
                }
            }
        }
        else
            for (int i = 0; i <= s; i++)
            {
                for (int j = 0; j <= s; j++)
                {
                    uv[i * st + j] = new Vector2((float)(s-j)/ (3 * s), (float)(i + s) / (2 * s)); //Left
                    uv[i * st + st * st + j] = new Vector2((float) (2*s-j )/ (3 * s), (float)i / (2 * s)); //Front 
                    uv[i * st + 2 * st * st + j] = new Vector2((float) (2*s-j) / (3 * s), (float)(i + s) / (2 * s)); //Right
                    uv[i * st + 3 * st * st + j] = new Vector2((float)(3*s-j) / (3 * s), (float)i / (2 * s)); //Back
                    uv[i * st + 4 * st * st + j] = new Vector2((float)(3*s-j) / (3 * s), (float)(i + s) / (2 * s)); //Top                
                    uv[i * st + 5 * st * st + j] = new Vector2((float)(s-j) / (3 * s), (float)i / (2 * s));  //Bottom            
                }
            }


        //Note: The seams is still visible

        RemoveSeamsForUVFB();

        mesh.uv = uv;

    }

    private void RemoveSeamsForUVFB()
    {
        int s = size;
        int st = s + 1;

        if (!mirror)
        {
            for (int i = 0; i <= s; i++)
            {
                // Columns
                uv[i * st].x = uv[i * st].x + (0.00001f * tolerance);   // Left -left side
                uv[i * st + s].x = uv[i * st + s].x - (0.00001f * tolerance);   // Left -right side
                uv[i * st + st * st].x = uv[i * st + st * st].x + (0.00001f * tolerance);   // Front -left side
                uv[i * st + st * st + s].x = uv[i * st + st * st + s].x - (0.00001f * tolerance);   // Front - right side

                 
                uv[i * st + 3 * st * st].x = uv[i * st + 3 * st * st].x + (0.00001f * tolerance);   // Back -right side
                uv[i * st + 2 * st * st + s].x = uv[i * st + 2 * st * st + s].x - (0.00001f * tolerance);   // Right -left side

                uv[i * st + 4 * st * st].x = uv[i * st + 4 * st * st].x + (0.00001f * tolerance);   // Top -right side
                uv[i * st + 5 * st * st + s].x = uv[i * st + 5 * st * st + s].x - (0.00001f * tolerance);   // Bottom -left side            

            }

            for (int j = 0; j <= s; j++)
            {
                // Rows
                uv[2 * st * st + j].y = uv[2 * st * st + j].y + (0.00001f * tolerance); // Right -down side
                uv[j].y = uv[j].y + (0.00001f * tolerance); // Left -down side

                uv[4 * st * st + j].y = uv[4 * st * st + j].y + (0.00001f * (tolerance + 28)); // Top -down side           
                uv[2 * st * st - st + j].y = uv[2 * st * st - st + j].y - (0.00001f * (tolerance + 28)); // Front -top side

                uv[6 * st * st - st + j].y = uv[6 * st * st - st + j].y - (0.00001f * tolerance); // Down -top side
                uv[4 * st * st - st + j].y = uv[4 * st * st - st + j].y - (0.00001f * (tolerance + 28)); // Back -top side        
            }
        }

        else
        {
            for (int i = 0; i <= s; i++)
            {
                // Columns
                uv[i * st].x = uv[i * st].x - (0.00001f * tolerance);   // Right -right side
                uv[i * st + s].x = uv[i * st + s].x + (0.00001f * tolerance);   // Right -left side
                uv[i * st + st * st].x = uv[i * st + st * st].x - (0.00001f * tolerance);   // Front - right side
                uv[i * st + st * st + s].x = uv[i * st + st * st + s].x + (0.00001f * tolerance);   // Front -left side

                uv[i * st + 2 * st * st].x = uv[i * st + 2 * st * st].x - (0.00001f * tolerance);   // Left -right side
                uv[i * st + 2 * st * st + s].x = uv[i * st + 2 * st * st + s].x + (0.00001f * tolerance);   // Left -left side

                uv[i * st + 3 * st * st].x = uv[i * st + 3 * st * st].x + (0.00001f * tolerance);   // Back -right side
                uv[i * st + 3 * st * st + s].x = uv[i * st + 3 * st * st + s].x + (0.00001f * tolerance);   // Back -left side                              
               
                uv[i * st + 4 * st * st + s].x = uv[i * st + 4 * st * st + s].x + (0.00001f * tolerance);   // Top -left side

                uv[i * st + 5 * st * st].x = uv[i * st + 5 * st * st].x - (0.00001f * tolerance);   // Bottom -right side 
                uv[i * st + 5 * st * st + s].x = uv[i * st + 5 * st * st + s].x + (0.00001f * tolerance);   // Bottom -left side          
            }
            for (int j = 0; j <= s; j++)
            {
                // Rows
                uv[2 * st * st + j].y = uv[2 * st * st + j].y + (0.00001f * tolerance); // Left -down side
                uv[j].y = uv[j].y + (0.00001f * tolerance); // Right -down side

                uv[4 * st * st + j].y = uv[4 * st * st + j].y + (0.00001f * (tolerance + 28)); // Top -down side           
                uv[2 * st * st - st + j].y = uv[2 * st * st - st + j].y - (0.00001f * (tolerance + 28)); // Front -top side

                uv[6 * st * st - st + j].y = uv[6 * st * st - st + j].y - (0.00001f * tolerance); // Down -top side
                uv[4 * st * st - st + j].y = uv[4 * st * st - st + j].y - (0.00001f * (tolerance + 28)); // Back -top side        
            }
        }
    }

    private void SetVertex(int i, int x, int y, int z)
    {
        Vector3 inner = vertices[i] = new Vector3(x, y, z);
        Vector3[] norm = new Vector3[vertices.Length];

        if (x < roundness)
        {
            inner.x = roundness;
        }
        else if (x > size - roundness) //xSize
        {
            inner.x = size - roundness;
        }
        if (y < roundness)
        {
            inner.y = roundness;
        }
        else if (y > size - roundness) //ySize
        {
            inner.y = size - roundness;
        }
        if (z < roundness)
        {
            inner.z = roundness;
        }
        else if (z > size - roundness) //zSize
        {
            inner.z = size - roundness;
        }

        norm[i] = (vertices[i] - inner).normalized;
        vertices[i] = inner + norm[i] * roundness;

    }

    private void SetVertexToFullSphere(int i, int x, int y, int z)
    {
        Vector3[] norm = new Vector3[vertices.Length];
        Vector3 v = new Vector3(x, y, z) * 2f / size - Vector3.one;
        float x2 = v.x * v.x;
        float y2 = v.y * v.y;
        float z2 = v.z * v.z;
        Vector3 s;
        

        s.x = v.x * Mathf.Sqrt(1f - y2 / 2f - z2 / 2f + y2 * z2 / 3f);
        s.y = v.y * Mathf.Sqrt(1f - x2 / 2f - z2 / 2f + x2 * z2 / 3f);
        s.z = v.z * Mathf.Sqrt(1f - x2 / 2f - y2 / 2f + x2 * y2 / 3f);

        norm[i] = s;
        vertices[i] = norm[i] * radius;
 
    }   // It will show weird texture, so don't use this

    private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }

    private void OnDrawGizmos()
    {
        Generate();

        if (vertices == null) { return; }

        //for (int i = 0; i < vertices.Length; i++)
        //{
        //    Gizmos.color = Color.red;
        //    //Gizmos.DrawSphere(vertices[i], 0.1f);
        //    Gizmos.DrawSphere(uv[i], 0.05f);
        //}

    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
