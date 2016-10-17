using Seiro.Scripts.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts._Test1.UnitEditor.Component.StateMachine {

	/// <summary>
	/// ユニットエディタの状態
	/// </summary>
	public abstract class UnitEditorState : UnitEditorComponent, IState {

		//状態の持ち主
		protected UnitEditorStateMachine owner;

		//状態が有効か
		protected bool active = false;

		#region VirtualFunction

		/// <summary>
		/// 使用しない
		/// </summary>
		[Obsolete("Not using")]
		public override void Initialize(UnitEditor unitEditor) {
			base.Initialize(unitEditor);
		}

		/// <summary>
		/// 初期化
		/// </summary>
		public virtual void Initialize(UnitEditor unitEditor, UnitEditorStateMachine owner) {
			base.Initialize(unitEditor);
			this.owner = owner;
		}

		#endregion

		#region IState メンバー

		public virtual void Enter() {
			active = true;
		}

		public virtual void Exit() {
			active = false;
		}

		#endregion
	}
}