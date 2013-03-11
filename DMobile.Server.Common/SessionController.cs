using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Interface;

namespace Common
{
    public class SessionController
    {
        public ISessionValidator SessionValidator { get; set; }
    }
}
