using System.Collections;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class PlayerStateObjectTutorial : PlayerStateObject
	{
		private enum TutorialState
		{
			ReadyNext,
			WaitingNext,
			PendingAction,
			Complete,
			Backup
		}

		private enum TutorialStep
		{
			Start,
			TurnLeft,
			TurnRight,
			Jump,
			Done
		}

		private TutorialState currState;

		private TutorialStep currStep;

		private AbstractInputBehaviour inputBehaviour;

		private void Awake()
		{
			player = GetComponentInParent<PlayerController>();
			inputBehaviour = player.inputBehaviour;
		}

		public override void EnterState()
		{
			DispatchEnterStateEvent();
			UnityEngine.Debug.Log("Game EnterState -> Setting tutorial to 'ReadyNext' (" + currState + "," + currStep + ")");
			ChangeStep(TutorialStep.Start);
			ChangeState(TutorialState.ReadyNext);
		}

		private void Update()
		{
			if (currState == TutorialState.ReadyNext && !SledRacerGameManager.Instance.Paused)
			{
				ChangeState(TutorialState.WaitingNext);
				switch (currStep)
				{
				case TutorialStep.Start:
					UnityEngine.Debug.Log("Trigger Panel 1 -> Setting  (" + currState + "," + currStep + ")");
					StartCoroutine(TutorialLeftTurn());
					break;
				case TutorialStep.TurnLeft:
					UnityEngine.Debug.Log("Trigger Panel 2 -> Setting  (" + currState + "," + currStep + ")");
					StartCoroutine(TutorialRightTurn());
					break;
				case TutorialStep.TurnRight:
					UnityEngine.Debug.Log("Trigger Panel 3 -> Setting  (" + currState + "," + currStep + ")");
					StartCoroutine(TutorialJump());
					break;
				case TutorialStep.Jump:
					StartCoroutine(TutorialComplete());
					break;
				case TutorialStep.Done:
					ExitState();
					break;
				}
			}
			else if (SledRacerGameManager.Instance.Paused && currState != 0)
			{
				UnityEngine.Debug.Log("Game is Paused -> Setting tutorial to 'Backup' (" + currState + "," + currStep + ")");
				ChangeState(TutorialState.ReadyNext);
				switch (currStep)
				{
				case TutorialStep.Start:
					break;
				case TutorialStep.TurnLeft:
					ChangeStep(TutorialStep.Start);
					break;
				case TutorialStep.TurnRight:
					ChangeStep(TutorialStep.TurnLeft);
					break;
				case TutorialStep.Jump:
					ChangeStep(TutorialStep.TurnRight);
					break;
				case TutorialStep.Done:
					ChangeStep(TutorialStep.Jump);
					break;
				}
			}
		}

		private void ChangeState(TutorialState _state)
		{
			if (currState != _state)
			{
				currState = _state;
			}
		}

		private void ChangeStep(TutorialStep _step)
		{
			if (currStep != _step)
			{
				currStep = _step;
			}
		}

		public bool left()
		{
			Vector3 zero = Vector3.zero;
			bool flag = false;
			if (currStep == TutorialStep.TurnLeft)
			{
				flag = inputBehaviour.left();
				if (flag && currState == TutorialState.PendingAction)
				{
					ChangeState(TutorialState.ReadyNext);
				}
			}
			return flag;
		}

		public bool right()
		{
			Vector3 zero = Vector3.zero;
			bool flag = false;
			if (currStep == TutorialStep.TurnRight)
			{
				flag = inputBehaviour.right();
				if (flag && currState == TutorialState.PendingAction)
				{
					ChangeState(TutorialState.ReadyNext);
				}
			}
			return flag;
		}

		public bool jump()
		{
			Vector3 zero = Vector3.zero;
			bool flag = false;
			if (currStep == TutorialStep.Jump)
			{
				flag = inputBehaviour.jump();
				if (flag && currState == TutorialState.PendingAction)
				{
					ChangeState(TutorialState.ReadyNext);
				}
			}
			return flag;
		}

		private IEnumerator TutorialLeftTurn()
		{
			yield return new WaitForSeconds(1f);
			if (!SledRacerGameManager.Instance.Paused)
			{
				ChangeStep(TutorialStep.TurnLeft);
				ChangeState(TutorialState.PendingAction);
				UnityEngine.Debug.Log("Game Show Panel 1 -> Setting  (" + currState + "," + currStep + ")");
				DispatchTutorialEvent(new GameEvent(GameEvent.Type.TutorialProgress, 0));
			}
			else
			{
				ChangeStep(TutorialStep.Start);
				ChangeState(TutorialState.ReadyNext);
				UnityEngine.Debug.Log("Game Tried to show Panel 1 -> Paused => Setting  (" + currState + "," + currStep + ")");
			}
		}

		private IEnumerator TutorialRightTurn()
		{
			yield return new WaitForSeconds(0.5f);
			if (!SledRacerGameManager.Instance.Paused)
			{
				ChangeStep(TutorialStep.TurnRight);
				ChangeState(TutorialState.PendingAction);
				UnityEngine.Debug.Log("Game Show Panel 2 -> Setting  (" + currState + "," + currStep + ")");
				DispatchTutorialEvent(new GameEvent(GameEvent.Type.TutorialProgress, 1));
			}
			else
			{
				ChangeStep(TutorialStep.Start);
				ChangeState(TutorialState.ReadyNext);
				UnityEngine.Debug.Log("Game Tried to show Panel 2 -> Paused => Setting  (" + currState + "," + currStep + ")");
			}
		}

		private IEnumerator TutorialJump()
		{
			yield return new WaitForSeconds(0.5f);
			if (!SledRacerGameManager.Instance.Paused)
			{
				ChangeStep(TutorialStep.Jump);
				ChangeState(TutorialState.PendingAction);
				UnityEngine.Debug.Log("Game Show Panel 3 -> Setting  (" + currState + "," + currStep + ")");
				DispatchTutorialEvent(new GameEvent(GameEvent.Type.TutorialProgress, 2));
			}
			else
			{
				ChangeStep(TutorialStep.TurnRight);
				ChangeState(TutorialState.ReadyNext);
				UnityEngine.Debug.Log("Game Tried to show Panel 3 -> Paused => Setting  (" + currState + "," + currStep + ")");
			}
		}

		private IEnumerator TutorialComplete()
		{
			yield return new WaitForSeconds(0f);
			ChangeStep(TutorialStep.Done);
			ChangeState(TutorialState.ReadyNext);
			DispatchTutorialEvent(new GameEvent(GameEvent.Type.TutorialProgress));
		}

		private void DispatchTutorialEvent(GameEvent gameEvent)
		{
			UnityEngine.Debug.Log("Sending Event " + gameEvent.type);
			player.DispatchGameEvent(gameEvent);
		}

		protected override void ExitState()
		{
			base.enabled = false;
			DispatchExitStateEvent();
		}

		public override void AbortState()
		{
		}
	}
}
