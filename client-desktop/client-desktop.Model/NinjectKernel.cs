using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using client_desktop.Model.UnitOfWorks;

namespace client_desktop
{
    public class NinjectKernel : NinjectModule
    {
        public override void Load()
        {
            //Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IWebUnitOfWork>().To<WebUnitOfWork>();
        }

    }
}
