using System;
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
using System.Windows.Forms;
using System.Threading;

namespace NibbleNet {
  
    public partial class MainWindow : Window {

        WriteableBitmap[] buffer = new WriteableBitmap[2];
        int active = 0;

        byte[] data;
        int stride;

        public Color color = new Color(0, 0, 0);
        public List<GameObject> objects = new List<GameObject>();
        //public DynamicGameObject[] cells = new DynamicGameObject[1024];

        public Point mousePos = new Point(0,0);

        int actualWidth, actualHeight;

        Boolean[] mouse = new Boolean[16];
        Boolean[] keyboard = new Boolean[1024];

        public MainWindow() {

            this.Closed += onClose;

            InitializeComponent();

            image.KeyDown += keyDown;
            image.KeyUp += keyUp;

            image.MouseDown += mouseDown;
            image.MouseUp += mouseUp;

            buffer[0] = new WriteableBitmap((int)image.Width, (int)image.Height, 96, 96, PixelFormats.Rgb24, null);
            buffer[1] = new WriteableBitmap((int)image.Width, (int)image.Height, 96, 96, PixelFormats.Rgb24, null);

            data = new byte[buffer[0].PixelWidth * buffer[0].PixelHeight * 3];
            stride = buffer[0].PixelWidth * 3;

            image.Source = buffer[0];

            Thread thread = new Thread(run);
            thread.Start();
    
        }

        public void mouseDown(Object sender, System.Windows.Input.MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) { mouse[0] = true; }
            if (e.MiddleButton == MouseButtonState.Pressed) { mouse[1] = true; }
            if (e.RightButton == MouseButtonState.Pressed) { mouse[2] = true; }
        }

        public void mouseUp(Object sender, System.Windows.Input.MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Released) { mouse[0] = false; }
            if (e.MiddleButton == MouseButtonState.Released) { mouse[1] = false; }
            if (e.RightButton == MouseButtonState.Released) { mouse[2] = false; }
        }

        public void keyDown(Object sender, System.Windows.Input.KeyEventArgs e) {
            int keyCode = KeyInterop.VirtualKeyFromKey(e.Key);
            keyboard[keyCode] = true;
        }
        public void keyUp(Object sender, System.Windows.Input.KeyEventArgs e) {
            int keyCode = KeyInterop.VirtualKeyFromKey(e.Key);
            keyboard[keyCode] = false;
        }

        public void show() {
            image.Dispatcher.Invoke(new Action(() => image.Source = buffer[active] ));
            active++;
            if (active >= buffer.Length) {
                active = 0;
            }            
        }

        public void onClose(Object sender, EventArgs e) {
            Environment.Exit(0);
        }

        public void run() {
            long last = 0;
            
            while (true) {
                if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - last > (1000 / 60)) {
                    tick();
                    last = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                }
                render();
            }
        }

        public void clear() {
            for(int i = 0; i < data.Length; i+=3) {
                data[i] = this.color.Red;
                data[i + 1] = this.color.Green;
                data[i + 2] = this.color.Blue;
            }

            buffer[active].Dispatcher.Invoke(new Action(() => { buffer[active].WritePixels(new Int32Rect(0, 0, buffer[active].PixelWidth, buffer[active].PixelHeight), data, stride, 0); }));
        }

        public void fillRect(int offsetX, int offsetY, int width, int height) {
            buffer[active].Dispatcher.Invoke(new Action(() => actualWidth = buffer[active].PixelWidth));
            buffer[active].Dispatcher.Invoke(new Action(() => actualHeight = buffer[active].PixelHeight));
            for (int y = 0; y < height; y+=3) {
                int actualY = (int)(y * actualWidth);
                for (int x = 0; x < width; x+=3) {
                    int pixelPosX = x + (offsetX * 3);
                    int pixelPosY = actualY + (offsetY * 3 * actualWidth);
                    int pixelPos = pixelPosX + pixelPosY;
                    if (pixelPos >= 0 && pixelPos + 2 < data.Length) {
                        if (pixelPosX >= 0 && pixelPosX < actualWidth * 3) {
                            data[pixelPos] = color.Red;
                            data[pixelPos + 1] = color.Green;
                            data[pixelPos + 2] = color.Blue;
                        }
                    }
                }
            }
            buffer[active].Dispatcher.Invoke(new Action(() => { buffer[active].WritePixels(new Int32Rect(0, 0, buffer[active].PixelWidth, buffer[active].PixelHeight), data, stride, 0); }));
        }

        Random random = new Random();

        public void tick() {

            // Why on earth do WPF and winForms have their own point class?
            System.Drawing.Point p = System.Windows.Forms.Control.MousePosition;
            if(p != null) {
                this.Dispatcher.Invoke(new Action(() => { mousePos.X = p.X - this.Left - 8; }));
                this.Dispatcher.Invoke(new Action(() => { mousePos.Y = p.Y - this.Top - 30; }));
            }

            int r = random.Next(100);
            if(r < 20) {
                objects.Add(new Food(random.Next(actualWidth), random.Next(actualHeight)));
            }

            if (mouse[0]) { 
                objects.Add(new Cell((float)mousePos.X, (float)mousePos.Y));
                mouse[0] = false;
            }


            for(int i = 0; i < objects.Count(); i++) {
                if(objects[i] != null) {
                    objects[i].Tick(this);
                }
            }

        }

        public void render() {
            color.setColor(0, 0, 0);
            clear();

            for (int i = 0; i < objects.Count(); i++) {
                if (objects[i] != null) {
                    objects[i].Render(this);
                }
            }

            show();
        }

    }

}
