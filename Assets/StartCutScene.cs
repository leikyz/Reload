using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StartCutScene : MonoBehaviour
{
    private PlayableDirector playable;
    [SerializeField] private List<GameObject> props = new List<GameObject>();
    void Start()
    {
        playable = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playable.time > 35f)
        {
            foreach (GameObject obj in props) 
                obj.transform.SetParent(null, true);
        }

    }
}
