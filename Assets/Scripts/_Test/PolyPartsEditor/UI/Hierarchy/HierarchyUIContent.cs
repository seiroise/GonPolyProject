using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using Scripts._Test.PolyPartsEditor.Common;
using Seiro.Scripts.Graphics;
using Scripts._Test.PolyPartsEditor.Database;

namespace Scripts._Test.PolyPartsEditor.UI.Hierarchy {
	
	/// <summary>
	/// 階層表示用コンテンツ
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	class HierarchyUIContent : MonoBehaviour {

		[Serializable]
		public class ContentEvent : UnityEvent<HierarchyUIContent> { }

		[Header("UI")]
		public MeshImage meshImage;
		public Button button;
		public Text text;

		[Header("Callback")]
		public ContentEvent onClick;

		private RectTransform rectTrans;
		private PolyPartsObject polyObj;

		#region UnityEvent

		private void Awake() {
			rectTrans = GetComponent<RectTransform>();
		}

		private void Start() {
			if (button) {
				button.onClick.AddListener(OnClick);
			}
		}

		#endregion

		#region Function

		/// <summary>
		/// ポリゴンオブジェクトの設定
		/// </summary>
		public void SetPolyObj(PolyPartsObject polyObj) {
			this.polyObj = polyObj;
		}

		/// <summary>
		/// ポリゴンオブジェクトを取得
		/// </summary>
		public PolyPartsObject GetPolyObj() {
			return polyObj;
		}

		#endregion

		#region UICallback

		/// <summary>
		/// ボタンのクリック
		/// </summary>
		private void OnClick() {
			onClick.Invoke(this);
		}

		#endregion
	}
}