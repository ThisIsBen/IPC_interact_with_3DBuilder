using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IPCTest.Startup))]
namespace IPCTest
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
