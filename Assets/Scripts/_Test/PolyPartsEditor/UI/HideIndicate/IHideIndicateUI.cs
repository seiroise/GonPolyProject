using UnityEngine;

namespace Scripts._Test.PolyPartsEditor.UI.HideIndicate {

	/// <summary>
	/// 表示と非表示が可能なUI
	/// </summary>
	public interface IHideIndicateUI<T> where T : MonoBehaviour {

		/// <summary>
		/// 表示
		/// </summary>
		void Indicate();

		/// <summary>
		/// 非表示
		/// </summary>
		void Hide();
	}
}