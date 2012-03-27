using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimLib.Abstractions.Networking
{
	class SimpleTimer
	{
		private Thread thread;
		private int timeout;
		private volatile bool ended = false;

		public event EventHandler Finished;

		public SimpleTimer(int timeout)
		{
			thread = new Thread(Delay);
			this.timeout = timeout;
		}

		private void Delay()
		{
			while (!ended)
			{
				ended = true;
				try
				{
					Thread.Sleep(timeout);
				}
				catch (ThreadInterruptedException)
				{
				}
			}
			Thread asyncEvent = new Thread(OnFinished);
			asyncEvent.Start();
			Console.WriteLine("Ended!!");
		}

		public void Start()
		{
			if (thread.ThreadState == ThreadState.Unstarted)
			{
				thread.Start();
			}
		}

		public void Postpone()
		{
			ended = false;
		}


		private void OnFinished()
		{
			EventHandler hnd = Finished;
			if (hnd != null) hnd(this, EventArgs.Empty);
		}

		//              public void Restart()
		//              {
		//                      if (thread.ThreadState == ThreadState.WaitSleepJoin)
		//                      {
		//                              ended = true;
		//                              thread.Interrupt();
		//                              thread.Join();
		//                      }
		//                      if(!thread.IsAlive)
		//                      {
		//                              ended = false;
		//                              thread = new Thread(Delay);
		//                              thread.Start();
		//                      }
		//                      else Console.WriteLine("Fuck...");
		//              }

		//              public void Stop()
		//              {
		//                      ended = true;
		//                      if (thread.IsAlive)
		//                      {
		//                              thread.Interrupt();
		//                              thread.Join();
		//                      }
		//              }
	}
}














































//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;

//namespace SimLib.Abstractions.Networking
//{
//    class SimpleTimer
//    {
//        private Thread thread;
//        private int timeout;
//        private volatile bool abort = false;

//        private object mutex = new object();

//        public event EventHandler Finished;

//        public SimpleTimer(int timeout)
//        {
//            thread = new Thread(Delay);
//            this.timeout = timeout;
//        }

//        private void Delay()
//        {
//            try
//            {
//                Thread.Sleep(timeout);
//            }
//            catch (ThreadInterruptedException)
//            {
//                if (abort) return;
//            }
//            lock (mutex)
//            {
//                Thread asyncEvent = new Thread(OnFinished);
//                asyncEvent.Start();
//            }
			
//        }

//        public void Start()
//        {
//            lock (mutex)
//            {
//                if (thread.ThreadState == ThreadState.Unstarted)
//                {
//                    thread.Start();
//                    abort = false;
//                }
//                else Restart();
//            }
//        }

//        private void Restart()
//        {
//            Stop();
//            if (!thread.IsAlive)
//            {
//                thread = new Thread(Delay);
//                Start();
//            }
//            else Console.WriteLine(thread.ThreadState);
//        }

//        public void Stop()
//        {
//            abort = true;
//            lock (mutex)
//            {
//                if (thread.IsAlive)
//                {
//                    thread.Abort();
//                    thread.Join();
//                }
//            }
//        }

//        private void OnFinished()
//        {
//            EventHandler hnd = Finished;
//            if (hnd != null) hnd(this, EventArgs.Empty);
//        }
//    }
//}


////public void Start()
////{
////    if (thread.ThreadState == ThreadState.Unstarted)
////    {
////        thread.Start();
////        abort = false;
////    }
////    else if (thread.ThreadState == ThreadState.Stopped) Restart();
////    else
////        Console.WriteLine(thread.ThreadState);
////}

////public void Restart()
////{
////    if (!thread.IsAlive)
////    {
////        abort = false;
////        thread = new Thread(Delay);
////        thread.Start();
////    }
////}