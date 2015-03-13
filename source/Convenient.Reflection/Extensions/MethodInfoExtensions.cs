using System.Reflection;
using System.Runtime.CompilerServices;

namespace Convenient.Reflection.Extensions
{
    public static class MethodInfoExtensions
    {
        public static bool IsExtensionMethod(this MethodInfo method)
        {
            return method.IsDefined(typeof (ExtensionAttribute), false);
        }


    }
}