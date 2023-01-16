using TMPro;
using UnityEngine;

public class DropdownSample : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField]
	private TextMeshProUGUI text;

	[SerializeField]
	private TMP_Dropdown dropdownWithoutPlaceholder;

	[SerializeField]
	private TMP_Dropdown dropdownWithPlaceholder;

	#endregion
	#region Public Methods

	public void OnButtonClick() => this.text.text = this.dropdownWithPlaceholder.value > -1
		? "Selected values:\n" + this.dropdownWithoutPlaceholder.value + " - " + this.dropdownWithPlaceholder.value
		: "Error: Please make a selection";

	#endregion
}
