using UnityEngine;
using System.Collections.Generic;
using Scripts._Test.PolyPartsEditor.Common;
using Scripts._Test1.UnitEditor.Common.Parts;

namespace Scripts._Test1.UnitEditor.Component.Utility.Database {
	
	/// <summary>
	/// ユニットエディタのデータベース
	/// </summary>
	public class UnitEditorDatabase : UnitEditorComponent {

		[Header("Prefab")]
		public PartsObject prefab;

		[Header("Callback")]
		public PartsEvent onPartsDown;
		public PartsEvent onPartsUp;
		public PartsEvent onPartsClick;
		public PartsEvent onInstantiateParts;
		public PartsEvent onDeleteParts;

		//内部パラメータ
		private List<PartsObject> partsObjs;

		#region VirtualFunction

		public override void Initialize(UnitEditor unitEditor) {
			base.Initialize(unitEditor);

			partsObjs = new List<PartsObject>();
		}

		#endregion

		#region Function

		/// <summary>
		/// ポリゴンオブジェクトの初期化
		/// </summary>
		private void InitializePolyObj(PartsObject partsObj) {
			//親の設定
			partsObj.transform.SetParent(transform);
			//追加
			partsObjs.Add(partsObj);

			//コールバックの設定
			partsObj.onDown.AddListener(OnPartsDown);
			partsObj.onUp.AddListener(OnPartsUp);
			partsObj.onClick.AddListener(OnPartsClick);
		}

		/// <summary>
		/// ポリゴンオブジェクトの生成
		/// </summary>
		public PartsObject InstantiatePolyObj(List<Vector2> vertices, Color color) {
			//生成と頂点の設定
			PartsObject partsObj = Instantiate<PartsObject>(prefab);
			partsObj.SetVertices(vertices, color);

			//初期化
			InitializePolyObj(partsObj);

			//コールバック
			onInstantiateParts.Invoke(partsObj);
			
			return partsObj;
		}

		/// <summary>
		/// ポリゴンオブジェクトの削除
		/// </summary>
		public void DeletePolyPartsObject(PartsObject partsObj) {
			if (partsObjs.Remove(partsObj)) {
				//コールバック
				onDeleteParts.Invoke(partsObj);
				//削除
				Destroy(partsObj.gameObject);
			}
		}

		/// <summary>
		/// 全てのポリゴンを有効化する
		/// </summary>
		public void EnablePolygons() {
			for (int i = 0; i < partsObjs.Count; ++i) {
				partsObjs[i].Enable();
			}
		}

		/// <summary>
		/// 全てのポリゴンを無効化する
		/// </summary>
		public void DisablePolygons() {
			for (int i = 0; i < partsObjs.Count; ++i) {
				partsObjs[i].Disable();
			}
		}

		#endregion

		#region PartsCallback

		/// <summary>
		/// パーツの押下
		/// </summary>
		private void OnPartsDown(PartsObject parts) {
			onPartsDown.Invoke(parts);
		}

		/// <summary>
		/// パーツの押上
		/// </summary>
		private void OnPartsUp(PartsObject parts) {
			onPartsUp.Invoke(parts);
		}

		/// <summary>
		/// パーツのクリック
		/// </summary>
		private void OnPartsClick(PartsObject parts) {
			onPartsClick.Invoke(parts);
		}

		#endregion
	}
}