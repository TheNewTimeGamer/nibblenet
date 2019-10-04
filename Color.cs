using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibbleNet {
    public class Color {

        private byte red;
        private byte green;
        private byte blue;

        public byte Red {
            get { return this.red; }
            set {
                if(this.red >= 0 &&  this.red <= 255) {
                    this.red = value;
                }
            }
        }
        public byte Green {
            get { return this.green; }
            set {
                if (this.green >= 0 && this.green <= 255) {
                    this.green = value;
                }
            }
        }
        public byte Blue {
            get { return this.blue; }
            set {
                if (this.blue >= 0 && this.blue <= 255) {
                    this.blue = value;
                }
            }
        }

        public Color(byte red, byte green, byte blue) {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public Color(Color color) {
            this.red = color.Red;
            this.Green = color.Green;
            this.Blue = color.Blue;            
        }

        public void setColor(byte r, byte g, byte b) {
            this.Red = r;
            this.Green = g;
            this.Blue = b;
        }

        public void setColor(Color color) {
            this.Red = color.Red;
            this.Green = color.Green;
            this.Blue = color.Blue;
        }

        public void setColor(int rgb) {
            byte[] b = BitConverter.GetBytes(rgb);
            Red = b[0];
            Green = b[1];
            Blue = b[2];            
        }

        public int GetRgb() {
            return BitConverter.ToInt32(new byte[] { Red, Green, Blue, 255 }, 0);
        }

    }
}
