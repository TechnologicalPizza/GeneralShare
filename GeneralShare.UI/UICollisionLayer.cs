﻿using MonoGame.Extended;

namespace GeneralShare.UI
{
    public class UICollisionLayer : UIElement
    {
        private RectangleF _boundaries;
        private RectangleF _bounds;

        public override RectangleF Boundaries => _boundaries;
        public RectangleF Bounds { get => _bounds; set => SetBounds(value); }

        public UICollisionLayer(UIManager manager) : base(manager)
        {
            MarkedDirty += UILayer_MarkedDirty;
        }

        private void SetBounds(RectangleF value)
        {
            MarkDirtyE(ref _bounds, value, DirtMarkType.Bounds);
        }

        private void UILayer_MarkedDirty(DirtMarkType type)
        {
            if (type.HasFlags(DirtMarkType.Transform, DirtMarkType.Bounds))
            {
                float x = _position.X + _bounds.X - _origin.X * _scale.X;
                float y = _position.Y + _bounds.Y - _origin.Y * _scale.Y;
                float w = Bounds.Width * _scale.X;
                float h = Bounds.Height * _scale.Y;
                _boundaries = new RectangleF(x, y, w, h);

                InvokeMarkedDirty(DirtMarkType.Boundaries);
            }
        }
    }
}
