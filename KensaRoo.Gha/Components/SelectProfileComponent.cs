using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Grasshopper.Kernel;
using KensaRoo.Core.Models;
using KensaRoo.Core.Services;
using KensaRoo.Gha.Attributes;
using KensaRoo.Gha.Types;

namespace KensaRoo.Gha.Components;

public class SelectProfileComponent : GH_Component
{
    private static readonly ProfileLibraryService LibraryService = new();
    private static bool _isLibraryLoaded = false;
    
    // Public property to hold the name of the currently selected profile.
    // The Attributes class will read this property to know what text to draw.
    public string SelectedProfileName { get; private set; } = "Select a Profile...";
    
    // Private field to store the full data of the selected profile.
    private SteelProfile _selectedProfile;

    public SelectProfileComponent() : base (
        "Select Profile",
        "SelectProfile",
        "Select the steel profile from the library using a dropdown menu.",
        "KensaRoo",
        "1. Library"
        )
    {
        // Load the library when the component is first created.
        if (!_isLibraryLoaded)
        {
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var jsonPath = Path.Combine(assemblyLocation, "profiles.json");
            LibraryService.LoadFromFile(jsonPath);
            _isLibraryLoaded = true;
        }
    }
    
    // This is the crutial method that tells Grasshopper to use our custom attributes.
    public override void CreateAttributes()
    {
        m_attributes = new SelectProfileAttributes(this);
    }
    
    // This method builds and shows the context menu when called by the Attributes class.
    public void ShowContextMenu(Point screen_location)
    {
        var menu = new ContextMenuStrip();
        foreach (var profile in LibraryService.Profiles)
        {
            var menuItem = menu.Items.Add(profile.ProfileName);
            menuItem.Tag = profile; // Store the full profile object in the menu item
            menuItem.Click += MenuItem_Click; // Assign an event handler for when it's clicked.
        }
        menu.Show(screen_location);
    }

    // This is the event handler that runs when a user clicks an item in our menu.
    private void MenuItem_Click(object sender, EventArgs e)
    {
        if (sender is ToolStripMenuItem menuItem && menuItem.Tag is SteelProfile profile)
        {
            // Update our component's state with the selected profile.
            SelectedProfileName = profile.ProfileName;
            _selectedProfile = profile;
            
            // This is very important! It tells Grasshopper that our component's
            // state has changed, and it needs to re-calculate its solution.
            ExpireSolution(true);
        }
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
        // No longer need a name input, but a refresh button is still useful.
        pManager.AddBooleanParameter(
            "Refresh",
            "R",
            "Set to true to reload the profile library from disk",
            GH_ParamAccess.item, 
            false
            );
    }

    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
        // The output is our custom SteelProfile data type.
        pManager.AddParameter(
            new SteelProfileParam(),
            "Profile",
            "P",
            "The selected KensaRoo Steel Profile object.",
            GH_ParamAccess.item
            );
    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
        // Logic for the optional refresh button
        bool shouldRefresh = false;
        DA.GetData("Refresh", ref shouldRefresh);
        if (shouldRefresh)
        {
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var jsonPath = Path.Combine(assemblyLocation, "profiles.json");
            LibraryService.LoadFromFile(jsonPath);
            // If we refreshed, we should expire the solution to update any downstream components.
            this.ExpireSolution(true);
        }
        
        // If a profile has been selected, wrap it in our "Goo" and set the output.
        if (_selectedProfile != null)
        {
            DA.SetData("Profile", new SteelProfileGoo(_selectedProfile));
        }
    }
    
    // We will add persistence logic here later to save/load the selected profile.
    
    protected override Bitmap Icon => null;
    public override Guid ComponentGuid => new Guid("9A54FB17-7FD6-4640-B712-330C7955355B");

}