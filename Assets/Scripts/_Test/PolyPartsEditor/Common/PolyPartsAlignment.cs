using UnityEngine;
using System;
using System.Collections.Generic;

namespace Scripts._Test.PolyPartsEditor.Common {

	/// <summary>
	/// ポリゴンパーツの整列
	/// </summary>
	public class PolyPartsAlignment : MonoBehaviour {

		[Header("AlignmentParm")]
		public float alignStart = 1f;	//整列先頭位置
		public float alignEnd = 0f;		//整列終了位置

		#region Function

		/// <summary>
		/// 整列
		/// </summary>
		public void Alignment(List<Transform> trances) {
			//範囲と間隔を計算
			int count = trances.Count;
			float alignRange = alignEnd - alignStart;
			float alignDelta = count > 1 ? alignRange / (count - 1) : 0f;
			float deltaSum = 0f;

			//整列開始
			for(int i = 0; i < count; ++i) {
				Transform t = trances[i];
				//位置変更
				Vector3 p = t.localPosition;
				p.z = deltaSum;
				t.localPosition = p;
				deltaSum += alignDelta;
			}
		}

		#endregion

	}
}