using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static ConversionAgrigator;

public class ConversionAgrigator : MonoBehaviour
{
    // Start is called before the first frame update

    public delegate A Convertion<A>(string message);

    [ConversionMethod]
    [RuntimeInitializeOnLoadMethod]
    public static void GatherMethods()
    {
        Assembly[] assemblies = new Assembly[1];

        assemblies[0] = Assembly.GetExecutingAssembly();


        foreach (var ass in assemblies)
        {
            // object instance = Activator.CreateInstance(t);
            var methods = ass
                .GetTypes()
                .SelectMany(x => x.GetMethods())
                .Where(y => y.GetCustomAttributes().OfType<ConversionMethod>().Any())
                .ToDictionary(z => z.ReflectedType.Name);
            if (methods.Count > 0)
            {
                foreach (var a in methods)
                {
                    var dd = Activator.CreateInstance(a.Value.ReflectedType);
                    Converter.ConversionMethods.Add(a.Key, (x) => { return a.Value.Invoke(dd, new object[] { x }); });

                }
            }

        }
        


    }

}
public class AddToEvent : Attribute
{
    public string Event;
    public AddToEvent(string dd)
    {
        Event = dd;
    }
}
