using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeandroExhumed.SnakeGame.Snake
{
    public interface ICollector
    {
        void CollectEnginePower (float speedAddition);
        void CollectBatteringRam ();
    }
}