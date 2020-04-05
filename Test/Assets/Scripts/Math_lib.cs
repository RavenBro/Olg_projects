using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class Math_lib
    {
        public static Random random = new Random();
        //Класс полный всяких нужных нам математических методов
        public static int choose_random_from_weight_list(List<float> weights)
        {
            int n = 0;
            float s = 0;
            for (int i=0;i<weights.Count;i++)
            {
                s += weights[i];
            }
            float tmp = 0;
            float rnd =(float)(random.NextDouble()*s);
            for (int i = 0; i < weights.Count; i++)
            {
                if (rnd>=tmp&&rnd<(tmp+weights[i]))
                {
                    n = i;
                    break;
                }
                tmp += weights[i];
            }
                return (n);
        }
        public static float calculate_z(float x,float y)
        {
            return (10 * (int)(y) - 0.01f * (int)(x));
        }
    }
}
