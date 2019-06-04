using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;


public class SoundHandler : MonoBehaviour
{
    // The list of clips to be played randomly
    public List<AudioClip> Clips = new List<AudioClip>();

    // Everything necessary to the extraction of sounds stored in the Resources folder.
    private string absolutePath = "Assets/Resources/Sounds/ambient/";
    private string assetsPath = "Sounds/ambient/";
    private FileInfo[] soundFiles;

    
    // Variables to handle the periodic ambient sounds
    private System.Collections.IEnumerator randcallback = null;
    public float[] timeRange = new float[] { 25, 50 };

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        // LoadSounds();
        PlayDelayedSound();
    }

    bool IsValidFileType(string fileName)
    {
        string tmp = Path.GetExtension(fileName);
        return (tmp == ".ogg" || tmp == ".wav" || tmp == ".mp3");
    }

    private void LoadSounds()
    {
        Clips.Clear();
        // get all valid files
        var info = new DirectoryInfo(absolutePath);
        soundFiles = info.GetFiles()
            .Where(f => IsValidFileType(f.Name))
            .ToArray();

        // and load them
        foreach (var s in soundFiles)
            LoadFile(assetsPath + s.Name.Substring(0, s.Name.Length - 4));
    }

    private void LoadFile(string path)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip)
        {
            clip.name = Path.GetFileName(path);
            Clips.Add(clip);
        }
        else
        {
            Debug.Log("failed to load: " + path);
        }
    }


    public System.Collections.IEnumerator PlayRandomSound(float sec)
    {
        //Debug.Log("playing sound in " + sec + "s.");
        yield return new WaitForSeconds(sec);
        //Debug.Log("playing sound.");
        AudioClip clip = Clips[UnityEngine.Random.Range(0, Clips.Count)];
        if (clip)
            audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        //Debug.Log("Done.");
        PlayDelayedSound();
    }


    private void PlayDelayedSound()
    {
        randcallback = PlayRandomSound(UnityEngine.Random.Range(timeRange[0], timeRange[1]));
        StartCoroutine(randcallback);
    }
}
