using UnityEngine;
using System;
using Seiro.Scripts.FSM;

namespace Scripts._Test1.UnitEditor.Component {
	
	/// <summary>
	/// ユニットエディタの状態コンポーネント
	/// </summary>
	public abstract class UnitEditorStateComponent : UnitEditorComponent, IState {
		
		#region IState メンバー

		public virtual void Enter() { }

		public virtual void Exit() { }

		#endregion
	}
}
