using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client_desktop.Model
{
    public class DataContainer
    {
        private static DataContainer instance;

        public string Login { get; set; }
        public string Password { get; set; }

        public static DataContainer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataContainer();
                }
                return instance;
            }
        }
    }
}
