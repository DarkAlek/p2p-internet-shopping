using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_server.Models.WebApi
{
    public class UserApi
    {
        public string Id;
        public string UserName;
        public string Email;
        public string Password; // password hash or plain-text, depend on context
        public string PhoneNumber;
        public int Type;
    }
}