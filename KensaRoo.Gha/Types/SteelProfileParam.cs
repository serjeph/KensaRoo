using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace KensaRoo.Gha.Types;

/// <summary>
/// This is the "Param" class.
/// It defines the input/output socket for our custom data type
/// on a Grasshopper component.
/// </summary>
public class SteelProfileParam : GH_PersistentParam<SteelProfileGoo>
{
    public SteelProfileParam() : base(
        "Steel Profile", 
        "Profile", 
        "A KensaRoo Steel Profile data object", 
        "KensaRoo", 
        "1. Library"
        )
    {
    }

    public override Guid ComponentGuid => new Guid("8b742535-074f-4e59-a61c-ae01565e4929");

    protected override GH_GetterResult Prompt_Singular(ref SteelProfileGoo value) => GH_GetterResult.cancel;
    protected override GH_GetterResult Prompt_Plural(ref List<SteelProfileGoo> values) => GH_GetterResult.cancel;
}