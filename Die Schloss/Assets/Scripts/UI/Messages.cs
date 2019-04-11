using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Message
{

    public enum Action { Display, Wait, Drop };

    public enum Level { Low = 0, Medium = 1, High = 2, Mandatory = 3 };

    public Message(string message)
    {
        msg = message;
    }

    // Check if we display the message, keep it or drop it
    public virtual Action Evaluate(Level last)
    {
        UnityEngine.Debug.Log("base evaluate");
        return Action.Drop;
    }

    // data
    public Stopwatch sw;
    public string msg;

    // overridden values
    public float keepFor = 0.0F;
    public Level level = Level.Low;
}


public class MedMsg : Message
{
    public MedMsg(string message) : base(message)
    {
        keepFor = 2.0F;
        level = Level.Medium;
        sw = Stopwatch.StartNew();
    }

    public override Action Evaluate(Level last)
    {
        UnityEngine.Debug.Log("med evaluate");
        if (sw.Elapsed.TotalSeconds > keepFor)
            return Action.Drop;
        if (last < Level.Medium)
            return Action.Display;
        return Action.Wait;
    }
}


public class HighMsg : Message
{
    public HighMsg(string message) : base(message)
    {
        keepFor = 4.0F;
        level = Level.High;
        sw = Stopwatch.StartNew();
    }

    public override Action Evaluate(Level last)
    {
        if (sw.Elapsed.TotalSeconds > keepFor)
            return Action.Drop;
        if (last < Level.High)
            return Action.Display;
        return Action.Wait;
    }
}


public class MandMsg : Message
{

    public MandMsg(string message) : base(message)
    {
        level = Level.Mandatory;
    }

    public override Action Evaluate(Level last)
    {
        return Action.Display;
    }
}
