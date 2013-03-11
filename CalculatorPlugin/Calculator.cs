using System;
using DMobile.Server.Common.Context;
using DMobile.Server.Extension.Plugin.Common;

namespace CalculatorPlugin
{
    public class Calculator : PluginBase
    {
        public Calculator()
        {
            PluginInfo = new PluginSchema
                {
                    Name = "Calculator",
                    Author = "Jason",
                    Description = "Nothing",
                    Version = new Version(1, 1, 0, 0)
                };
        }

        public override bool ReceiveDataContext(DataContext dataContext)
        {
            return true;
        }

        public override bool Close()
        {
            return true;
        }

        public override bool Open()
        {
            return true;
        }

        public override bool Stop()
        {
            return true;
        }

        public override bool Restart()
        {
            return true;
        }
    }
}