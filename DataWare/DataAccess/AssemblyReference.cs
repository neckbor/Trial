﻿using System.Reflection;

namespace DataAccess;

public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
