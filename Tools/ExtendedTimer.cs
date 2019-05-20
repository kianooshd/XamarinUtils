using System;
using Xamarin.Forms;

namespace Tools
{
    public class ExtendedTimer
    {
        public bool IsRunning { get; private set; }
        private bool Stopping { get; set; }
        private bool Paused { get; set; }
        private bool DelayedStop { get; set; }

        private TimeSpan Interval { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan Elapsed { get; private set; }
        public TimeSpan Remained { get; private set; }


        private Action Tick { get; set; }
        private Action Stopped { get; set; }
        private Action Started { get; set; }
        private Action Resumed { get; set; }

        public bool ManualStop { get; set; }

        public ExtendedTimer(TimeSpan interval, TimeSpan duration, Action tick = null, Action stopped = null, bool manualStop = false)
        {
            Interval = interval;
            Tick = tick ?? (() => { });
            Stopped = stopped ?? (() => { });
            Duration = duration;
            Paused = false;
            Stopping = false;
            DelayedStop = false;
            Elapsed = new TimeSpan(0);
            ManualStop = manualStop;
        }

        public void Resume()
        {
            if (Paused)
            {
                Paused = false;

                Resumed?.Invoke();
                RunTimer();
            }
        }

        public void Start()
        {
            Elapsed = new TimeSpan(0);

            if (!IsRunning)
            {
                IsRunning = true;
                Started?.Invoke();

                if (Stopping) Stop(true);

                else
                {
                    RunTimer();
                }
            }
        }

        public void Stop(bool timerEnds = false, bool instantStop = true)
        {
            if (Paused)
            {
                Stopping = true;
                return;
            }

            Stopping = false;
            IsRunning = false;
            DelayedStop = !instantStop;
            if (timerEnds && instantStop) Stopped?.Invoke();
        }

        public void Pause()
        {
            Paused = true;
        }

        private void RunTimer()
        {
            Device.StartTimer(Interval, () =>
            {

                if (Paused) return false;

                if (!IsRunning)
                {
                    if (DelayedStop) Stopped?.Invoke();
                    return false;
                }

                Elapsed = new TimeSpan(Elapsed.Ticks + Interval.Ticks);

                if (!ManualStop) Remained = new TimeSpan(Duration.Ticks - Elapsed.Ticks);

                Tick?.Invoke();

                if (ManualStop) return true;

                if (Elapsed.Ticks >= Duration.Ticks)
                {
                    Stop(true);

                    return false;
                }

                return true;

            });

        }
    }
}