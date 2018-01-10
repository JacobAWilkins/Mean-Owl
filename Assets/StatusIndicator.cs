using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

	[SerializeField]
	private RectTransform healthBar;

	public void SetHealth (int _current, int _max) {
		float _value = (float)_current / _max;
		healthBar.localScale = new Vector3 (_value, healthBar.localScale.y, healthBar.localScale.z);
	}
}
