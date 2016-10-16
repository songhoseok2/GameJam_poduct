using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonScript : MonoBehaviour
{
	public int buttonID = -1;

	public void clicked()
	{
		Utils.GameManager().SetSelection(buttonID);
	}

}
