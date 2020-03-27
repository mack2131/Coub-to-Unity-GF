using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public AudioSource punchSource;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        AudioSource[] ass = GetComponents<AudioSource>();
        punchSource = ass[0];
    }

    public void PlayPunch()
    {
        punchSource.Play();
    }
}
