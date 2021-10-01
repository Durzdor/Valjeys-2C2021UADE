using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory
{
    private object value;

    public Memory(object value)
    {
        this.value = value;
    }

    public object GetValue()
    {
        return value;
    }

    public void SetValue(object newValue)
    {
        value = newValue;
    }
}


public class Blackboard : MonoBehaviour
{
    Dictionary<string, Memory> memories = new Dictionary<string, Memory>();

    public Dictionary<string, Memory> Memories { get => memories; set => memories = value; }

    public object Get(string memoryName)
    {
        return memories.ContainsKey(memoryName) ? memories[memoryName].GetValue() : null;
    }

    public void Set(string memoryName, object memoryValue)
    {
        if (memories.ContainsKey(memoryName))
        {
            memories[memoryName].SetValue(memoryValue);
        }
        else
        {
            var newMemory = new Memory(memoryValue);
            memories.Add(memoryName, newMemory);
        }
    }
}
