using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibbleNet {
    public class StaticGameObject : GameObject {

        public Color color = new Color(100, 50, 100);

        public StaticGameObject(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public StaticGameObject(float x, float y, float width, float height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public override void Tick(MainWindow instance) { }

        public override void Render(MainWindow instance) {
            instance.color.setColor(this.color);
            instance.fillRect((int)x, (int)y, (int)width, (int)height);
        }

        public Boolean intersects(StaticGameObject obj) {
            if(this.x+this.width > obj.x && this.x < obj.x + obj.width) {
                if (this.y + this.height > obj.y && this.y < obj.y + obj.height) {
                    return true;
                }
            }
            return false;
        }

    }
}
