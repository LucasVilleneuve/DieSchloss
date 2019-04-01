using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class DialogManager : Singleton<DialogManager>
{
    // It is not recommended to simply enqueue messages. Use AddMessage() for Evaluations
    public Queue<Message> messages = new Queue<Message>();
    public Message current = null;
    public string display = "flibo";

    public DialogHandler handler = null;

    //void Start()
    //{
    //    //////////
    //    //// test
    //    current = new Message("bonjour");
    //    Thread.Sleep(1800);

    //    AddMEssage(new HighMsg("toma Suce"));

    //    AddMEssage(new MedMsg("cuit"));
    //    this.UpdateMessage();

    //    //// end
    //    //////////
    //}


    // Called to add a new message for display.
    // Depending of its level, it will be dropped, it will replace the current message or it will be added to the queue for later display.
    // If it is added to the queue but not displayed within the given time, it will be dropped as well.
    // (Message will always be dropped and MandMsg will always replace)
    public void AddMessage(Message msg)
    {
        Debug.Log("Adding new message: [" + msg.msg + "].");
        Message.Action tmp;
        
        if (current == null) {
            //Debug.Log("displaying");
            current = msg;
            this.updateDisplay();
        }
        else {
            tmp = msg.Evaluate(current.level);
            switch (tmp)
            {
                case Message.Action.Drop:
                    //Debug.Log("dropping");
                    return;
                case Message.Action.Display:
                    //Debug.Log("displaying");
                    current = msg;
                    this.updateDisplay();
                    break;
                case Message.Action.Wait:
                    //Debug.Log("queuing");
                    messages.Enqueue(msg);
                    break;
            }
        }
    }


    // Called whenever the current message expire.
    // If the queue is empty, the display turns off.
    // If not, the next element in the queue is evaluated (to see if it hasn't expired) and displayed.
    public void UpdateMessage()
    {
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

//public class MyClass : MonoBehaviour
//{
//    private void OnEnable()
//    {
//        Debug.Log(MySingleton.Instance.MyTestString);
//    }
//}