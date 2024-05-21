/////////////////////////////////////////////////////////////////////////////////////
//  File:   Utils.cs                                                11 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using Ng911Lib.Utilities;
using System.Text.Json;

namespace PsapSimulator;

/// <summary>
/// Class for various utility functions
/// </summary>
public class Utils
{
    /// <summary>
    /// Creates a deep copy of an object by serializing it as a JSON object and then de-serializing it.
    /// Only public and protected members of the object will be copied.
    /// </summary>
    /// <param name="obj">Input object to make a copy of</param>
    /// <returns>Returns a deep copy of the input object</returns>
    public static object CopyObject(object obj)
    {
        object copy;
        string str = JsonHelper.SerializeToString(obj);
        Type type = obj.GetType();
        copy = JsonSerializer.Deserialize(str, obj.GetType())!;
        return copy;
    }
}
