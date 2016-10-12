using UnityEngine;
using System.Collections.Generic;
using Scripts._Test.PolyPartsEditor.Common;

namespace Scripts._Test.PolyPartsEditor.Database {

	/// <summary>
	/// ポリゴンパーツのデータベース
	/// </summary>
	public class PolyPartsDatabase : MonoBehaviour {

		[Header("Prefab")]
		public PolyPartsObject prefab;

		[Header("Callback")]
		public PolyPartsObjectEvent onPolyObjInstantiated;
		public PolyPartsObjectEvent onPolyObjDeleted;
		public PolyPartsObjectEvent onPolyObjClicked;
		public PolyPartsObjectEvent onPolyObjVertexChanged;
		public PolyPartsObjectEvent onPolyObjColorChanged;

		//内部パラメータ
		private List<PolyPartsObject> polyObjs;

		#region UnityEvent

		private void Awake() {
			polyObjs = new List<PolyPartsObject>();
		}

		#endregion

		#region Function

		/// <summary>
		/// ポリゴンオブジェクトの生成
		/// </summary>
		public PolyPartsObject InstantiatePolyObj(List<Vector2> vertices) {
			PolyPartsObject polyObj = Instantiate<PolyPartsObject>(prefab);

			//transformなどの設定
			polyObj.SetVertices(vertices);

			//初期化
			InitializePolyObj(polyObj);

			return polyObj;
		}

		/// <summary>
		/// ポリゴンオブジェクトの複製を作成する
		/// </summary>
		public PolyPartsObject InstantiateClone(PolyPartsObject polyObj, Vector2 point) {
			PolyPartsObject clone = polyObj.InstantiateClone(point);

			//初期化
			InitializePolyObj(clone);

			return clone;
		}

		/// <summary>
		/// ポリゴンオブジェクトの初期化
		/// </summary>
		/// <param name="polyObj"></param>
		private void InitializePolyObj(PolyPartsObject polyObj) {

			//親の設定
			polyObj.transform.SetParent(transform);

			//追加
			polyObjs.Add(polyObj);

			//コールバック設定
			polyObj.onClick.AddListener(OnPolyObjClicked);
			polyObj.onVertexChanged.AddListener(OnPolyObjVertexChanged);
			polyObj.onColorChanged.AddListener(OnPolyObjColorChanged);

			//コールバック
			onPolyObjInstantiated.Invoke(polyObj);
		}

		/// <summary>
		/// ポリゴンオブジェクトの削除
		/// </summary>
		public void DeletePolyPartsObject(PolyPartsObject polyObj) {
			if(polyObjs.Remove(polyObj)) {
				//コールバック
				onPolyObjDeleted.Invoke(polyObj);
				//削除
				Destroy(polyObj.gameObject);
			}
		}

		/// <summary>
		/// 全てのポリゴンを有効化する
		/// </summary>
		public void EnablePolygons() {
			for(int i = 0; i < polyObjs.Count; ++i) {
				polyObjs[i].Enable();
			}
		}

		/// <summary>
		/// 指定したポリゴン以外を有効化する
		/// </summary>
		public void EnablePolygons(PolyPartsObject ignore) {
			EnablePolygons();
			ignore.Disable();
		}

		/// <summary>
		/// 全てのポリゴンを無効化する
		/// </summary>
		public void DisablePolygons() {
			for(int i = 0; i < polyObjs.Count; ++i) {
				polyObjs[i].Disable();
			}
		}

		/// <summary>
		/// 指定したポリゴン以外を無効化する
		/// </summary>
		public void DisablePolygons(PolyPartsObject ignore) {
			DisablePolygons();
			ignore.Enable();
		}

		#endregion

		#region Callback

		/// <summary>
		/// ポリゴンオブジェクトのクリック
		/// </summary>
		private void OnPolyObjClicked(PolyPartsObject polyObj) {
			onPolyObjClicked.Invoke(polyObj);
		}

		/// <summary>
		/// ポリゴンオブジェクトの頂点変更
		/// </summary>
		private void OnPolyObjVertexChanged(PolyPartsObject polyObj) {
			onPolyObjVertexChanged.Invoke(polyObj);
		}

		/// <summary>
		/// ポリゴンオブジェクトの色変更
		/// </summary>
		private void OnPolyObjColorChanged(PolyPartsObject polyObj) {
			onPolyObjColorChanged.Invoke(polyObj);
		}

		#endregion
	}
}