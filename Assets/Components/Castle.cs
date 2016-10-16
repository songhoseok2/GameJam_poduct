using UnityEngine;
using System.Collections;

public class Castle : MonoBehaviour {

	void OnDestroy() {
		if (Utils.GameManager()) {
			Utils.GameManager().GameOver();
		}
	}

}
