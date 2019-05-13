using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    // It is not recommended to simply enqueue messages. Use AddMessage() for Evaluations
    private Queue<Message> messages = new Queue<Message>();
    private IEnumerator callback = null;
    private IEnumerator randcallback = null;

    private Message current = null;
    private string display = null;

    public SoundHandler handler = null;

    private List<string> RandomNoises;


}