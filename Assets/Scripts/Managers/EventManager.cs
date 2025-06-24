using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager<T>
{

  public event Action<T> OnSend; // ? T not declared
    
}

public class EventData
{
    public Card card;
    public Enemy target;

    public EventData(Card card, Enemy target)
    {
        this.card = card;
        this.target = target;
    }
}