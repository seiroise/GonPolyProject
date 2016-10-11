using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace Scripts._Test.PolyPartsEditor.UI.ColorEditor {

	/// <summary>
	/// 数値編集フィールド
	/// </summary>
	public class NumEditField : MonoBehaviour {

		/// <summary>
		/// 変更通知イベント
		/// </summary>
		[Serializable]
		public class OnNumChanged : UnityEvent<int> { }

		[Header("UI")]
		public InputField inputField;
		public Slider slider;

		[Header("Numeric Range")]
		public int numRange = 255;

		[Header("Callback")]
		public OnNumChanged onNumChanged;

		private int value = 0;

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
			//UI設定
			if(inputField) {
				inputField.onValueChanged.AddListener(OnInputValueChanged);
				inputField.onEndEdit.AddListener(OnInputEndEdit);
			}
			if(slider) {
				slider.onValueChanged.AddListener(OnSliderValueChanged);
				slider.minValue = 0f;
				slider.maxValue = numRange;
			}
		}

		/// <summary>
		/// 値の設定
		/// </summary>
		public void SetValue(int value) {
			//範囲内に収める
			value = Mathf.Clamp(value, 0, numRange);
			
			//UIに反映
			if (inputField) {
				inputField.text = value.ToString();
			}
			if (slider) {
				slider.value = value;
			}

			this.value = value;
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 入力欄の入力値変更
		/// </summary>
		private void OnInputValueChanged(string value) {
			int result;
			if (int.TryParse(value, out result)) {
				SetValue(result);
			}
			//コールバック
			onNumChanged.Invoke(this.value);
		}

		/// <summary>
		/// 入力完了
		/// </summary>
		private void OnInputEndEdit(string value) {
			OnInputValueChanged(value);
		}

		/// <summary>
		/// スライダーの入力値変更
		/// </summary>
		private void OnSliderValueChanged(float value) {
			SetValue((int)value);
			//コールバック
			onNumChanged.Invoke(this.value);
		}

		#endregion
	}
}