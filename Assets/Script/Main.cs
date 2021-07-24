using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bookread;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        UGUIManager.I.Open<BookList>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
