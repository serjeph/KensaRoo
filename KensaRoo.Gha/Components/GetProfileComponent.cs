using System;
using System.Drawing;
using System.Linq;
using Grasshopper.Kernel;
using KensaRoo.Core.Services;
using KensaRoo.Gha.Types;

namespace KensaRoo.Gha.Components;

public class GetProfileComponent : GH_Component
{
    // We access the same static library service that ListProfilesComponent uses.
    private static readonly ProfileLibraryService LibraryService = new ProfileLibraryService();
    
    public GetProfileComponent()
        : base(
            "Get Profile",
            "GetProfile",
            "Retrieves the full data for a specific steel profile by name.",
            "KensaRoo",
            "1. Library"
            )
    { }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
        pManager.AddTextParameter(
            "Profile Name", 
            "Name", 
            "Name of the profile to retrieve (e.g., 'CH-100x50x5-JIS'",
            GH_ParamAccess.item
            );
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
        // The output uses the custom parameter type we created earlier.
        pManager.AddParameter(
            new SteelProfileParam(),
            "Profile",
            "P",
            "The selected KensaRoo Steel Profile object",
            GH_ParamAccess.item
            );
    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
        string profileName = string.Empty;
        if (!DA.GetData("Profile Name", ref profileName)) return;
        if (string.IsNullOrEmpty(profileName)) return;
        
        // Find the profile in our library that matches the input name.
        var foundProfile = LibraryService.Profiles.FirstOrDefault(p => p.ProfileName.Equals(profileName, StringComparison.OrdinalIgnoreCase));
        if (foundProfile == null)
        {
            AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, $"Profile not found: {profileName}");
            return;
        }
        
        DA.SetData("Profile", new SteelProfileGoo(foundProfile));
    }
    
    protected override Bitmap Icon => null;
    
    public override Guid ComponentGuid => new Guid("F2CB6918-C2DB-4CD5-A9AE-84E542297B10");
}