using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Scripts._Test.PolyPartsEditor.UI.ColorAdjuster {

	/// <summary>
	/// 数値入力フィールド
	/// </summary>
	public class NumericInputField : MonoBehaviour {

		[Header("UI")]
		public InputField inputField;
		public Slider slider;

		#region UnityEvent

		private void Start() {
			Initialize();
		}

		#endregion

		#region Function

		/// <summary>
		/// 初期化
		/// </summary>
		private void Initialize() {
			//UIコールバックの設定
			if(inputField) {
				inputField.onValueChanged.AddListener(OnInputValueChanged);
				inputField.onEndEdit.AddListener(OnEndEdit);
			}
			if(slider) {
				slider.onValueChanged.AddListener(OnSliderValueChanged);
			}
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 入力欄の入力値変更
		/// </summary>
		private void OnInputValueChanged(string value) {
		}

		/// <summary>
		/// 入力完了
		/// </summary>
		private void OnEndEdit(string value) {

		}

		/// <summary>
		/// スライダーの入力値変更
		/// </summary>
		private void OnSliderValueChanged(float value) {
			
		}

		#endregion
	}
}