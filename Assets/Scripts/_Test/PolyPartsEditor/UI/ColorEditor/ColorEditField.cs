using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Scripts._Test.PolyPartsEditor.UI.ColorEditor {
	
	/// <summary>
	/// 色編集フィールド
	/// </summary>
	class ColorEditField : MonoBehaviour {

		[Header("Color")]
		public Color initColor = Color.white;

		[Header("Preview")]
		public Image now;
		public Image next;

		[Header("RGB")]
		public NumEditField r;
		public NumEditField g;
		public NumEditField b;

		[Header("HexValue")]
		public InputField hexInput;

		[Header("Callback")]
		public ColorEditorEvent onColorChanged;

		private Color32 nextColor;

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
			if (r) {
				r.onNumChanged.AddListener(OnRedValueChanged);
			}
			if (g) {
				g.onNumChanged.AddListener(OnGreenValueChanged);
			}
			if (b) {
				b.onNumChanged.AddListener(OnBlueValueChanged);
			}
			if (hexInput) {
				hexInput.onEndEdit.AddListener(OnHexValueEndEdit);
			}
			SetColor(initColor);
		}

		/// <summary>
		/// 色の設定
		/// </summary>
		public void SetColor(Color32 color) {
			SetNowColor(color);
			SetRGBValue(color);
			SetHexValue(color);
		}

		/// <summary>
		/// 現在の色を設定
		/// </summary>
		private void SetNowColor(Color32 color) {
			if (now) {
				now.color = color;
			}
			SetNextColor(color);
		}

		/// <summary>
		/// 次の色を設定
		/// </summary>
		private void SetNextColor(Color32 color) {
			if (next) {
				next.color = color;
			}
			nextColor = color;
		}

		/// <summary>
		/// RGB値を設定
		/// </summary>
		private void SetRGBValue(Color32 color) {
			if (r) {
				r.SetValue(color.r);
			}
			if (g) {
				g.SetValue(color.g);
			}
			if (b) {
				b.SetValue(color.b);
			}
		}

		/// <summary>
		/// 16進数表記を設定
		/// </summary>
		private void SetHexValue(Color32 color) {
			if (hexInput) {
				hexInput.text = ColorUtility.ToHtmlStringRGB(color);
			}
		}

		/// <summary>
		/// 変更コールバックの起動
		/// </summary>
		private void InvokeChangeCallback() {
			onColorChanged.Invoke(nextColor);
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 赤色の変更
		/// </summary>
		private void OnRedValueChanged(int value) {
			nextColor.r = (byte)value;
			SetNextColor(nextColor);
			SetHexValue(nextColor);
			//コールバック
			InvokeChangeCallback();
		}

		/// <summary>
		/// 緑色の変更
		/// </summary>
		private void OnGreenValueChanged(int value) {
			nextColor.g = (byte)value;
			SetNextColor(nextColor);
			SetHexValue(nextColor);
			//コールバック
			InvokeChangeCallback();
		}

		/// <summary>
		/// 青色の変更
		/// </summary>
		private void OnBlueValueChanged(int value) {
			nextColor.b = (byte)value;
			SetNextColor(nextColor);
			SetHexValue(nextColor);
			//コールバック
			InvokeChangeCallback();
		}

		/// <summary>
		/// 16進表記の変更
		/// </summary>
		private void OnHexValueEndEdit(string value) {
			Color temp;
			if (ColorUtility.TryParseHtmlString("#" + value, out temp)) {
				SetNextColor(temp);
				SetRGBValue(temp);
				//コールバック
				InvokeChangeCallback();
			}
		}

		#endregion
	}
}