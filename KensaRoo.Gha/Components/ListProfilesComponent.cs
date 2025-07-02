using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Grasshopper.Kernel;
using KensaRoo.Core.Services;

namespace KensaRoo.Gha.Components;

public class ListProfilesComponent : GH_Component
{
    // This static instance ensures the library is loaded only once.
    public static readonly ProfileLibraryService LibraryService = new ProfileLibraryService();
    private static bool _isLibraryLoaded = false;

    public ListProfilesComponent()
        : base(
            "List Profiles",
            "ListProfiles",
            "List all available steel profiles from the library.",
            "KensaRoo",
            "1. Library"
            )
    {
        
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
        pManager.AddBooleanParameter(
            "Refresh",
            "R",
            "Set to true to reload the profile library from disk.",
            GH_ParamAccess.item,
            true
            );
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
        pManager.AddTextParameter(
            "Profile Names",
            "Names",
            "A list of all available steel profiles from the library.",
            GH_ParamAccess.list
            );
    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
        bool shouldRefresh = false;
        DA.GetData("Refresh", ref shouldRefresh);

        if (!_isLibraryLoaded || shouldRefresh)
        {
            var assembyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var jsonPath =  Path.Combine(assembyLocation, "profiles.json");
            // This is the line that reads the file.
            LibraryService.LoadFromFile(jsonPath);
            _isLibraryLoaded = true;
            
            if(shouldRefresh) ExpireSolution(true);
        }
        
        var names = LibraryService.Profiles.Select(p => p.ProfileName).ToList();
        DA.SetDataList("Profile Names", names);
    }
    
    protected override System.Drawing.Bitmap Icon => null;

    public override Guid ComponentGuid => new Guid("F046D19C-A2A9-4AC0-95B2-263F77DC177A");
}