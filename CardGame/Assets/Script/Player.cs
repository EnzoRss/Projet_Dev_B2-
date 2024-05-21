using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ushort id;
    public string username;
    public string password;
    public Deck deck;
    public int pv = 20;

   public Player(string name,string password)
    {
        this.username = name;
        this.password = password;
    }
    public Player( Deck deck)
    {
        this.deck = deck;
    }
   
    public void PrintPlayer()
    {
        Debug.Log(this.id);
        Debug.Log(this.username);
        Debug.Log(this.password);
        Debug.Log(this.deck);
    }
}
