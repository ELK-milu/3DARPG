using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern.CommandSystem
{
    public interface ICommand
    {
        void Execute();
    }
}
