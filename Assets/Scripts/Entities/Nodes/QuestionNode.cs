using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : INode
{

    //public delegate bool MyDelegate(); //no pide nada, delevuelve un boleano
    //MyDelegate _question;

    //public Action _question; //este puede recibir variables en invokar. pero no devuelve nada, 

    public Func<bool> _question; //Es lo mismo, recibe hasta 16 variables  pero el ultimo declarado es lo que devuelve. Si queremos que devuelva un bool y no reciba nada... es el ejemplo actual

    private INode _trueNode;
    private INode _falseNode;

    public QuestionNode(Func<bool> question, INode trueNode, INode falseNode) 
    {
        _question = question;
        _trueNode = trueNode;
        _falseNode = falseNode;
    }

    public void Execute() 
    {
        if (_question())
        {
            _trueNode.Execute();
        }
        else 
        {
            _falseNode.Execute();
        }
    }
}
