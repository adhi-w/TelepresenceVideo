using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class VideoController : MonoBehaviour
{
    private VideoPlayer vidPlayer;
    public float videoFramerate;
    public float frame;    
    public float video_index_time;

    


    // Start is called before the first frame update
    void Start()
    {
        vidPlayer = GetComponent<VideoPlayer>();
        //vidPlayer.url = "file://D:/Videos (D)/Dolly Robot Vid/TW20_Q8K_20220119_200613_000000.mp4";
    }

    // Update is called once per frame
    void Update()
    {         
        video_index_time = (float)vidPlayer.time;  //video current/index time when it is playing
        frame = vidPlayer.frame;
        videoFramerate = (float)vidPlayer.frameRate;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (vidPlayer.isPlaying) vidPlayer.Pause();
            else if (vidPlayer.isPaused) vidPlayer.Play();
        }

    }
}
