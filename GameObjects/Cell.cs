using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NibbleNet {
    public class Cell : DynamicGameObject{

        public int type = 0;
        public float speed = 5;

        public float foodMax { get; set; } = 10;
        public float food { get; set; } = 5;

        public Cell(float x, float y) : base(x, y, 50, 50) { }

        public override void Tick(MainWindow instance) {
            base.Tick(instance);

            if(food <= 0.0f) {
                instance.objects.Remove(this);
            }

            this.width = 5 * (food*2);
            this.height = 5 * (food*2);

            if(this.food > this.foodMax) {
                this.food /= 2;
                Cell cell = (Cell)this.Clone();
                cell.color = new Color(cell.color);
                cell.mutate();
                instance.objects.Add(cell);
                this.x -= 25;
            }

            StaticGameObject closest = null;
            int closestDistance = 9999;
            for(int i = 0; i < instance.objects.Count(); i++) {
                StaticGameObject obj = (StaticGameObject)instance.objects[i];
                if (obj is Food && (type == 0 || type == 2)) {
                    int distance = (int)(Math.Abs(obj.x-this.x)+Math.Abs(obj.y-this.y));
                    if(distance < closestDistance) {
                        closestDistance = distance;
                        closest = obj;
                    }
                }
                
                if (obj is Cell && (type == 1 || type == 2)) {
                    if (obj.color.GetRgb() != this.color.GetRgb()) {
                        int distance = (int)(Math.Abs(obj.x - this.x) + Math.Abs(obj.y - this.y));
                        if (distance < closestDistance) {
                            Cell c = (Cell)obj;
                            if (this.type == 1) {
                                if (this.food > c.food / 2) {
                                    closestDistance = distance;
                                    closest = obj;
                                }
                            } else {
                                if (this.food > c.food) {
                                    closestDistance = distance;
                                    closest = obj;
                                }
                            }
                        }
                    }
                }
            }

            if (closest != null) {
                double r = Math.Atan2(closest.y - this.y, closest.x - this.x);
                this.velocityX = (float)Math.Cos(r) * speed;
                this.velocityY = (float)Math.Sin(r) * speed;

                food -= 0.01f * speed;

                if (closestDistance < 50) {
                    if (closest.intersects(this)) {
                        if (closest is Food) {
                            this.food += ((Food)closest).foodValue;
                            instance.objects.Remove(closest);
                        }else if(closest is Cell) { 
                            this.food += ((Cell)closest).food;
                            instance.objects.Remove(closest);
                        }
                    }
                }

            }
        }

        public override void Render(MainWindow instance) {
            if (type == 0) {
                instance.color.setColor(0, 255, 0);
            } else if (type == 1) {
                instance.color.setColor(255, 0, 0);
            } else {
                instance.color.setColor(0, 0, 255);
            }
            instance.fillRect((int)x-1, (int)y-1, (int)width+4, (int)height+4);
            base.Render(instance);
        }

        public void mutate() {
            Random r = new Random();
            this.speed += (((float)r.Next(10)) - 5) / 5;
            this.foodMax += (((float)r.Next(10)) - 5) / 5;

            int cc = r.Next(100);
            if(cc < 50) {
                int[] mod = new int[3];
                mod[0] = r.Next(3)-1;
                mod[1] = r.Next(3)-1;
                mod[2] = r.Next(3)-1;
                
                if (mod[0] == -1) { color.Red -= 51; } else if (mod[0] == 1) { color.Red += 51; }
                if (mod[1] == -1) { color.Green -= 51; } else if (mod[1] == 1) { color.Green += 51; }
                if (mod[2] == -1) { color.Blue -= 51; } else if (mod[2] == 1) { color.Blue += 51; }
            }
            cc = r.Next(100);
            if(cc < 20) {
                type = r.Next(3);
                if(type == 3 && speed >= 3) { speed = 2; }
            }

        }

    }
}
