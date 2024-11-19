/////////////////////////////////////////////////////////////////////////////////////
//  File:   QueuedActionThread.cs                                   31 Oct 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Logging;
using System.Collections.Concurrent;
using System.Threading;

namespace SrsSimulator;

public class QueuedActionThread
{
    private const int DEFAULT_WAIT_INTERVAL_MS = 100;
    private const int MINIMUM_WAIT_INTERVAL_MS = 0;

    private bool m_IsStarted = false;
    private bool m_IsShutdown = false;
    private ConcurrentQueue<Action> m_WorkQueue = new ConcurrentQueue<Action>();
    private SemaphoreSlim m_Semaphore = new SemaphoreSlim(0, int.MaxValue);
    private int m_WaitIntervalMsec = DEFAULT_WAIT_INTERVAL_MS;
    private Thread m_Thread;

    public QueuedActionThread(int WaitIntervalMsec = DEFAULT_WAIT_INTERVAL_MS)
    {
        if (WaitIntervalMsec < MINIMUM_WAIT_INTERVAL_MS)
            m_WaitIntervalMsec = MINIMUM_WAIT_INTERVAL_MS;
        else
            m_WaitIntervalMsec = WaitIntervalMsec;

        m_Thread = new Thread(ThreadLoop);
        m_Thread.IsBackground = true;
        m_Thread.Priority = ThreadPriority.Highest;
    }

    public void Start()
    {
        if (m_IsStarted == true || m_IsShutdown == true)
            return;

        m_IsStarted = true;
        m_Thread.Start();
    }

    public void Shutdown()
    {
        if (m_IsStarted == false || m_IsShutdown == true)
            return;

        m_IsShutdown = true;
        m_Semaphore.Release();
        m_Thread.Join();
    }

    public void EnqueueWork(Action action)
    {
        m_WorkQueue.Enqueue(action);
        m_Semaphore.Release();
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void DoTimedEvents()
    {
    }

    private void ThreadLoop()
    {
        while (m_IsShutdown == false)
        {
            m_Semaphore.Wait(m_WaitIntervalMsec);

            DoTimedEvents();

            while (m_IsShutdown == false && m_WorkQueue.TryDequeue(out Action? action) == true)
            {
                if (action != null)
                {
                    try
                    {
                        action();
                    }
                    catch (Exception actionException)
                    {
                        SipLogger.LogError(actionException, "");
                    }
                }
            }
        } // end while
    }
}
