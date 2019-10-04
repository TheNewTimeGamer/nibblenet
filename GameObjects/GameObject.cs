using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibbleNet {
    public abstract class GameObject : ICloneable {

        public float x { get; set; }
        public float y { get; set; }

        public float width { get; set; }
        public float height { get; set; }

        public abstract void Tick(MainWindow instance);
        public abstract void Render(MainWindow instance);

        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}
