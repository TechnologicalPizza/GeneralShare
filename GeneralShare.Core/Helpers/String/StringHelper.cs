﻿using System;
using System.Diagnostics;
using System.Text;

namespace GeneralShare
{
    public static partial class StringHelper
    {
        public static int LineCount(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            int index = -1;
            int count = 1;
            while (-1 != (index = value.IndexOf('\n', index + 1)))
                count++;

            return count;
        }

        public static bool Equals(this string value, StringBuilder builder)
        {
            if (builder == null && value == null)
                return true;

            if (builder != null && value == null)
                return false;

            if (builder == null && value != null)
                return false;

            int length = builder.Length;
            if (length == value.Length)
            {
                for (int i = 0; i < length; i++)
                {
                    if (builder[i] != value[i])
                        return false;
                }
                return true;
            }
            return false;
        }

        public static string ToSpaced(this string camelCasedString)
        {
            if (string.IsNullOrWhiteSpace(camelCasedString))
                return string.Empty;

            var newText = new StringBuilder(camelCasedString, (int)(camelCasedString.Length * 1.1f));
            for (int i = 1; i < newText.Length; i++)
            {
                if (char.IsUpper(newText[i]) && newText[i - 1] != ' ')
                    newText.Insert(i, ' ');
            }
            return newText.ToString();
        }

        public static byte[] HexToByteArray(string hex)
        {
            byte[] output = new byte[hex.Length >> 1];
            HexToByteArray(hex, output);
            return output;
        }

        [DebuggerHidden]
        private static ArgumentException GetKeyException()
        {
            return new ArgumentException("The binary key cannot have an odd number of digits");
        }

        [DebuggerHidden]
        private static ArgumentException GetTooShortException()
        {
            return new ArgumentException("The output array was too short for given binary key.");
        }

        public static int HexToByteArray(string hex, byte[] output)
        {
            if (hex.Length % 2 == 1)
                throw GetKeyException();

            int length = hex.Length / 2;
            if (output.Length < length)
                throw GetTooShortException();

            for (int i = 0; i < length; ++i)
            {
                output[i] = (byte)(
                    GetHexValue(hex[i * 2]) * 16 +
                    GetHexValue(hex[i * 2 + 1]));
            }

            return length;
        }

        public static int HexToByteSpan(Span<char> hex, Span<byte> output)
        {
            if (hex.Length % 2 == 1)
                throw GetKeyException();

            int length = hex.Length / 2;
            if (output.Length < length)
                GetTooShortException();

            for (int i = 0; i < length; ++i)
            {
                output[i] = (byte)(
                    GetHexValue(hex[i * 2]) * 16 +
                    GetHexValue(hex[i * 2 + 1]));
            }

            return length;
        }

        public static int GetHexValue(char hex)
        {
            //Two combined, but a bit slower:
            return hex - (hex < 58 ? 48 : (hex < 97 ? 55 : 87));
        }

        public static int GetHexValueUppercaseOnly(char hex)
        {
            //For uppercase A-F letters:
            return hex - (hex < 58 ? 48 : 55);
        }

        public static int GetHexValueLowercaseOnly(char hex)
        {
            //For lowercase a-f letters:
            return hex - (hex < 58 ? 48 : 87);
        }
    }
}
