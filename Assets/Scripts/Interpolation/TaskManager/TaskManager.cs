using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Task : IFlushable
{
	public bool Running { get { return task.Running; } }

	public bool Paused { get { return task.Paused; } }

	public bool flushAtFinish = true;

	public delegate void FinishedHandler (bool manual);

	public event FinishedHandler Finished;

	///-------------------------

	TaskManager.TaskState task;

	public Task() {} // Constructor must non-abstract and public.

	public Task (IEnumerator c, bool autoStart = true)
	{
		task = TaskManager.CreateTask (c);
		task.Finished += TaskFinished;
		if (autoStart)
			Start ();
	}

	public static Task Get (IEnumerator c, bool autoStart = true)
	{
		Task t = ObjPool<Task>.Get ();
		if (t.task == null)
			t.task = TaskManager.CreateTask (c);
		else
			t.task.coroutine = c;
		t.task.Finished += t.TaskFinished;
		if (autoStart)
			t.Start ();
		return t;
	}

	///----------------------

	public void Start ()
	{
		task.Start ();
	}

	public void Stop ()
	{
		task.Stop ();
	}

	public void Pause ()
	{
		task.Pause ();
	}

	public void Unpause ()
	{
		task.Unpause ();
	}

	public void Reset ()
	{
		task.Restart ();
	}

	void TaskFinished (bool manual)
	{
		FinishedHandler handler = Finished;
		if (handler != null)
			handler (manual);
		if (flushAtFinish)
			Flush ();
	}

	///--------------------

	bool m_flushed;

	public bool GetFlushed ()
	{
		return m_flushed;
	}

	public void SetFlushed (bool flushed)
	{
		m_flushed = flushed;
	}

	public void Flush ()
	{
		task.Stop ();
		task.Finished -= TaskFinished;
		ObjPool<Task>.Release (this);
	}

	///----------------------------

	static Dictionary<string, Task> idStack = new Dictionary<string, Task>();

	public static Task Get (IEnumerator c, string id, bool overrideIfExist = true, bool autoStart = true)
	{
		if(idStack.ContainsKey(id))
		{
			Task t = idStack[id];
			if(overrideIfExist)
			{
				if(c == t.task.coroutine)
					t.task.Restart();
				else
				{
					t.Stop();
					t.task.coroutine = c;
				}
			}
			if(autoStart)
				t.Start();
			return t;
		} else {
			Task t = Get(c, autoStart);
			idStack.Add(id, t);
			return t;
		}
	}


}

class TaskManager : MonoBehaviour
{
	public class TaskState
	{

		public delegate void FinishedHandler (bool manual);

		public event FinishedHandler Finished;

		public IEnumerator coroutine;
		public bool Running {get; private set;}
		public bool Paused {get; private set;}
		bool stopped;
		bool restart;

		public TaskState (IEnumerator c)
		{
			coroutine = c;
		}

		public void Pause ()
		{
			Paused = true;
		}

		public void Unpause ()
		{
			Paused = false;
		}

		public void Restart ()
		{
			restart = true;
		}

		public void Start ()
		{
			Running = true;
			stopped = false;
			singleton.StartCoroutine (CallWrapper ());
		}

		public void Stop ()
		{
			stopped = true;
			Running = false;
		}

		IEnumerator CallWrapper ()
		{
			yield return null;
			IEnumerator e = coroutine;
			while (Running) {
				if (Paused)
					yield return null;
				else if(restart){
					restart = false;
					if(e != null)
						e.Reset();
				}
				else {
					if(e != coroutine)
						e = coroutine;
					if (e != null && e.MoveNext ()) {
						yield return e.Current;
					} else {
						Running = false;
					}
				}
			}
			
			FinishedHandler handler = Finished;
			if (handler != null)
				handler (stopped);
		}
	}

	static TaskManager singleton;

	public static TaskState CreateTask (IEnumerator coroutine)
	{
		if (singleton == null) {
			GameObject go = new GameObject ("TaskManager");
			singleton = go.AddComponent<TaskManager> ();
		}
		return new TaskState (coroutine);
	}
}
