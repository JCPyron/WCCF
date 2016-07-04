using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCCFNew
{
    class GmailClass
    {
        private string myUser;
        private string myPassword;
        public GmailClass(string aUser, string aPassword)
        {
            myUser = aUser;
            myPassword = aPassword;
        }
        public string User
        {
            get { return myUser; }
            set { myUser = value; }
        }
        public string Password
        {
            get { return myPassword;}
            set { myPassword = value; }
        }
    }
}
