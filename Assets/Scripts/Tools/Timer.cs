using System;

namespace Utilities {
	public abstract class Timer {
		protected float initialTime;
		protected float Time { get; set; }
		public bool IsRunning { get; protected set; }
        
		public float Progress => Time / initialTime;
        
		public Action OnTimerStart = delegate { };
		public Action OnTimer = delegate { };
		public Action OnTimerStop = delegate { };

		protected Timer(float value) {
			initialTime = value;
			IsRunning = false;
		}

		public void Start() {
			Time = initialTime;
			if (!IsRunning) {
				IsRunning = true;
				OnTimerStart.Invoke();
			}
		}

		public void Stop() {
			if (IsRunning) {
				IsRunning = false;
				OnTimerStop.Invoke();
			}
		}
        
		public void Resume() => IsRunning = true;
		public void Pause() => IsRunning = false;
		public float GetTime() => Time;
		public float GetInitialTime() => initialTime;
        
		public abstract void Tick(float deltaTime);
	}
    
	public class CountdownTimer : Timer {
		public CountdownTimer(float value) : base(value) { }

		public override void Tick(float deltaTime) {
			if (IsRunning && Time > 0) {
				Time -= deltaTime;
				OnTimer.Invoke();
			}
            
			if (IsRunning && Time <= 0) {
				Stop();
			}
		}
        
		public bool IsFinished => Time <= 0;
        
		public void Reset() => Time = initialTime;
        
		public void Reset(float newTime) {
			initialTime = newTime;
			Reset();
		}
		
		public void ResetNowTime(float newTime,bool isStart = false) {
			Time = newTime;
			if(!isStart) return;
			if (!IsRunning) {
				IsRunning = true;
				OnTimerStart.Invoke();
			}
		}
		
	}
    
	public class RecordTimer : Timer {
		public RecordTimer() : base(0) { }

		public override void Tick(float deltaTime) {
			if (IsRunning) {
				Time += deltaTime;
			}
		}
        
		public void Reset() => Time = 0;
        
	}
}
