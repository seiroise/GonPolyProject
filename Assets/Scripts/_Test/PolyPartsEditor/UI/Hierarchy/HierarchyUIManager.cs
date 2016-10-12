using UnityEngine;
using System.Collections.Generic;
using Scripts._Test.PolyPartsEditor.Common;

namespace Scripts._Test.PolyPartsEditor.UI.Hierarchy {

	/// <summary>
	/// ポリゴンパーツ階層のUI表示
	/// </summary>
	class HierarchyUIManager : MonoBehaviour {

		[Header("Editor")]
		public PolyPartsEditor editor;

		[Header("Content")]
		public RectTransform contentsParent;

		[Header("Prefab")]
		public HierarchyUIContent prefab;

		private Dictionary<PolyPartsObject, HierarchyUIContent> polyObjTable;

		#region UnityEvent

		private void Awake() {
			polyObjTable = new Dictionary<PolyPartsObject, HierarchyUIContent>();
		}

		private void Start() {
			//コールバックの設定
			if(editor) {
				editor.database.onPolyObjInstantiated.AddListener(OnPolyObjInstantiated);
				editor.database.onPolyObjDeleted.AddListener(OnPolyObjDeleted);
				editor.database.onPolyObjVertexChanged.AddListener(OnPolyObjVertexChanged);
				editor.database.onPolyObjColorChanged.AddListener(OnPolyObjColorChanged);
			}
		}

		#endregion

		#region Function

		/// <summary>
		/// Contentの生成
		/// </summary>
		private HierarchyUIContent InstantiateContent(PolyPartsObject polyObj) {
			HierarchyUIContent content = Instantiate(prefab);
			content.SetPolyObj(polyObj);
			content.meshImage.SetEasyMesh(polyObj.GetPolygonEasyMesh());
			//transformの設定
			RectTransform rectTrans = (RectTransform)content.transform;
			rectTrans.SetParent(contentsParent, false);
			//上に追加
			rectTrans.SetAsFirstSibling();
			return content;
		}

		/// <summary>
		/// コンテンツの追加
		/// </summary>
		private void AddContent(PolyPartsObject polyObj) {
			HierarchyUIContent content = InstantiateContent(polyObj);
			polyObjTable.Add(polyObj, content);
		}

		#endregion

		#region Callback

		/// <summary>
		/// ポリゴンオブジェクトの生成
		/// </summary>
		private void OnPolyObjInstantiated(PolyPartsObject polyObj) {
			//Contentの追加
			AddContent(polyObj);
		}

		/// <summary>
		/// ポリゴンオブジェクトの削除
		/// </summary>
		private void OnPolyObjDeleted(PolyPartsObject polyObj) {
			//Contentの取得
			if (!polyObjTable.ContainsKey(polyObj)) return;
			HierarchyUIContent content = polyObjTable[polyObj];
			//Contentの削除
			Destroy(content.gameObject);
		}

		/// <summary>
		/// ポリゴンオブジェクトの頂点変更
		/// </summary>
		private void OnPolyObjVertexChanged(PolyPartsObject polyObj) {
			//Contentの取得
			if (!polyObjTable.ContainsKey(polyObj)) return;
			HierarchyUIContent content = polyObjTable[polyObj];
			content.meshImage.SetEasyMesh(polyObj.GetPolygonEasyMesh());
		}

		/// <summary>
		/// ポリゴンオブジェクトの色変更
		/// </summary>
		private void OnPolyObjColorChanged(PolyPartsObject polyObj) {
			//Contentの取得
			if (!polyObjTable.ContainsKey(polyObj)) return;
			HierarchyUIContent content = polyObjTable[polyObj];
			content.meshImage.SetColor(polyObj.GetPolygonColor());
		}

		#endregion

		#region UICallback

		/// <summary>
		/// コンテントのクリック
		/// </summary>
		private void OnContentClick(HierarchyUIContent content) {
		
		}

		#endregion
	}
}