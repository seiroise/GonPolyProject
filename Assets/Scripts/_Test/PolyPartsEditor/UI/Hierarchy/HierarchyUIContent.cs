using UnityEngine;
using UnityEngine.UI;
using Scripts._Test.PolyPartsEditor.Common;
using Seiro.Scripts.Graphics;
using Scripts._Test.PolyPartsEditor.Database;

namespace Scripts._Test.PolyPartsEditor.UI.Hierarchy {
	
	/// <summary>
	/// 階層表示用コンテンツ
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	class HierarchyUIContent : MonoBehaviour {

		[Header("UI")]
		public MeshImage meshImage;
		public Text text;

		[Header("Adjust")]
		public float maxWidth;
		public Vector2 spacing = new Vector2(5f, 5f);

		[Header("Test")]
		public PolyPartsDatabase db;

		private RectTransform rectTransform;
		private float height;

		#region UnityEvent

		private void Awake() {
			rectTransform = GetComponent<RectTransform>();
		}

		private void Start() {
			db.onPolyObjDrawed.AddListener(SetPolyObject);
		}

		#endregion

		#region Function

		/// <summary>
		/// 表示するポリゴンオブジェクトの設定
		/// </summary>
		public void SetPolyObject(PolyPartsObject polyObj) {
			if(meshImage) {

				//メッシュイメージの設定
				EasyMesh eMesh = polyObj.GetPolygonEasyMesh();
				meshImage.SetEasyMesh(eMesh);

				//各種調整
				Rect rect = polyObj.GetInclusionRect();

				//MeshImage倍率調整
				float scale = (maxWidth - (spacing.x * 2f)) / rect.width;
				meshImage.rectTransform.localScale = new Vector2(scale, scale);

				//位置調整
				this.height = rect.height * scale + (spacing.y * 2f);

				//大きさ調整
				Vector2 sizeDelta = rectTransform.sizeDelta;
				sizeDelta.y = height;
				rectTransform.sizeDelta = sizeDelta;
			}
		}

		#endregion
	}
}