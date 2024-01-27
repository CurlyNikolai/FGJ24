using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectHat : MonoBehaviour
{
    [SerializeField]
    private Transform[] hats;

    [SerializeField]
    private int currentIndex;
    private int maxNumberHats;

    [SerializeField]
    private bool nextHat;

    // Start is called before the first frame update
    void Start()
    {
        maxNumberHats = transform.childCount;
        Debug.Log("Number of hats: " + maxNumberHats);
        hats = new Transform[maxNumberHats];

        for (int i = 0 ; i < maxNumberHats; i++) {
            hats[i] = transform.GetChild(i);
        }
        
        InputController.RequestFire += ChangeHat;
    }

    void ChangeHat(int index) {
        hats[currentIndex].gameObject.SetActive(false);

        currentIndex++;
        currentIndex %= maxNumberHats;

        hats[currentIndex].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (nextHat) {
            ChangeHat(0);
            nextHat = false;
        }
    }
}
