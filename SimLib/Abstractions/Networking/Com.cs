using System;
using System.Collections.Generic;
using System.Threading;
using SimLib.Fields;
using SimLib.Messages;
using SimLib.Nodes;
using SimLib.Messages.Types;

namespace SimLib.Abstractions.Networking
{
    public class Com<T>
    {
        List<IMessage> onGoingMessages;
        Dictionary<int, int> era; // era contains the transfered messages per period of time
        NodeContainer nodes;
        private int previousEraNo;
        private bool firstOfAnEra;

        public Com(NodeContainer nc)
        {
            this.onGoingMessages = new List<IMessage>();
            era = new Dictionary<int, int>();
            era.Add(0, 0); //0,0
            this.nodes = nc;
            previousEraNo = 0;
            firstOfAnEra = true;
        }

        /// <summary>
        /// Starts a transmission period
        /// </summary>
        public void startOfAnEra(String eraName)
        {
            //Console.WriteLine("\n[DBG]\tAPP\tStarting phase \"" + eraName.ToUpper() + "\"\t@" +previousEraNo);
            while (onGoingMessages.Count > 0 || firstOfAnEra)
            {
                //Transmit them
                foreach (IMessage message in onGoingMessages)
                {
                    deliver(message);
                }

                //Clear transmission queue
                onGoingMessages.Clear();

                foreach (INode inode in nodes.List())
                {
                    inode.processReceived();
                }

                //Add any messages for transmission
                foreach (INode iNode in nodes.List())
                {
                    onGoingMessages.AddRange(iNode.transmit());
                    iNode.endTransmissions();
                }
                //Now all messages are ready to be sent
                //Log the number of messages
                era.Add(era.Count, onGoingMessages.Count);
                //Console.WriteLine("[DBG]\tAPP\t@" + era.Count + "\tMSG #: " + era[era.Count-1]);
                firstOfAnEra = false;
            }
            //Console.WriteLine("[DBG]\tAPP\tEnded phase \"" + eraName.ToUpper() + "\"\tNumber of transmission periods: " + (era.Count - previousEraNo));
            previousEraNo = era.Count;
            firstOfAnEra = true;
        }

        /// <summary>
        /// Delivers a message to the respective targets
        /// </summary>
        /// <param name="message"></param>
        private void deliver(IMessage message)
        {
            if (typeof(T).FullName != "SimLib.Messages.IMessage")
            {
                return;
            }
            if (message.Envelop.Target == MessageTargets.ALL_IN_RANGE)
            {
                List<int> targets = nodes.Neighbors(message.Envelop.Source);
                foreach (var mTarget in targets)
                {
                    INode source = nodes.Get(message.Envelop.Source);
                    INode target = nodes.Get(mTarget);
                    if (SimMath.Distance.WithinRange(source, target))
                    {
                        target.receive(message);
                    }
                }
            }
            else
            {
                INode source = nodes.Get(message.Envelop.Source);
                INode target = nodes.Get(message.Envelop.Target);
                if (SimMath.Distance.WithinRange(source, target))
                {
                    target.receive(message);
                }
            }
        }
    }

    //private Thread dispatcher;
    //private Queue<T> queue;
    //private int waitTime;
    //private Object locker;
    //private Timer timer;
    //private ManualResetEvent myevent;
    //private Field field;
    //public event EventHandler EmptyQueueEvent;

    //public Com(Field field)
    //{
    //    queue = new Queue<T>();
    //    this.field = field;
    //    locker = new Object();
    //    waitTime = 20;
    //    timer = new Timer(FireEmpty, null, Timeout.Infinite, Timeout.Infinite);
    //    myevent = new ManualResetEvent(false);
    //    dispatcher = new Thread(Serve);
    //    dispatcher.IsBackground = true;
    //}

    //void emptyQueueTimer_Finished(object sender, EventArgs e)
    //{
    //    FireEmpty(null);
    //}

    //public void Start()
    //{
    //    dispatcher.Start();
    //}

    //private void Serve()
    //{
    //    while (true)
    //    {
    //        int count = 0;
    //        lock (locker)
    //            count = queue.Count;
    //        if (queue.Count == 0)
    //        {
    //            myevent.WaitOne(); // Wait for Add() to signal us
    //        }
    //        lock (locker)
    //            count = queue.Count;
    //        while (count != 0)
    //        {
    //            lock (locker)
    //            {
    //                deliver(queue.Dequeue());
    //                count = queue.Count;
    //                timer.Change(waitTime, 0);
    //            }
    //        }
    //        // Reset to non-signaled state so the next loop iteration
    //        // goes into the wait state waiting on Add()
    //        myevent.Reset();
    //    }
    //}

    //private void deliver(T item)
    //{
    //    new Messenger<T>(field).deliver(item);
    //}

    //public void Add(T item)
    //{
    //    lock (locker)
    //    {
    //        queue.Enqueue(item);
    //    }
    //    myevent.Set();
    //}

    //private void FireEmpty(object o)
    //{
    //    EventHandler handler = EmptyQueueEvent;
    //    if (handler != null)
    //    {
    //        handler(this, EventArgs.Empty);
    //    }
    //}
}
