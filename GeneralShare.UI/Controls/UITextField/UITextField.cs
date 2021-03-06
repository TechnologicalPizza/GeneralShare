﻿using GeneralShare.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace GeneralShare.UI
{
    public partial class UITextField : UITextElement
    {
        public delegate bool ValidateKeyDelegate(InputSource source, int character);
        public static readonly ValidateKeyDelegate DefaultValidateInput;

        public const char DefaultObscureChar = '*';

        private char _obscureChar;
        private bool _isObscured;
        private bool _isMultiLined;
        private int _charLimit;

        private TextSegment _placeholderSegment;
        private Color _placeholderColor;
        private float _placeholderColorLerp;

        // TODO: implement key repeating
        private float _keyRepeatTime;
        private float _repeatKeyOccuring;
        private ValidateKeyDelegate _validateInput;

        static UITextField()
        {
            DefaultValidateInput = (s, k) => true;
        }

        public UITextField(UIManager manager, BitmapFont font) : base(manager, font)
        {
            Caret = new CaretData();
            ValidateInput = DefaultValidateInput;

            _placeholderSegment = new TextSegment(font);
            UsePlaceholderColorFormatting = true;
            PlaceholderColor = Color.Gray * 0.8f;
            PlaceholderSelectColor = Color.LightGoldenrodYellow * 0.9f;
            PlaceholderColorTransitionSpeed = 0.125f;

            _obscureChar = DefaultObscureChar;
            _charLimit = 1024;
            
            CharBlacklist = new ObservableHashSet<char>();
            CharBlacklist.OnAdd += (s, value) => Remove(value);

            IsKeyboardEventTrigger = true;
            IsMouseEventTrigger = true;
            BuildQuadTree = true;
            IsSelectable = true;
            SelectionColor = Color.OrangeRed;
            SelectionOutlineThickness = 2f;

            OnKeyRepeat += UITextArea_OnKeyRepeat;
            OnKeyPress += UITextArea_OnKeyPress;
            OnTextInput += UITextArea_OnTextInput;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (IsHovered || IsSelected)
            {
                LerpPlaceholderColor(time, 1);
            }
            else if (_placeholderSegment.Color != _placeholderColor)
            {
                LerpPlaceholderColor(time, 0);
            }
        }

        public override void Draw(GameTime time, SpriteBatch batch)
        {
            base.Draw(time, batch);

            if (Length == 0)
                batch.DrawString(_placeholderSegment, StringRect.Position);

            if (IsSelected)
                batch.DrawRectangle(Boundaries, SelectionColor, SelectionOutlineThickness);
        }

        protected override RectangleF OnBoundaryUpdate(RectangleF newRect)
        {
            RectangleF baseRect = base.OnBoundaryUpdate(newRect);

            baseRect.X -= SelectionOutlineThickness;
            baseRect.Y -= SelectionOutlineThickness;
            baseRect.Width += SelectionOutlineThickness * 2;
            baseRect.Height += SelectionOutlineThickness * 2;

            return baseRect;
        }

        private void LerpPlaceholderColor(GameTime time, float dst)
        {
            float transitionSpeed = 1f;
            if(PlaceholderColorTransitionSpeed > 0)
                transitionSpeed = time.Delta * 1f / PlaceholderColorTransitionSpeed * 2;

            _placeholderColorLerp = Mathf.Lerp(_placeholderColorLerp, dst, transitionSpeed);
            _placeholderSegment.Color = Color.Lerp(_placeholderColor, PlaceholderSelectColor, _placeholderColorLerp);
            _placeholderSegment.ApplyColors();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _placeholderSegment?.Dispose();
                _placeholderSegment = null;
            }

            base.Dispose(disposing);
        }

        public class CaretData
        {
            public int StartIndex;
            public int SelectionCount;
        }
    }
}