﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GeneralShare.UI
{
    public abstract class UIElement : UITransform
    {
        public delegate void MouseHoverDelegate(UIElement sender, MouseState mouseState);
        public delegate void GenericMouseDelegate(UIElement sender, MouseState mouseState, MouseButton buttons);
        public delegate void GenericKeyboardDelegate(UIElement sender, Keys key);
        public delegate void RepeatedKeyboardDelegate(UIElement sender, Keys key, float timeDown);
        public delegate void ScrollDelegate(UIElement sender, float amount);
        public delegate void TextInputDelegate(UIElement sender, TextInputEvent data);
        
        public event GenericMouseDelegate OnMouseDown;
        public event GenericMouseDelegate OnMousePress;
        public event GenericMouseDelegate OnMouseRelease;
        public event MouseHoverDelegate OnMouseEnter;
        public event MouseHoverDelegate OnMouseHover;
        public event MouseHoverDelegate OnMouseLeave;
        public event ScrollDelegate OnScroll;
        public event RepeatedKeyboardDelegate OnKeyRepeat;
        public event GenericKeyboardDelegate OnKeyDown;
        public event GenericKeyboardDelegate OnKeyPress;
        public event GenericKeyboardDelegate OnKeyRelease;
        public event TextInputDelegate OnTextInput;

        private bool _wasSelected;
        private bool _isSelectable;

        public abstract RectangleF Boundaries { get; }
        public string Name { get; set; }
        public bool IsMouseEventTrigger { get; set; }
        public bool IsKeyboardEventTrigger { get; set; }

        public bool IsSelected
        {
            get => Manager.SelectedElement == this;
            set
            {
                if (value && IsSelectable)
                    Manager.SelectedElement = this;
                else if (!value && IsSelected)
                    Manager.SelectedElement = null;
            }
        }

        /// <summary>
        /// Returns <see langword="true"/> if the cursor is within
        /// this <see cref="UIElement"/>'s boundaries, otherwise <see langword="false"/>.
        /// </summary>
        public bool IsHovered { get; internal set; }

        /// <summary>
        /// Dictates if this <see cref="UIElement"/> intercepts mouse events.
        /// </summary>
        public bool IsIntercepting { get; set; }

        /// <summary>
        /// Dictates if this <see cref="UIElement"/> can be selected
        /// as <see cref="UIManager.SelectedElement"/>.
        /// </summary>
        public bool IsSelectable
        {
            get => _isSelectable;
            set
            {
                if (_isSelectable != value)
                {
                    _isSelectable = value;
                    if (!_isSelectable)
                    {
                        _wasSelected = IsSelected;
                        IsSelected = false;
                    }
                    else if(_wasSelected && Manager.SelectedElement == null)
                        IsSelected = true;
                }
            }
        }

        public UIElement(UIManager manager) : base(manager)
        {
            IsMouseEventTrigger = false;
            IsKeyboardEventTrigger = false;
            IsIntercepting = true;
        }

        internal void TriggerOnTextInput(TextInputEvent data)
        {
            OnTextInput?.Invoke(this, data);
        }

        internal void TriggerOnMouseDown(MouseState state, MouseButton buttons)
        {
            OnMouseDown?.Invoke(this, state, buttons);
        }

        internal void TriggerOnMousePress(MouseState state, MouseButton buttons)
        {
            OnMousePress?.Invoke(this, state, buttons);
        }

        internal void TriggerOnMouseRelease(MouseState state, MouseButton buttons)
        {
            OnMouseRelease?.Invoke(this, state, buttons);
        }

        internal void TriggerOnMouseEnter(MouseState state)
        {
            OnMouseEnter?.Invoke(this, state);
        }

        internal void TriggerOnMouseHover(MouseState state)
        {
            OnMouseHover?.Invoke(this, state);
        }

        internal void TriggerOnMouseLeave(MouseState state)
        {
            OnMouseLeave?.Invoke(this, state);
        }

        internal void TriggerOnScroll(float amount)
        {
            OnScroll?.Invoke(this, amount);
        }

        internal void TriggerOnKeyRepeat(Keys key, float timeDown)
        {
            OnKeyRepeat?.Invoke(this, key, timeDown);
        }

        internal void TriggerOnKeyDown(Keys key)
        {
            OnKeyDown?.Invoke(this, key);
        }

        internal void TriggerOnKeyPress(Keys key)
        {
            OnKeyPress?.Invoke(this, key);
        }

        internal void TriggerOnKeyRelease(Keys key)
        {
            OnKeyRelease?.Invoke(this, key);
        }
    }
}