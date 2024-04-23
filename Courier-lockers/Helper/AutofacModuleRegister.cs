using Autofac;
using Autofac.Extras.DynamicProxy;
using System.Reflection;

namespace WMSService.Helper
{
    public class AutofacModuleRegister : Autofac.Module
    {
        //重写Autofac管道Load方法，在这里注册注入
        protected override void Load(ContainerBuilder builder)
        {
            var assemblysServices = Assembly.Load(ServiceCore.GetAssemblyName());
            builder.RegisterAssemblyTypes(assemblysServices)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors();
        }
    }
}
