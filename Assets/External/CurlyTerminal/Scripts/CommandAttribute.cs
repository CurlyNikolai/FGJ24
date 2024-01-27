using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[System.AttributeUsage(System.AttributeTargets.Method)]
public class CommandAttribute : System.Attribute
{
    public static Dictionary<string, MethodInfo> commandMap = new Dictionary<string, MethodInfo>();

    public static void InitCommandMap()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var methods = assembly.GetTypes()
                              .SelectMany(t => t.GetMethods())
                              .Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
                              .ToArray();
        foreach (var m in methods)
            commandMap.Add(m.Name, m);
    }

}
