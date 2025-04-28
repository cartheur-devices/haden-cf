using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;
using System.Drawing;
using System.Data;

namespace Haden.Utilities.CF
{
    public class Util
    {
        private Util()
        {
        }

        #region Filename manipulation

        /// <summary>
        /// Strips the path from a filename
        /// 
        /// i.e. 'D:\util\zip.exe' becomes 'zip.exe'
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string StripDirectory(string path)
        {
            int index = path.LastIndexOf('\\');
            if (index != -1)
            {
                return path.Substring(index + 1);
            }
            else
            {
                return path;
            }
        }

        /// <summary>
        /// Strips the filename from a path
        /// 
        /// i.e. 'D:\util\zip.exe' becomes 'D:\util'
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string StripFilename(string path)
        {
            int index = path.LastIndexOf('\\');
            if (index != -1 && index != path.Length - 1)
            {
                return path.Substring(0, index);
            }
            else
            {
                return path;
            }
        }

        public static string GetExtension(string path)
        {
            int lastForwardSlashIdx = path.LastIndexOf('\\');
            int lastDotIdx = path.LastIndexOf('.');
            if (lastForwardSlashIdx > lastDotIdx)
            {
                return "";
            }
            else
            {
                return path.Substring(lastDotIdx + 1);
            }
        }

        #endregion

        #region String functions

        /// <summary>
        /// Pads a number with zeroes
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string PadZero(int value, int length)
        {
            return value.ToString().PadLeft(length, '0');
        }

        /// <summary>
        /// Creates a string containing a list of strings
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string CreateList(List<string> items)
        {
            return CreateList(items, "");
        }

        /// <summary>
        /// Creates a string containing a list of strings
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string CreateList(List<string> items, string defaultValue)
        {
            if (items == null || items.Count == 0)
            {
                return defaultValue;
            }
            else
            {
                string result = "";
                int count = 0;
                foreach (string item in items)
                {
                    if (count > 0)
                    {
                        if (count == items.Count - 1)
                        {
                            result += " en ";
                        }
                        else
                        {
                            result += ", ";
                        }
                    }
                    result += item;
                    count++;
                }
                return result;
            }
        }

        /// <summary>
        /// Creates a string containing a list of strings
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string CreateList(List<object> items, string comma, string and)
        {
            StringBuilder result = new StringBuilder();
            int count = 0;
            foreach (object obj in items)
            {
                if (count > 0)
                {
                    if (count == items.Count - 1)
                    {
                        result.Append(and);
                    }
                    else
                    {
                        result.Append(comma);
                    }
                }
                result.Append(obj.ToString());
                count++;
            }
            return result.ToString();
        }

        /// <summary>
        /// Returns true if the whitespace trimmed, lowercase
        /// versions of both strings are equal
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool KindOfEquals(string s1, string s2)
        {
            return s1.Trim().ToLower() == s2.Trim().ToLower();
        }

