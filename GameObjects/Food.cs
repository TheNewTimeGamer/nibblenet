using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibbleNet {
    class Food : StaticGameObject {

        public int foodValue { get; set; } = 3; // This value represents how long a cell's live gets extended in seconds when eaten.
        
        public Food(float x, float y) : base(x, y, 50, 50) {
            this.color.setColor(0, 255, 0);
        }

    }
}
