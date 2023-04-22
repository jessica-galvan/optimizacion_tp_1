using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    //no solamente dice "ahora transisionamos de estado"
    //sino que también llama a las funciones DENTRO
    //del estado.

    IState<T> current;
    //CONSTRUCTOR: primera función que se ejecuta.
    //recopila la info necesaria para isntanciarse.
    //se pueden tener varios.

    public FSM() //ESTE es un constructor.
                 //lo que va entre () es lo que necesita para crearse.
    {

    }

    public FSM(IState<T> initialState)
    {
        SetInit(initialState);
    }

    public void SetInit(IState<T> initialState)
    {
        //acá ya tenemos el primer estado.
        current = initialState;
        //llamamos a su awake
        current.Awake();
    }

    public void OnUpdate()
    {
        //hay que setear el primer estado
        //porque tenemos las transiciones,
        //pero al estado viejo aún no lo seteamos.
        //siempre tiene que tener un estado
        if (current != null)
            current.Execute();
    }

    public void Transition(T input)
    {
        IState<T> newState = current.GetTransition(input);
        if (newState == null)
            //no hay transición
            return;

        //pero si sí hay transición,
        //se llama al sleep del current 
        current.Sleep();
        //el actual pasa a ser el nuevp
        current = newState;
        //le hacemos el awake
        current.Awake();
    }
}
