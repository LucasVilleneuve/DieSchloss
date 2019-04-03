using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Timers;

public class DialogManager : Singleton<DialogManager>
{
    // It is not recommended to simply enqueue messages. Use AddMessage() for Evaluations
    private Queue<Message> messages = new Queue<Message>();
    private IEnumerator callback = null;

    private Message current = null;
    private string display = null;

    public DialogHandler handler = null;

    DialogManager()
    {
        // Create a timer with a two second interval.
        //aTimer = new System.Timers.Timer(3000);
        // Hook up the Elapsed event for the timer. 
        //aTimer.AutoReset = false;
    }


    // Called to add a new message for display.
    // Depending of its level, it will be dropped, it will replace the current message or it will be added to the queue for later display.
    // If it is added to the queue but not displayed within the given time, it will be dropped as well.
    // (Message will always be dropped and MandMsg will always replace)
    public void AddMessage(Message msg)
    {
        if (callback != null)
        {
            StopCoroutine(callback);
            callback = null;
        }
        Debug.Log("Adding new message: [" + msg.msg + "].");
        Message.Action tmp;

        if (current == null) {
            Debug.Log("default displaying");
            current = msg;
            updateDisplay();
        }
        else {
            tmp = msg.Evaluate(current.level);
            switch (tmp)
            {
                case Message.Action.Drop:
                    Debug.Log("dropping");
                    return;
                case Message.Action.Display:
                    Debug.Log("displaying");
                    current = msg;
                    updateDisplay();
                    break;
                case Message.Action.Wait:
                    Debug.Log("queuing");
                    messages.Enqueue(msg);
                    break;
            }
        }
        //aTimer.Elapsed += UpdateMessage;
        //aTimer.Enabled = true;
        callback = UpdateMessage(3);
        StartCoroutine(callback);
    }


    // Called whenever the current message expires.
    // If the queue is empty, the display turns off.
    // If not, the next element in the queue is evaluated (to see if it hasn't expired) and displayed.
    public IEnumerator UpdateMessage(float sec)
    {
        yield return new WaitForSeconds(sec);
        //aTimer.Enabled = false;
        Debug.Log("changing message.");
        if (messages.Count == 0)
        {
            current = null;
            this.updateDisplay("");
        }
        else
        {
            // check if the message's time limit was reached /!\
            current = messages.Dequeue();
            this.updateDisplay();
            callback = UpdateMessage(3);
            StartCoroutine(callback);
        }
    }


    // called to update the variable responsible of the display.
    // If no parameter is given, it will use the current message.
    public void updateDisplay(string tmp = null)
    {
        if (tmp == null && current != null) { display = current.msg; }
        else { display = tmp; }

        if (handler) { handler.setMessage(display); }
    }
}