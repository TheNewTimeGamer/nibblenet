using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibbleNet {
    public class DynamicGameObject : StaticGameObject {    

        public float velocityX { get; set; }
        public float velocityY { get; set; }

        private float dragCoefficient = 0.05f;

        public DynamicGameObject(float x, float y) : base(x, y) { }
        public DynamicGameObject(float x, float y, float width, float height) : base(x, y, width, height){ }

        public override void Tick(MainWindow instance) {
            base.Tick(instance);

            this.x += this.velocityX;
            this.y += this.velocityY;

            if (velocityX < 0.0) { 
                velocityX += dragCoefficient * Math.Abs(velocityX);
            }else if(velocityX > 0.0) {
                velocityX -= dragCoefficient * Math.Abs(velocityX);
            }

            if (velocityY < 0.0) {
                velocityY += dragCoefficient * Math.Abs(velocityY);
            } else if (velocityY > 0.0) {
                velocityY -= dragCoefficient * Math.Abs(velocityY);
            }

            if (Math.Abs(velocityX) < 0.05f) { velocityX = 0; }
            if (Math.Abs(velocityY) < 0.05f) { velocityY = 0; }

        }
        
    }
}
