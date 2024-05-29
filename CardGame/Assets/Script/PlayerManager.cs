using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance; // Instance statique du gestionnaire de joueur
    public Player player; // Référence au joueur actuel

    void Awake()
    {
        // Vérifie s'il existe déjà une instance du gestionnaire de joueur
        if (instance == null)
        {
            // Si aucune instance n'existe, définir celle-ci comme l'instance actuelle
            instance = this;
            // Ne détruise pas ce GameObject lors du chargement d'une nouvelle scène
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si une instance existe déjà, détruire ce GameObject pour éviter les doublons
            Destroy(gameObject);
        }
    }
}
