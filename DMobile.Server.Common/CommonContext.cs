using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Interface;
using Common.Interface;
using Session;
using Utilities;
using System.IO;

namespace Common
{
    public class CommonContext
    {
        private static CommonContext _currentContext;
        public static CommonContext CurrentContext
        {
            get { return _currentContext ?? (_currentContext = new CommonContext()); }
        }

        public MethodDispatcher MethodDispatcher { get; private set; }

        public SessionController SessionController { get; private set; }

        public IParseMethodRequest ParseMethodRequest { get; set; }

        public IParseMethodResponse ParseMethodResponse { get; set; }

        public IAttachResolver AttachResolver { get; set; }

        public ISecurityAuthorize SecurityAuthorize { get; set; }

        private IBusiness _business;
        public IBusiness Business
        {
            get { return _business; }
            set
            {
                _business = value;
                if (value != null)
                {
                    
                    SessionValidator validator = new SessionValidator
                                                     {
                                                         SessionEntries = value.GetSessionEntries()
                                                     };
                    SessionController.SessionValidator = validator;
                    
                    MethodRoute.Init(value.GetType());
                }
            }
        }

        public FastProduceJson JsonProvider { get; set; }

        private CommonContext()
        {
            MethodDispatcher = new MethodDispatcher();
            SessionController = new SessionController();
            JsonProvider = new FastProduceJson();
        }
    }
}
