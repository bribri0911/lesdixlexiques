using Unity.VisualScripting;
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
            RemoveMask(indexGood);
        }

        GameObject maskInstance = Instantiate(prefabMask, child.transform);
        maskInstance.name = $"Mask_{indexGood}";
        maskForPlayer[indexGood] = maskInstance;

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
            Destroy(maskForPlayer[index]);
            maskForPlayer[index] = null;
        }
    }

    public void ChangeMask(int direction)
    {
        int count = 0;
        for (int i = 0; i < nbrMaskMax; i++) if (maskForPlayer[i] != null) count++;
        
        if (count < 2) return;

        int nextIndex = indexMask;
        do
        {
            nextIndex = (nextIndex + direction + nbrMaskMax) % nbrMaskMax;
        } while (maskForPlayer[nextIndex] == null);

        UpdateActiveMask(nextIndex);
    }

    private void UpdateActiveMask(int newIndex)
    {
        indexMask = newIndex;
        for (int i = 0; i < nbrMaskMax; i++)
        {
            if (maskForPlayer[i] != null)
            {
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