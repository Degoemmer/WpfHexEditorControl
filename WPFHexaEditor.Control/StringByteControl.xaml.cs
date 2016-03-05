﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFHexaEditor.Control.Core;

namespace WPFHexaEditor.Control
{
    /// <summary>
    /// Interaction logic for StringByteControl.xaml
    /// </summary>
    public partial class StringByteControl : UserControl
    {
        private byte? _byte = null;
        private bool _isSelected = false;
        private bool _isByteModified = false;
        private bool _readOnlyMode;

        public event EventHandler Click;
        public event EventHandler MouseSelection;
        public event EventHandler StringByteModified;
        public event EventHandler MoveNext;

        public StringByteControl()
        {
            InitializeComponent();
        }

        public long BytePositionInFile { get; set; } = -1;

        public byte? Byte
        {
            get
            {
                return this._byte;
            }
            set
            {
                this._byte = value;

                UpdateLabelFromByte();

                if (IsByteModified)
                    if (StringByteModified != null)
                        StringByteModified(this, new EventArgs());
            }
        }

        /// <summary>
        /// Update control label from byte property
        /// </summary>
        private void UpdateLabelFromByte()
        {
            if (_byte != null)
            {
                StringByteLabel.Content = Converters.ByteToChar(_byte.Value);
            }
            else
            {
                StringByteLabel.Content = "";
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                _isSelected = value;

                UpdateBackGround();
            }
        }

        /// <summary>
        /// Update Background
        /// </summary>
        private void UpdateBackGround()
        {
            if (_isSelected)
            {
                StringByteLabel.Foreground = Brushes.White;
                this.Background = Brushes.Blue;
            }
            else if (_isByteModified)
            {
                this.Background = Brushes.LightGray;
                StringByteLabel.Foreground = Brushes.Black;
            }
            else
            {
                this.Background = Brushes.Transparent;
                StringByteLabel.Foreground = Brushes.Black;
            }
        }

        public bool IsByteModified
        {
            get
            {
                return this._isByteModified;
            }
            set
            {
                this._isByteModified = value;

                UpdateBackGround();
            }
        }

        public bool ReadOnlyMode
        {
            get
            {
                return _readOnlyMode;
            }
            set
            {
                _readOnlyMode = value;
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            //if (!ReadOnlyMode)
            //    if (KeyValidator.IsHexKey(e.Key))
            //    {
            //        string key;
            //        if (KeyValidator.IsNumericKey(e.Key))
            //            key = KeyValidator.GetDigitFromKey(e.Key).ToString();
            //        else
            //            key = e.Key.ToString().ToLower();

            //        StringByteLabel.Content = key;

            //        //Move focus event
            //        if (MoveNext != null)
            //            MoveNext(this, new EventArgs());                    
            //    }
            bool isok = false;

            if (Keyboard.Modifiers != ModifierKeys.Shift && e.Key != Key.RightShift && e.Key != Key.LeftShift)
            {
                StringByteLabel.Content = Converters.ByteToChar((byte)KeyInterop.VirtualKeyFromKey(e.Key)).ToString().ToLower();//e.Key.ToString();
                isok = true;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Shift && e.Key != Key.RightShift && e.Key != Key.LeftShift)
            {
                isok = true;
                StringByteLabel.Content = Converters.ByteToChar((byte)KeyInterop.VirtualKeyFromKey(e.Key));//e.Key.ToString();    
            }

            //Move focus event
            if (isok)
                if (MoveNext != null)
                {
                    MoveNext(this, new EventArgs());

                    IsByteModified = true;
                    Byte = Converters.CharToByte(StringByteLabel.Content.ToString()[0]);
                }
        }
    


        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_byte != null)
                if (!IsByteModified && !_isSelected)
                    this.Background = Brushes.SlateGray;

            if (e.LeftButton == MouseButtonState.Pressed)
                if (MouseSelection != null)
                    MouseSelection(this, e);
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_byte != null)
                if (!IsByteModified && !_isSelected)
                    this.Background = Brushes.Transparent;
        }

        private void StringByteLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.Focus();

                if (Click != null)
                    Click(this, e);
            }
        }        
    }
}