using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Les attributs publics de la classe Player
    public ushort id; // Identifiant unique du joueur
    public string username; // Nom d'utilisateur du joueur
    public string password; // Mot de passe du joueur
    public Deck deck; // Instance de la classe Deck représentant le deck du joueur
    public int pv = 20; // Points de vie du joueur, initialisés à 20 par défaut

    // Constructeur prenant un nom d'utilisateur et un mot de passe en paramètres
    public Player(string name, string password)
    {
        this.username = name; // Initialisation du nom d'utilisateur
        this.password = password; // Initialisation du mot de passe
    }

    // Constructeur prenant un deck en paramètre
    public Player(Deck deck)
    {
        this.deck = deck; // Initialisation du deck
    }

    // Méthode pour afficher les informations du joueur
    public void PrintPlayer()
    {
        Debug.Log(this.id); // Affichage de l'identifiant du joueur
        Debug.Log(this.username); // Affichage du nom d'utilisateur du joueur
        Debug.Log(this.password); // Affichage du mot de passe du joueur
        Debug.Log("Deck : " + this.deck); // Affichage de l'état du deck du joueur
    }
}
