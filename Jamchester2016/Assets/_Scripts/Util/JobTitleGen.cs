using System;
using System.Collections.Generic;
using System.Linq;

using Random = UnityEngine.Random;

public static class JobTitleGen
{
    private static string[] Prefixes;
    private static string[] Suffixes;

    public static string GetJobTitle()
    {
        return string.Format("{0} {1}",
            Prefixes[Random.Range(0, Prefixes.Length)],
            Suffixes[Random.Range(0, Suffixes.Length)]);
    }

    public static string GetJobTitlePlural()
    {
        return GetJobTitle() + "s";
    }



    static JobTitleGen()
    {
        Prefixes = new []
        {
            "Lead", "Junior", "Associate", "Production", "Key", "Art", "Lighting", "Catering", "Best",
            "Lead", "Junior", "Associate", "Production", "Unit", "Stage",
        };

        Suffixes = new[]
        {
            "Producer", "Tester", "Grip", "Writer", "Technician", "Makeup", "Wardrobe", "Carpenter", "Coordinator", "Caterer",
            "Usher", "Host", "Manager",
        };
    }
}