        public static string Hex(byte[] hex)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hex.Length; i++)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" ");
                }
                builder.Append(Hex(hex[i]));
            }
            return builder.ToString();
        }

        public static string Hex(string s)
        {
            return Hex(System.Text.Encoding.ASCII.GetBytes(s));
        }

        public static string Hex(byte b)
        {
            int hi = (int)(b / 16);
            int lo = b % 16;
            return hexChars[hi] + hexChars[lo];
        }
        static string[] hexChars = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

        #endregion

        #region Date/Time functions
        /// <summary>
        /// Returns a timestamp in ISO format (YYYYMMDDHHMMSSMMMM)
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ISOTimeStamp(DateTime time)
        {
            return time.Year + PadZero(time.Month, 2) + PadZero(time.Day, 2) + PadZero(time.Hour, 2) + PadZero(time.Minute, 2) + PadZero(time.Second, 2) + PadZero(time.Millisecond, 4);
        }

        /// <summary>
        /// Returns the total number of milliseconds until the specified time is reached.
        /// 
        /// If the specified time is in the past, the resulting number will be negative
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static double MilliSecondsTo(DateTime time)
        {
            TimeSpan span = time.Subtract(DateTime.Now);
            return span.TotalMilliseconds;
        }

        /// <summary>
        /// Retrieves the number of milliseconds that elapsed 
        /// since the computer started
        /// </summary>
        /// <returns></returns>
        public static long MilliSeconds()
        {
            if (hiresTimerFrequency == 0)
            {
                QueryPerformanceFrequency(out hiresTimerFrequency);
            }

            long result;
            QueryPerformanceCounter(out result);
            result *= 1000;
            result /= hiresTimerFrequency;
            return result;
        }
        private static long hiresTimerFrequency = 0;

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(
            out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(
            out long lpFrequency);
        #endregion

        #region Random
        /// <summary>
        /// Contains a random object for general use
        /// </summary>
        /// <value></value>
        public static Random Random
        {
            get
            {
                return _random;
            }
        }
        private static Random _random = new Random();
        #endregion

        #region Math

        public static int Mod(int n, int modulo)
        {
            if (n < 0)
            {
                int a = Math.Abs(n / modulo);
                n += (a + 1) * modulo;
            }
            return n % modulo;
        }


        public static double Mod(double n, double modulo)
        {
            if (n < 0)
            {
                int a = (int)Math.Abs(n / modulo);
                n += (a + 1) * modulo;
            }
            return n % modulo;
        }

        public static double Distance(Point p1, Point p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }


        public static float Clamp(float n, float min, float max)
        {
            return Math.Max(Math.Min(n, max), min);
        }

        #endregion

        #region Color

        /// <summary>
        /// Converts a (h,s,v) color to a RGB color
        /// </summary>
        /// <param name="hue">The hue of the color [0-360]</param>
        /// <param name="saturation">Saturation [0.0-1.0]</param>
        /// <param name="value">Value [0.0-1.0]</param>
        /// <returns></returns>
        public static Color RGBFromHSV(double hue, double saturation, double value)
        {
            hue = Mod(hue, 360.0);
            int quadrant = (int)(hue / 60.0);
            double f = (hue / 60.0) - (double)quadrant;
            double p = value * (1.0 - saturation);
            double q = value * (1.0 - f * saturation);
            double r = value * (1.0 - (1.0 - f) * saturation);
            switch (quadrant)
            {
                case 0:
                    return Color.FromArgb((int)(value * 255.0), (int)(r * 255.0), (int)(p * 255.0));
                case 1:
                    return Color.FromArgb((int)(q * 255.0), (int)(value * 255.0), (int)(p * 255.0));
                case 2:
                    return Color.FromArgb((int)(p * 255.0), (int)(value * 255.0), (int)(r * 255.0));
                case 3:
                    return Color.FromArgb((int)(p * 255.0), (int)(q * 255.0), (int)(value * 255.0));
                case 4:
                    return Color.FromArgb((int)(r * 255.0), (int)(p * 255.0), (int)(value * 255.0));
                case 5:
                    return Color.FromArgb((int)(value * 255.0), (int)(p * 255.0), (int)(q * 255.0));
                default:
                    throw new Exception("This should not happen");
            }
        }

        #endregion

        #region Graphics

        //public static void DrawThick(Graphics g, string s, Brush outerBrush, Brush innerBrush, Font f, RectangleF clipping)
        //{
        //    clipping.Offset(1, 0);
        //    g.DrawString(s, f, outerBrush, clipping);
        //    clipping.Offset(-1, -1);
        //    g.DrawString(s, f, outerBrush, clipping);
        //    clipping.Offset(-1, 1);
        //    g.DrawString(s, f, outerBrush, clipping);
        //    clipping.Offset(1, 1);
        //    g.DrawString(s, f, outerBrush, clipping);
        //    clipping.Offset(0, -1);
        //    g.DrawString(s, f, innerBrush, clipping);
        //}

        public static void DrawThick(Graphics g, string s, Brush outerBrush, Brush innerBrush, Font f, int x, int y)
        {
            RectangleF clipping = new RectangleF(x, y, g.ClipBounds.Width, g.ClipBounds.Height);
            //DrawThick(g, s, outerBrush, innerBrush, f, clipping);
        }


        public static float PointsTomm(float points)
        {
            return points * 25.41f / 72.0f;
        }


        #endregion

        #region Data

        /// <summary>
        /// This function populates the string with values from the datarow
        /// 
        /// 'Adres: {address}' -> 'Adres: Noorderstraat 5'
        /// </summary>
        /// <param name="str"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static string Populate(string str, DataRow row)
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                str = str.Replace("{" + column.ColumnName + "}", row[column].ToString());
            }
            return str;
        }

        #endregion

        #region Debugging

        public static void Assert(bool condition, string message)
        {
            Assert(condition, message, "A condition was not satisfied.");
        }

        public static void Assert(bool condition, string message, string title)
        {
            if (!condition)
            {
                throw new AssertionFailedException(message, title);
            }
        }

        #endregion

        #region Conversion

        public static int GetInt32(byte[] buffer, int start)
        {
            int result = 0;
            result += buffer[start];
            result += 256 * buffer[start + 1];
            result += 65536 * buffer[start + 2];
            result += 16777216 * buffer[start + 3];
            return result;
        }

        public static ushort GetUInt16(byte[] buffer, int start)
        {
            ushort result = 0;
            result += buffer[start];
            result += (ushort)(256 * buffer[start + 1]);
            return result;
        }

        public static uint GetUInt32(byte[] buffer, int start)
        {
            uint result = 0;
            result += buffer[start];
            result += (uint)(256 * buffer[start + 1]);
            result += (uint)(65536 * buffer[start + 2]);
            result += (uint)(16777216 * buffer[start + 3]);
            return result;
        }

        public static short GetInt16(byte[] buffer, int start)
        {
            short result = 0;
            result += buffer[start];
            result += (short)(256 * buffer[start + 1]);
            return result;
        }

        public static void SetInt32(byte[] buffer, int start, int value)
        {
            for (int i = 0; i < 4; i++)
            {
                buffer[start + i] = (byte)(value & 0xff);
                value /= 256;
            }
        }

        public static void SetUInt32(byte[] buffer, int start, uint value)
        {
            for (int i = 0; i < 4; i++)
            {
                buffer[start + i] = (byte)(value & 0xff);
                value /= 256;
            }
        }

        public static void SetInt16(byte[] buffer, int start, Int16 value)
        {
            for (int i = 0; i < 2; i++)
            {
                buffer[start + i] = (byte)(value & 0xff);
                value /= 256;
            }
        }

        #endregion


    }

    public class AssertionFailedException : ApplicationException
    {
        public string Title
        {
            get
            {
                return _title;
            }
        }
        string _title = null;


        public AssertionFailedException(string message, string title)
            : base(message)
        {
            _title = title;
        }

    }
}
