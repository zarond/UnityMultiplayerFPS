using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teamcolors : MonoBehaviour
{
    public const int numberOfColors=5; 
    public Color[] teamcols = new Color[numberOfColors]; // цвета, назначаемые игрокам разных команд, начиная с -1
    public Color chosencolor;

    health hlth;
    // Start is called before the first frame update
    void Start()
    {
        hlth = GetComponent<health>();
        chosencolor = teamcols[(hlth.teamid + 1 < numberOfColors) ?(hlth.teamid + 1):(numberOfColors-1)];
        if (this.gameObject.CompareTag("Player")) {
            this.transform.GetChild(2).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.color = chosencolor;
            this.transform.GetChild(3).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.color = chosencolor;
            this.transform.GetChild(1).GetChild(1).GetChild(3).GetComponent<SkinnedMeshRenderer>().material.color = chosencolor;
        }
        else if (this.gameObject.CompareTag("Character")) {
            this.transform.GetChild(1).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.color = chosencolor;
        }
    }

}
