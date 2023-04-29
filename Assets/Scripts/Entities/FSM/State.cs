using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T> : IState<T> //T es un parámetro genérico
{                              //la lista es un ejemplo de un T

    Dictionary<T, IState<T>> transitions = new Dictionary<T, IState<T>>();
    //tenemos un diccionario de inputs genéricos
    //que buscan un estado en ese "índice".

    public IState<T> GetTransition(T input)
    {
        if (transitions.ContainsKey(input))
        {
            return transitions[input];
        }
        return null;
    }

    public virtual void Awake() //el virtual es para poder modificarlo después.
    {

    }

    public virtual void Execute()
    {

    }

    public virtual void Sleep()
    {

    }

    public void AddTransition(T input, IState<T> state)
    {
        transitions[input] = state;
    }

    public void RemoveTransition(T input)
    {
        if (transitions.ContainsKey(input))
        {
            transitions.Remove(input);
        }
    }

    public void RemoveTransition(IState<T> state)
    {
        foreach (var item in transitions)
        {
            if (item.Value == state)
            {
                transitions.Remove(item.Key);
                break; //un break rompe un foreach. lo corta. lo mismo para for y while
            }
        }
    }
}
