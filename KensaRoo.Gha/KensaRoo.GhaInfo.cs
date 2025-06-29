using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace KensaRoo.Gha
{
    public class KensaRoo_GhaInfo : GH_AssemblyInfo
    {
        public override string Name => "KensaRoo.Gha Info";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("10561d83-a22f-46c5-a0c5-eeed234b078a");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";

        //Return a string representing the version.  This returns the same version as the assembly.
        public override string AssemblyVersion => GetType().Assembly.GetName().Version.ToString();
    }
}