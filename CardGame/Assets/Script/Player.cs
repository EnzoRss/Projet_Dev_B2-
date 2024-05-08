using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string surname;
    public Deck deck;
    public int pv = 20;

   public Player(string name)
    {
        this.surname = name;
    }
    public Player( Deck deck)
    {
        this.deck = deck;
    }
   
}
