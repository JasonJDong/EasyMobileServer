using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DMobile.Server.Login;
using DMobile.Server.Main;

namespace FrameworkTest
{
    public partial class Form1 : Form
    {
        private static ManualResetEvent manualEvent = new ManualResetEvent(false);
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenHost();
            button1.Enabled = false;
        }

        private  void OpenHost()
        {
            manualEvent.Reset();
            Thread t1 = new Thread(OpenMainBusiness);
            t1.Start();
        }

        private  void OpenMainBusiness()
        {
            using (WebServiceHost webHost = new WebServiceHost(typeof(CommunicationCenter)))
            {
                webHost.Open();
                Thread t2 = new Thread(OpenLogin);
                t2.Start();
                while (manualEvent.WaitOne())
                {
                    break;
                }
            }
        }

        private  void OpenLogin()
        {
            using (WebServiceHost login = new WebServiceHost(typeof(LoginService)))
            {
                login.Open();
                while (manualEvent.WaitOne())
                {
                    break;
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            manualEvent.Set();
            base.OnClosing(e);
        }
    }
}
