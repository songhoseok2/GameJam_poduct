using UnityEngine;
using System.Collections;

public class DialogScript : MonoBehaviour
{
    public void showDialog(GameObject stuffToDialog)
    {
        Instantiate<GameObject>(stuffToDialog);
    }
}
