using UnityEngine;
using System.Collections.Generic;

// 큐에 넣고 빼서 실행

public class CommandQueue
{
    private Queue<ICommand> commands = new Queue<ICommand>();

    public void EnQueue(ICommand command)
    {
        commands.Enqueue(command);
    }

    public void ExcuteAll()
    {
        while(commands.Count > 0)
        {
            commands.Dequeue().Execute();
        }
    }
}
