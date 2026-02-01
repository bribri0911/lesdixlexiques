using UnityEngine;

public class GetMask : MonoBehaviour
{
    public int nbrMaskMax = 3;
    public GameObject[] maskForPlayer;
    public GameObject child;
    public int indexMask = 0;

    void Start()
    {
        maskForPlayer = new GameObject[nbrMaskMax];
    }

    public void AddMask(GameObject prefabMask)
    {
        if (!TryGetIndexNullForMask(out int indexGood))
        {
            indexGood = indexMask;
            // On dépose l'ancien masque au sol avant d'en prendre un nouveau
            RemoveMask(indexGood);
        }

        // On change le parent vers le slot du joueur
        prefabMask.transform.SetParent(child.transform);
        prefabMask.transform.localPosition = Vector3.zero; 
        prefabMask.name = $"Mask_{indexGood}";
        
        maskForPlayer[indexGood] = prefabMask;
        
        // On met à jour l'affichage (active le nouveau, désactive les autres)
        UpdateActiveMask(indexGood);
    }

    public bool TryGetIndexNullForMask(out int index)
    {
        for (int i = 0; i < nbrMaskMax; i++)
        {
            if (maskForPlayer[i] == null)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

    public void RemoveMask(int index)
    {
        if (index >= 0 && index < nbrMaskMax && maskForPlayer[index] != null)
        {
            GameObject maskToDrop = maskForPlayer[index];

            // 1. Déterminer le parent de destination
            Transform worldParent = null;
            if (GeneratMack.Instance != null && GeneratMack.Instance.parentOfMask != null)
                worldParent = GeneratMack.Instance.parentOfMask.transform;

            // 2. Sortir de l'inventaire
            maskToDrop.transform.SetParent(worldParent);

            // 3. Placer au sol et SURTOUT le réactiver pour qu'il soit visible
            maskToDrop.transform.position = transform.position;
            maskToDrop.SetActive(true); 

            // 4. Le rendre à nouveau détectable par le système de ramassage
            if (GeneratMack.Instance != null)
            {
                GeneratMack.Instance.activeMasksOnFloor.Add(maskToDrop);
            }

            // 5. Vider le slot
            maskForPlayer[index] = null;
            
            Debug.Log($"📦 Masque {maskToDrop.name} déposé au sol.");
        }
    }

    public void ChangeMask(int direction)
    {
        int count = 0;
        for (int i = 0; i < nbrMaskMax; i++) if (maskForPlayer[i] != null) count++;
        
        if (count < 2) return;

        int nextIndex = indexMask;
        int attempts = 0;
        do
        {
            nextIndex = (nextIndex + direction + nbrMaskMax) % nbrMaskMax;
            attempts++;
        } while (maskForPlayer[nextIndex] == null && attempts < nbrMaskMax);

        UpdateActiveMask(nextIndex);
    }

    private void UpdateActiveMask(int newIndex)
    {
        indexMask = newIndex;
        for (int i = 0; i < nbrMaskMax; i++)
        {
            if (maskForPlayer[i] != null)
            {
                // Seul le masque sélectionné est visible sur le joueur
                maskForPlayer[i].SetActive(i == indexMask);
            }
        }
    }

    public void UseMaskActive()
    {
        if (maskForPlayer[indexMask] != null)
        {
            UseEffect currentMaskScript = maskForPlayer[indexMask].GetComponent<UseEffect>();
            if (currentMaskScript != null)
            {
                currentMaskScript.TryUse();
            }
        }
    }
}