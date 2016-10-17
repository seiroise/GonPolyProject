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

		[Header("Alignment")]
		public PolyPartsAlignment alignment;

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
		/// コンテントの追加
		/// </summary>
		private void AddContent(PolyPartsObject polyObj) {
			HierarchyUIContent content = InstantiateContent(polyObj);
			//コンテントの追加
			polyObjTable.Add(polyObj, content);
		}

		/// <summary>
		/// コンテントの削除
		/// </summary>
		private void DelteContent(PolyPartsObject polyObj) {
			if (!polyObjTable.ContainsKey(polyObj)) return;
			polyObjTable.Remove(polyObj);
			//コンテントの削除
			HierarchyUIContent content = polyObjTable[polyObj];
			Destroy(content.gameObject);
		}

		/// <summary>
		/// コンテント内のポリゴンの頂点変更
		/// </summary>
		private void PolyObjVertexChanged(PolyPartsObject polyObj) {
			if (!polyObjTable.ContainsKey(polyObj)) return;
			HierarchyUIContent content = polyObjTable[polyObj];
			//コンテントのメッシュを再設定
			content.meshImage.SetEasyMesh(polyObj.GetPolygonEasyMesh());
		}

		/// <summary>
		/// コンテント内のポリゴンの色変更
		/// </summary>
		private void PolyObjColorChanged(PolyPartsObject polyObj) {
			//Contentの取得
			if (!polyObjTable.ContainsKey(polyObj)) return;
			HierarchyUIContent content = polyObjTable[polyObj];
			//コンテントのメッシュの色を再設定
			content.meshImage.SetColor(polyObj.GetPolygonColor());
		}

		#endregion

		#region Callback

		/// <summary>
		/// ポリゴンオブジェクトの生成
		/// </summary>
		private void OnPolyObjInstantiated(PolyPartsObject polyObj) {
			AddContent(polyObj);
		}

		/// <summary>
		/// ポリゴンオブジェクトの削除
		/// </summary>
		private void OnPolyObjDeleted(PolyPartsObject polyObj) {
			DelteContent(polyObj);
		}

		/// <summary>
		/// ポリゴンオブジェクトの頂点変更
		/// </summary>
		private void OnPolyObjVertexChanged(PolyPartsObject polyObj) {
			PolyObjVertexChanged(polyObj);	
		}

		/// <summary>
		/// ポリゴンオブジェクトの色変更
		/// </summary>
		private void OnPolyObjColorChanged(PolyPartsObject polyObj) {
			PolyObjColorChanged(polyObj);
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