using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance; // Instance statique du gestionnaire de joueur
    public Player player; // R�f�rence au joueur actuel

    void Awake()
    {
        // V�rifie s'il existe d�j� une instance du gestionnaire de joueur
        if (instance == null)
        {
            // Si aucune instance n'existe, d�finir celle-ci comme l'instance actuelle
            instance = this;
            // Ne d�truise pas ce GameObject lors du chargement d'une nouvelle sc�ne
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si une instance existe d�j�, d�truire ce GameObject pour �viter les doublons
            Destroy(gameObject);
        }
    }
}
