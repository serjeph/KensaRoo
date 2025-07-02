using Grasshopper.Kernel.Types;
using KensaRoo.Core.Models;

namespace KensaRoo.Gha.Types;

/// <summary>
/// This is the "Goo" wrapper class.
/// It tells Grasshopper how to handle our custom SteelProfile object.
/// </summary>
public class SteelProfileGoo : GH_Goo<SteelProfile>
{
    public SteelProfileGoo()
    {

    }

    public SteelProfileGoo(SteelProfile nativeData) : base(nativeData)
    {

    }
    
    public override bool IsValid => Value != null && !string.IsNullOrEmpty(Value.ProfileName);
    
    public override string TypeName => "Steel Profile";
    
    public override string TypeDescription => "A KensaRoo Steel Profile data object";
    
    public override IGH_Goo Duplicate() => new SteelProfileGoo(Value);

    public override string ToString() => IsValid ? $"Steel Profile [{Value.ProfileName}]" : "Null Steel Profile";
}