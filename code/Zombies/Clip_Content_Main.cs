using Godot;
using System;

public class Clip_Content_Main : Control
{
    private RID _canvasItem;
    public override void _Ready()
    {
        _canvasItem = GetCanvasItem();
        Update();
    }
    public override void _Draw()
    {
        if (this.RectClipContent)
        {
            Rect2 clipRect = new Rect2(-1024, -1024, 20480, 1024 + RectSize.y);
            VisualServer.CanvasItemSetClip(_canvasItem, true);
            VisualServer.CanvasItemSetCustomRect(_canvasItem, true, clipRect);
        }
            base._Draw();
    }
    public override void _Notification(int what)
    {
        if (what == NotificationResized)
        {
            Update();
        }
    }
}
