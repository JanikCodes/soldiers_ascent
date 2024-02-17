using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component that can receive tasks and queue them. 
/// </summary>
public class TaskQueue : MonoBehaviour
{
    public Queue<Task> Tasks { get; private set; }

    private void Start()
    {
        // Initialize the queue
        Tasks = new Queue<Task>();

        Task task1 = new Task();
        task1.TaskType = TaskType.Wait;
        QueueTask(task1);

        Task task2 = new Task();
        task2.TaskType = TaskType.Roam;
        QueueTask(task2);
    }

    public Task GetCurrentTask()
    {
        return Tasks.Peek();
    }

    public void QueueTask(Task task)
    {
        Tasks.Enqueue(task);
    }

    public void CompleteCurrentTask()
    {
        if (Tasks.Count > 0)
        {
            Tasks.Dequeue();
        }
    }
}
