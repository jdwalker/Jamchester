using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class CreditsGen
{
    public static void Init()
    {

    }


    public static string Next()
    {
        return JobTitleGen.GetJobTitle() + ": " + NameGen.GetName();
    }
}