using UnityEngine;
using UnityEngine.UI;
using Scripts._Test.PolyPartsEditor.Database;
using Scripts._Test.PolyPartsEditor.Common;

namespace Scripts._Test.PolyPartsEditor.UI.Main {
	
	/// <summary>
	/// ポリゴンオブジェクトの確認
	/// </summary>
	class PolyObjView : MonoBehaviour {

		public PolyPartsDatabase database;

		public MeshImage view;

		private void Start() {
			database.onPolyObjDrawed.AddListener(OnPolyObjDrawed);
		}

		private void OnPolyObjDrawed(PolyPartsObject polyObj) {
			view.SetEasyMesh(polyObj.GetPolygonEasyMesh());
		}
	}
}