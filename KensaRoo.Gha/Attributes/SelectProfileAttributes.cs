using System.Drawing;
using System.Windows.Forms;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using KensaRoo.Gha.Components;


namespace KensaRoo.Gha.Attributes;

/// <summary>
/// This class defines the custom look and feel for our component.
/// It's responsible for drawing a dropdown-style button and handling mouse clicks.
/// </summary>
public class SelectProfileAttributes : GH_ComponentAttributes
{
    private RectangleF _buttonBounds;
    
    public SelectProfileAttributes(SelectProfileComponent owner) : base(owner) { }

    // The Layout method is called by Grasshopper to arrange the component's parts.
    protected override void Layout()
    {
        // This rus the standard Grasshopper layout logic first.
        base.Layout();
        
        // We then shrink the component's main body slightly to make room for our button.
        Rectangle rec = GH_Convert.ToRectangle(Bounds);
        rec.Height += 22; // Add 22 pixels of height for the button area.

        Rectangle buttonRec = rec;
        buttonRec.Y = buttonRec.Bottom - 22; // button area at the bottom.
        buttonRec.Height = 22;
        buttonRec.Inflate(-2, -2); // Shrink slightly.

        Bounds = rec; // update component's total bounds.
        _buttonBounds = buttonRec;
    }
    
    // The Render method is where we draw our custom UI elements.
    protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
    {
        // This runs the standard Grasshopper component rendering first.
        base.Render(canvas, graphics, channel);

        if (channel == GH_CanvasChannel.Objects)
        {
            var palette = Owner.Locked ? GH_Palette.Locked : GH_Palette.White;
            string displayText = ((SelectProfileComponent)Owner).SelectedProfileName;
            var capsule = GH_Capsule.CreateTextCapsule(_buttonBounds, _buttonBounds, palette, displayText);
            capsule.Render(graphics, Selected, Owner.Locked, false);
            capsule.Dispose();
            
            // 1. Define the size and position of the arrow area.
            var arrowRect = _buttonBounds;
            arrowRect.X = arrowRect.Right - 12;
            arrowRect.Width = 8;
            arrowRect.Inflate(0, -8);
            
            // 2. Define the three points of our triangle.
            var p1 = new PointF(arrowRect.X, arrowRect.Y);
            var p2 = new PointF(arrowRect.Right, arrowRect.Y);
            var p3 = new PointF(arrowRect.X + arrowRect.Width / 2f, arrowRect.Bottom);
            PointF[] arrowPoints = { p1, p2, p3 };
            
            // 3. Draw the filled triangle
#pragma warning disable CA1416
            graphics.FillPolygon(Brushes.Black, arrowPoints);
#pragma warning restore CA1416
        }
    }

    public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
    {
        if (e.Button == MouseButtons.Left && _buttonBounds.Contains(e.CanvasLocation))
        {
            ((SelectProfileComponent)Owner).ShowContextMenu(e.ControlLocation);
            return GH_ObjectResponse.Handled;
        }
        return base.RespondToMouseDown(sender, e);
    }
}