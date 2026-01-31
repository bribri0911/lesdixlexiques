using UnityEngine;

public class DebugEventVisualizer : MonoBehaviour
{
    void OnEnable()
    {
        // On s'abonne à tous les événements pour les voir dans la console
        WebsocketManage.OnMoovePlayer += (id, dir) => Debug.Log($"<color=cyan>[EVENT]</color> Moove: User {id} move to {dir}");
        WebsocketManage.OnUseMask += (id) => Debug.Log($"<color=yellow>[EVENT]</color> UseMask: User {id}");
        WebsocketManage.OnChangeMaskToLeft += (id) => Debug.Log($"<color=orange>[EVENT]</color> Mask Left: User {id}");
        WebsocketManage.OnChangeMaskToRight += (id) => Debug.Log($"<color=orange>[EVENT]</color> Mask Right: User {id}");
        WebsocketManage.OnGetMask += (id) => Debug.Log($"<color=green>[EVENT]</color> Get Mask: User {id}");
    }

    void OnDisable()
    {
        // Toujours se désabonner pour éviter les fuites de mémoire
        // (Même si ici c'est pour du debug)
    }
}