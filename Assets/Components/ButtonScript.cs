using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonScript : MonoBehaviour
{
    public char buttonID;
    // Use this for initialization
	
	public void clicked()
    {
        Utils.GameManager().SetSelection(buttonID);
    }

}
