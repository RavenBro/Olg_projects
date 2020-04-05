using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class Tech_mine_maker
    {
        static int[,] blocks;
        static int[,] tmp_blocks;
        static int[,] biomes;
        static int[,] tracks;
        static int size;
        public static List<int[,]> really_make_mine(int size)
        {
            //создаем матрицы побольше, чтобы избежать выхода за пределы массива
             blocks = new int[size + 100, size + 100];
            biomes = new int[size + 100, size + 100];
            tracks = new int[size, size];
            Tech_mine_maker.size = size;
            System.Random rnd = new System.Random();
            //определяем количество гномьих шахт. Хотя бы одна шахта в пещере всегда есть. Количество шахт зависит от размера
            //здесь и далее константы не несут особого смысла - просто случайно придуманные числа
            int number_of_dwarf_mines = rnd.Next(1, 1+(size * size) / 3769);
            for (int i=0;i<number_of_dwarf_mines;i++)
            {
                //центр гномьей шахты должен создаваться только там, где уже нет гномьего биома. Это не предотвращает
                // наложение, но делает его менее вероятным. Если покаким-то причинам мы не нашли блок без гномьего биома
                //то cоздаем как придется, чтобы избежать бесконечного цикла
                int count = 0;
                int center_x = rnd.Next(size);
                int center_y = rnd.Next(size);
                while (count<137&&biomes[center_y,center_x]==2&&
                    ((center_x-size/2)* (center_x - size / 2) - (center_y - size / 2) * (center_y - size / 2) < size*size/36))
                {
                    count++;
                     center_x = rnd.Next(size);
                     center_y = rnd.Next(size);
                }
                make_dwarf_mine(center_x+50, center_y+50);
            }
            int number_of_natural_caves = rnd.Next(1, 1 + (size * size) / 2397);
            for (int i=0;i<15;i++)
            {
                int count = 0;
                int center_x = rnd.Next(size);
                int center_y = rnd.Next(size);
                while (count < 137 && biomes[center_y, center_x] == 2)
                {
                    count++;
                    center_x = rnd.Next(size);
                    center_y = rnd.Next(size);
                }
                make_natural_cave(center_x+50, center_y+50);
            }
            //cоздание особых комнат
            int k = rnd.Next(10);
            if (k<8)
            {
                int x = 0;
                int y = 0;
                if (k/2==0)
                {
                    x = rnd.Next(size -20, size -8);
                    y = rnd.Next(size -20, size -8);
                }
                else if (k / 2 == 1)
                {
                    x = rnd.Next(size -20, size -8);
                    y = rnd.Next(58, 70);

                }
                else if (k / 2 == 2)
                {
                    x = rnd.Next(58,70);
                    y = rnd.Next(size -20, size -8);

                }
                else if (k / 2 == 3)
                {
                    x = rnd.Next(58, 70);
                    y = rnd.Next(58, 70);

                }
                generate_skeletons_room(x, y);
            }
            k = rnd.Next(10);
            if (k < 8)
            {
                int x = 0;
                int y = 0;
                if (k / 2 == 0)
                {
                    x = rnd.Next(size - 20, size - 8);
                    y = rnd.Next(size - 20, size - 8);
                }
                else if (k / 2 == 1)
                {
                    x = rnd.Next(size - 20, size - 8);
                    y = rnd.Next(58, 70);

                }
                else if (k / 2 == 2)
                {
                    x = rnd.Next(58, 70);
                    y = rnd.Next(size - 20, size - 8);

                }
                else if (k / 2 == 3)
                {
                    x = rnd.Next(58, 70);
                    y = rnd.Next(58, 70);

                }
                generate_crystalls_room(x, y);
            }
            make_mine(size);
            find_hidden_rooms();
            //переводим увеличенные матрицы в обычные и делаем так, чтобы края карты всегда были камнем
            int[,] final_0_1_matrix = new int[size, size];
            int[,] final_biome_matrix = new int[size, size];
            for (int i=0;i<size;i++)
            {
                for (int j=0;j<size;j++)
                {
                    final_biome_matrix[i, j] = biomes[i + 50, j + 50];
                    final_0_1_matrix[i, j] = blocks[i + 50, j + 50];
                    if (i < 2 || j < 2 || i > size - 3 || j > size - 3)
                    {
                        final_biome_matrix[i, j] = 0;
                        final_0_1_matrix[i, j] = 0;
                    }
                }
            }
            List<int[,]> ret = new List<int[,]> { final_0_1_matrix,final_biome_matrix,tracks};
            return (ret);
        }
        static void generate_skeletons_room(int x,int y)
        {
            int room_size = 8;
            System.Random rnd = new System.Random();
            remove_block(x, y, 5);
            int x1 = rnd.Next(1 + room_size / 2);
            int x2 = rnd.Next(1 + room_size / 2);
            int y1 = rnd.Next(1 + room_size / 2);
            int y2 = rnd.Next(1 + room_size / 2);
            for (int j = y - y1; j < y + y2; j++)
            {
                for (int k = x - x1; k < x + x2; k++)
                {
                    remove_block(k, j, 5);
                }
            }
        }
        static void generate_crystalls_room(int x,int y)
        {
            System.Random rnd = new System.Random();
            int room_size = 8;
            int x1 = rnd.Next(5, 1 + room_size / 2);
            int x2 = rnd.Next(5, 1 + room_size / 2);
            int y1 = rnd.Next(2, room_size / 2);
            int y2 = rnd.Next(2, room_size / 2);
            for (int j = x; j < x + x1; j++)
            {
                for (int i = y - y2; i < y + y1; i++)
                {
                    remove_block(j, i, 6);
                }
                List<float> chances = new List<float>() { 1 - (x - j), 6 - 4 * (x - j), 4, 6, 1 };
                y1 += Math_lib.choose_random_from_weight_list(chances) - 2;
                y2 += Math_lib.choose_random_from_weight_list(chances) - 2;
                if (y1 <= 0)
                {
                    y1 = 1;
                }
                if (y2 < 0)
                {
                    y2 = 1;
                }
            }
            for (int j = x; j > x - x1; j--)
            {
                for (int i = y - y2; i < y + y1; i++)
                {
                    remove_block(j, i, 6);
                }
                List<float> chances = new List<float>() { 1 + (x - j), 6 + 4 * (x - j), 4, 6, 1 };
                y1 += Math_lib.choose_random_from_weight_list(chances) - 2;
                y2 += Math_lib.choose_random_from_weight_list(chances) - 2;
                if (y1 <= 0)
                {
                    y1 = 1;
                }
                if (y2 < 0)
                {
                    y2 = 1;
                }
            }
        }
        static void find_hidden_rooms()
        {
            //находим участки карты, в которые нельзя попасть из места появления игрока
            //и изменяем им биом на нетронутые шахты/пещеры.
            tmp_blocks = (int[,])blocks.Clone();
            int x = 50 + size / 2;
            int y = x;
            flood_block(y, x);
            for (int i=0;i<size+100;i++)
            {
                for (int j=0;j<size+100;j++)
                {
                    if (tmp_blocks[i,j]==1)
                    {
                        if (biomes[i,j]==0)
                        {
                            biomes[i, j] = 3;
                        }
                        else if (biomes[i, j] == 2)
                        {
                            biomes[i, j] = 4;
                        }
                    }
                }
            }
        }
        static void flood_block(int y,int x)
        {
            tmp_blocks[y, x] = 1000;
            if (x>0)
            {
                if (tmp_blocks[y, x-1] == 1)
                { flood_block(y, x - 1); }

            }
            if (y > 0)
            {
                if (tmp_blocks[y-1, x] == 1)
                { flood_block(y-1, x); }

            }
            if (x+1<size+100)
            {
                if (tmp_blocks[y,x+1]==1)
                {
                    flood_block(y, x + 1);
                }
            }
            if (y + 1 < size+100)
            {
                if (tmp_blocks[y+1, x] == 1)
                {
                    flood_block(y+1, x);
                }
            }

        }
        static void make_natural_cave(int x,int y)
        {
            System.Random rnd = new System.Random();
            int room_size = 10;
            int x1 = rnd.Next(5,1 + room_size / 2);
            int x2 = rnd.Next(5,1 + room_size / 2);
            int y1 = rnd.Next(2,room_size / 2);
            int y2 = rnd.Next(2,room_size / 2);
            for (int j=x;j<x+x1;j++)
            {
                for (int i=y-y2; i<y+y1;i++)
                {
                    remove_block(j, i, 0);
                }
                List<float> chances = new List<float>() { 1 - (x - j), 6 - 4 * (x - j), 4,6,1};
                y1 += Math_lib.choose_random_from_weight_list(chances) - 2;
                y2 += Math_lib.choose_random_from_weight_list(chances) - 2;
                if (y1<=0)
                {
                    y1 = 1;
                }
                if (y2<0)
                {
                    y2 = 1;
                }
            }
            for (int j = x; j > x - x1; j--)
            {
                for (int i = y - y2; i < y + y1; i++)
                {
                    remove_block(j, i, 0);
                }
                List<float> chances = new List<float>() { 1+(x-j), 6+4 * (x - j), 4, 6, 1 };
                y1 += Math_lib.choose_random_from_weight_list(chances) - 2;
                y2 += Math_lib.choose_random_from_weight_list(chances) - 2;
                if (y1 <= 0)
                {
                    y1 = 1;
                }
                if (y2 < 0)
                {
                    y2 = 1;
                }
            }
        }
        static void make_dwarf_mine(int x,int y)
        {
            // сначала слздаем центры комнат. Непонятно, зато работает.
            int room_size = 10;
            List<int> rooms_x = new List<int>() {x};
            List<int> rooms_y = new List<int>() {y};
            System.Random rnd = new System.Random();
            for (int i=-2;i<=2;i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    double r = j*j + i*i;
                    if (r>0&&r<5)
                    {
                        int s = rnd.Next(2 * (int)(r));
                        if (s!=1)
                        {
                            rooms_x.Add(x+j*room_size);
                            rooms_y.Add(y+i*room_size);
                        }
                    }
                }
            }
            //А теперь создаем сами комнаты случайного размера и коридоры между ними. Поскольку и комнаты, и коридоры
            //создаются случайно, то некоторые комнаты могут не иметь входа - до них нужно докопаться
            for (int i = 0; i < rooms_y.Count; i++)
            {
                remove_block(rooms_x[i], rooms_y[i], 2);
                int x1 = rnd.Next(1+room_size / 2);
                int x2 = rnd.Next(1+room_size / 2);
                int y1 = rnd.Next(1+room_size / 2);
                int y2 = rnd.Next(1+room_size / 2);
                for (int j = rooms_y[i] - y1; j < rooms_y[i] + y2; j++)
                {
                    for (int k = rooms_x[i] - x1; k < rooms_x[i] + x2; k++)
                    {
                        remove_block(k, j, 2);
                    }
                }
                for (int j = 0; j < rooms_y.Count; j++)
                {
                    if (rooms_y[i]==rooms_y[j])
                    {
                        int a = rooms_x[i];
                        int b = rooms_x[j];
                        if (a>b)
                        {
                            b = a;
                            a = rooms_x[j];
                        }
                        if (rnd.Next(4) != 3)
                        {
                            for (int k = a; k < b; k++)
                            {
                                remove_block(k, rooms_y[i], 2);
                            }
                        }
                    }
                    else if (rooms_x[i] == rooms_x[j])
                    {
                        int a = rooms_y[i];
                        int b = rooms_y[j];
                        if (a > b)
                        {
                            b = a;
                            a = rooms_y[j];
                        }
                        if (rnd.Next(4) != 3)
                        {
                            for (int k = a; k < b; k++)
                            {
                                remove_block(rooms_x[i], k, 2);
                            }
                        }
                    }
                }
            }
        }
         static void remove_block(int x,int y,int biome_to_set)
        {//если мы при создании шахты ломаем блок, то вызываем эту функцию, которая превращает этот блок и все соседние
         //в блоки указанного биома. Необходимо для правильной генерации декораций.
            //Debug.Log("broken " + x + "," + y);
            if (x>0&&y>0&&x<size+100&&y<size+100)
            {
                blocks[y, x] = 1;
                for (int i = y - 1; i < y + 2; i++)
                {
                    for (int j = x - 1; j < x + 2; j++)
                    {
                        if (i > 0 && j > 0 && i < size && j < size)
                        {
                            biomes[i, j] = biome_to_set;
                        }
                    }
                }
            }
            
        }
        public static List<Vector2> worm(int length, int x_start, int y_start, List<float> chances_of_every_direction)
        {
            List<Vector2> dots = new List<Vector2>() {new Vector2(x_start,y_start)};
            int x = x_start;
            int y = y_start;
            int dir = 0;
            int prev_dir = 0;
            for (int i=0;i<length;i++)
            {
                while(dir==prev_dir)
                {
                    dir = Math_lib.choose_random_from_weight_list(chances_of_every_direction);
                }
                
                if (dir==0)
                {
                    y++;
                }
                else if (dir==1)
                {
                    x++;
                }
                else if (dir==2)
                {
                    y--;
                }
                else if (dir==3)
                {
                    x--;
                }
                prev_dir=dir;
                Debug.Log("worm" + dir + " " + x + "," + y);
                dots.Add(new Vector2(x, y));
            }
            return (dots);
        }
        public static void make_mine(int size)
        {
            int[,] matrix = new int[size, size];
            System.Random rnd = new System.Random();
            int a = rnd.Next(size / 4, size / 2 - 1);
            int tr_a = rnd.Next(a, a);
            for (int i = 0; i < a; i++)
            {
                remove_block(50 + size / 2, 50 + size / 2 - i, 1);
                remove_block(50 + size / 2 + 1, 50 + size / 2 - i, 1);
                remove_block(50 + size / 2 - 1, 50 + size / 2 - i, 1);
                if (i <= tr_a)
                {
                    tracks[size / 2-i,size / 2] =20;
                }

            }
            int b = rnd.Next(size / 4, size / 2 - 1);
            tr_a = rnd.Next(b, b);
            for (int i = 0; i < b; i++)
            {
                remove_block(50 + size / 2, 50 + size / 2 + i, 1);
                remove_block(50 + size / 2 + 1, 50 + size / 2 + i, 1);
                remove_block(50 + size / 2 -1, 50 + size / 2 + i, 1);
                if (i <= tr_a)
                {
                    tracks[size / 2+i,size / 2] = 20;
                }
            }
            int c = (int)(rnd.NextDouble() * 7+3);
            for (int j = 0; j < c; j++)
            {
                int d = (int)(rnd.NextDouble() * (-0.9 + a + b) + size / 2 - a + 1.0)/5*5;
                int e = (int)(rnd.NextDouble() * (-2.1 + size / 2));
                int tr_l = rnd.Next(e / 2, e);
                int tr_r = rnd.Next(e / 2, e);
                int delta = 0;
                if (tracks[d,size/2]==20)
                {
                    int tmp = rnd.Next(1, 5);
                    if (tmp==0)
                    {
                        delta = 1;
                        if (d<size/2)
                        {
                            tracks[d, size/2] += 3;
                            tracks[d+1, size/2-1] += 1;
                        }
                        else
                        {
                            tracks[d-1, size / 2+1] += 4;
                            tracks[d, size / 2] += 2;
                        }
                        
                    }
                    else if (tmp == 1)
                    {
                        tr_r = 0;
                        if (d < size / 2)
                        {
                            tracks[d-1, size / 2] += 2;
                        }
                        else
                        {
                            tracks[d, size / 2 - 1] += 1;
                        }
                    }
                    else if (tmp == 2)
                    {
                        tr_l = 0;
                        if (d < size / 2)
                        {
                            tracks[d, size / 2 + 1] += 4;
                        }
                        else
                        {
                            tracks[d+1, size / 2] += 3;
                        }
                    }
                    else
                    {
                        tr_r = 0;
                        tr_l = 0;
                    }
                }
                else
                {
                    tr_r = 0;
                    tr_l = 0;
                }
                for (int i = 0; i < e; i++)
                {
                    remove_block(50 + size / 2 + i, 50 + d, 1);
                    remove_block(50 + size / 2 + i, 50 + d + 1, 1);
                    remove_block(50 + size / 2 + i, 50 + d - 1, 1);
                    if (i<=tr_r&&i!=0)
                    {
                        tracks[ d - delta, size / 2 + i] += 10;
                    }
                }
                e = (int)(rnd.NextDouble() * (-2.1 + size / 2));
                for (int i = 0; i < e; i++)
                {
                    remove_block(50 + size / 2 - i, 50 + d, 1);
                    remove_block(50 + size / 2 - i, 50 + d + 1, 1);
                    remove_block(50 + size / 2 - i, 50 + d - 1, 1);
                    if (i <= tr_l && i != 0)
                    {
                        tracks[ d+delta, size / 2 - i] += 10;
                    }
                }
                //расставляем вагонетки
                for (int i=0;i<size;i++)
                {
                    for (int t=0;t<size;t++)
                    {
                        if (tracks[i,t]==10)
                        {
                            int k = rnd.Next(30);
                            if (k==17)
                            {
                                tracks[i, t] += 100;
                            }
                        }
                        else if (tracks[i, t] == 20)
                        {
                            int k = rnd.Next(30);
                            if (k == 17)
                            {
                                tracks[i, t] += 200;
                            }
                        }

                    }
                }
                int x = (int)(rnd.NextDouble() * (-0.9 + a + b) + size / 2 - a + 1.0);
                int y = size / 2 - 1;
                int q = rnd.Next(4);
                List<float> chances = new List<float>() { 1, 1, 1, 1 };
                chances[q] += 4;
                List<Vector2> dots = worm(300, x, y, chances);
                for (int i = 0; i < dots.Count; i++)
                {
                    remove_block((int)dots[i].x + 50, (int)dots[i].y + 50, 1);
                }


            }
        }
    }
}
