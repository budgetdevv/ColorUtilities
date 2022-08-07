using System.Reflection;
using System.Runtime.CompilerServices;

namespace Color.Encoding
{
    public static class Optimizer
    {
        [ModuleInitializer]
        internal static void Optimize()
        {
            foreach (var Type in Assembly.GetExecutingAssembly().GetTypes())
            {
                RuntimeHelpers.RunClassConstructor(Type.TypeHandle);
            }
        }
    }
}