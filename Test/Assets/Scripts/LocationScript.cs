using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts;

public class LocationScript : MonoBehaviour {
    public GameObject PrefMast;
    //матрица с кодами биомов, где
    /* 0 - природный биом
     * 1 - человеческая шахта
     * 2 - гномья шахта
     * 3 - нетронутая прородная пещера
     * 4 - нетронутая гномья шахта
     */
    //матрица проходимости, где 0 - можно пройти, 1 - нельзя 
    public void Generate_World(Vector3 start, List<int[,]> full_info, float size)
    {
        int[,] level_simple_matrix = full_info[0];
        int[,] biomes_matrix = full_info[1];
        int[,] tracks = full_info[2];
        System.Random rand = new System.Random();
        PrefabMasterScript PrefMastScript = PrefMast.GetComponent<PrefabMasterScript>();

        double[] TorchPossibility = new double[] { 0.05, 0.6, 9 };
        double[] SupportPossibility = new double[] { 0.05, 1, 10 };

        int TorchCount = 0;
        int SupportCount = 0;

        int x_size = level_simple_matrix.GetUpperBound(0) + 1;
        int y_size = level_simple_matrix.Length / (level_simple_matrix.GetUpperBound(0) + 1);
        int[,] temp_matrix = new int[y_size + 2, x_size + 2];
        int[,] temp_biomes_matrix = new int[y_size + 2, x_size + 2];
        for (int i = 0; i < y_size + 2; i++)
        {
            for (int j = 0; j < x_size + 2; j++)
            {
                if (i == 0 || i == y_size + 1 || j == 0 || j == x_size + 1)
                {
                    temp_matrix[i, j] = 1;
                    temp_biomes_matrix[i, j] = 0;
                }
                else
                {
                    temp_matrix[i, j] = level_simple_matrix[i - 1, j - 1];
                    temp_biomes_matrix[i, j] = biomes_matrix[i - 1, j - 1];
                }
            }
        }
        int[,] level = temp_matrix;

        for (int i = 1; i < y_size + 1; i++)
        {
            for (int j = 1; j < x_size + 1; j++)
            {
                GameObject obj;
                int hesh;
                bool placed = false;
                if (tracks[i - 1, j - 1] != 0)
                {
                    Debug.Log("tracks " + tracks[i - 1, j - 1]);
                    placed = true;
                    int a = tracks[i - 1, j - 1] % 10;
                    int b = tracks[i - 1, j - 1] % 100 / 10;
                    int c = tracks[i - 1, j - 1] / 100;
                    if (a == 1)
                    {
                        Instantiate(PrefMastScript.Prefab_List[15], start + new Vector3(i * size, -0.9f, j * size), Quaternion.Euler(-90, 0, 0), transform.GetChild(1).transform);
                    }
                    else if (a == 2)
                    {
                        Instantiate(PrefMastScript.Prefab_List[15], start + new Vector3(i * size, -0.9f, j * size), Quaternion.Euler(-90, 90, 0), transform.GetChild(1).transform);
                    }
                    else if (a == 3)
                    {
                        Instantiate(PrefMastScript.Prefab_List[15], start + new Vector3(i * size, -0.9f, j * size), Quaternion.Euler(-90, -90, 0), transform.GetChild(1).transform);

                    }
                    else if (a == 4)
                    {
                        Instantiate(PrefMastScript.Prefab_List[15], start + new Vector3(i * size, -0.9f, j * size), Quaternion.Euler(-90, 180, 0), transform.GetChild(1).transform);
                    }

                    if (b == 1)
                    {
                        Instantiate(PrefMastScript.Prefab_List[14], start + new Vector3(i * size, -0.9f, j * size), Quaternion.Euler(-90, 0, 0), transform.GetChild(1).transform);
                    }
                    else if (b == 2)
                    {
                        Instantiate(PrefMastScript.Prefab_List[14], start + new Vector3(i * size, -0.9f, j * size), Quaternion.Euler(-90, 90, 0), transform.GetChild(1).transform);
                    }
                    if (c == 1)
                    {
                        Instantiate(PrefMastScript.Prefab_List[16], start + new Vector3(i * size, -0.13f, j * size), Quaternion.Euler(-90, 0, 0), transform.GetChild(1).transform);
                    }
                    else if (c == 2)
                    {
                        Instantiate(PrefMastScript.Prefab_List[16], start + new Vector3(i * size, -0.13f, j * size), Quaternion.Euler(-90, 90, 0), transform.GetChild(1).transform);
                    }
                }
                if (level[i, j] == 0)
                {

                    obj = Instantiate(PrefMastScript.Prefab_List[4], start + new Vector3(i * size, -size, j * size), Quaternion.Euler(0, 90, 90), transform.GetChild(0).transform);
                    if (!placed)
                    {
                        if (temp_biomes_matrix[i, j] == 0)
                        {
                            //шансы появления каждого из декоративных объектов в порядке, в котором объекты указаны в 
                            //prefabmaster . Первое число - шанс, что не появится ничего
                            List<float> chances = new List<float>() { 100, 10, 10, 2, 1, 1, 0, 0, 0 };
                            //список корректировок координаты расположения объектов, по умолчанию 0
                            List<float> y_corrections = new List<float>() { -0.9f, -0.7f, -1.1f, -0.8f, -0.7f, -0.7f, -0.7f, -0.7f };
                            int k = Math_lib.choose_random_from_weight_list(chances) - 1;
                            if (k < PrefMastScript.natural_objects_prefabs_list.Count && k != -1)
                            {
                                float delta = 0;
                                if (k < y_corrections.Count)
                                {
                                    delta = y_corrections[k];
                                }
                                Instantiate(PrefMastScript.natural_objects_prefabs_list[k], start + new Vector3(i * size, delta, j * size), Quaternion.Euler(-90, 0, 0), transform.GetChild(1).transform);
                            }
                        }
                        else if (temp_biomes_matrix[i, j] == 1)
                        {

                        }
                        else if (temp_biomes_matrix[i, j] == 2)
                        {
                            //шансы появления каждого из декоративных объектов в порядке, в котором объекты указаны в 
                            //prefabmaster . Первое число - шанс, что не появится ничего
                            List<float> chances = new List<float>() { 100, 0.2f, 0.05f, 1, 0, 0.05f, 0, 0, 0, 3, 0.1f };
                            //список корректировок координаты расположения объектов, по умолчанию 0
                            List<float> y_corrections = new List<float>() { -1 };
                            int k = Math_lib.choose_random_from_weight_list(chances) - 1;
                            if (k < PrefMastScript.dwarf_prefabs_list.Count && k != -1)
                            {
                                float delta = 0;
                                if (k < y_corrections.Count)
                                {
                                    delta = y_corrections[k];
                                }
                                Instantiate(PrefMastScript.dwarf_prefabs_list[k], start + new Vector3(i * size, delta, j * size), Quaternion.Euler(-90, 0, 0), transform.GetChild(1).transform);
                            }
                        }
                        else if (temp_biomes_matrix[i, j] == 3)
                        {
                            //шансы появления каждого из декоративных объектов в порядке, в котором объекты указаны в 
                            //prefabmaster . Первое число - шанс, что не появится ничего
                            List<float> chances = new List<float>() { 100, 10, 25, 5, 5, 10, 2, 0.67f, 1 };
                            //список корректировок координаты расположения объектов, по умолчанию 0
                            List<float> y_corrections = new List<float>() { -0.9f, -0.7f, -1.1f, -0.8f, -0.7f, -0.7f, -0.7f, -0.7f };
                            int k = Math_lib.choose_random_from_weight_list(chances) - 1;
                            if (k < PrefMastScript.natural_objects_prefabs_list.Count && k != -1)
                            {
                                float delta = 0;
                                if (k < y_corrections.Count)
                                {
                                    delta = y_corrections[k];
                                }
                                Instantiate(PrefMastScript.natural_objects_prefabs_list[k], start + new Vector3(i * size, delta, j * size), Quaternion.Euler(-90, 0, 0), transform.GetChild(1).transform);
                            }
                        }
                        else if (temp_biomes_matrix[i, j] == 4)
                        {
                            //шансы появления каждого из декоративных объектов в порядке, в котором объекты указаны в 
                            //prefabmaster . Первое число - шанс, что не появится ничего
                            List<float> chances = new List<float>() { 100, 2, 0.5f, 5, 0.9f, 2, 2, 1, 0.4f, 5, 0.5f };
                            //список корректировок координаты расположения объектов, по умолчанию 0
                            List<float> y_corrections = new List<float>() { -1 };
                            int k = Math_lib.choose_random_from_weight_list(chances) - 1;
                            if (k < PrefMastScript.dwarf_prefabs_list.Count && k != -1)
                            {
                                float delta = 0;
                                if (k < y_corrections.Count)
                                {
                                    delta = y_corrections[k];
                                }
                                Instantiate(PrefMastScript.dwarf_prefabs_list[k], start + new Vector3(i * size, delta, j * size), Quaternion.Euler(-90, 0, 0), transform.GetChild(1).transform);
                            }



                        }
                        else if (temp_biomes_matrix[i, j] == 5)
                        {
                            float a, b;
                            System.Random rnd = new System.Random();
                            for (int p = 0; p < 8; p++)
                            {
                                a = (float)(rnd.NextDouble() * (size + 0.0)-size);
                                b = (float)(rnd.NextDouble() * (size + 0.0)-size);
   
                                Instantiate(PrefMastScript.dwarf_prefabs_list[0], start + new Vector3(i * size, -1, j * size)+new Vector3(a,0,b), Quaternion.Euler(-90, 0, 0), transform.GetChild(1).transform);

                            }

                        }
                        else if (temp_biomes_matrix[i, j] == 6)
                        {
                            float a, b;
                            System.Random rnd = new System.Random();
                            for (int p = 0; p < 4; p++)
                            {
                                a = (float)(rnd.NextDouble() * (size + 0.0)-size);
                                b = (float)(rnd.NextDouble() * (size + 0.0)-size);
                                int c = rnd.Next(0, 2);
                                if (c==0)
                                {
                                    Instantiate(PrefMastScript.natural_objects_prefabs_list[2], start + new Vector3(i * size, -1.1f, j * size) + new Vector3(a, 0, b), Quaternion.Euler(-90, 0, 0), transform.GetChild(1).transform);
                                }
                                else
                                {
                                    Instantiate(PrefMastScript.natural_objects_prefabs_list[3], start + new Vector3(i * size, -0.7f, j * size) + new Vector3(a, 0, b), Quaternion.Euler(-90, 0, 0), transform.GetChild(1).transform);
                                }

                            }

                        }

                    }
                }
                else
                {
                    int[][] indexses = new int[][] { new int[] { i - 1, j }, new int[] { i, j + 1 }, new int[] { i + 1, j }, new int[] { i, j - 1 } };
                    int[] neibs = new int[4];
                    for (int k = 0; k < neibs.Length; k++)
                    {
                        int p = level[indexses[k][0], indexses[k][1]];
                        neibs[k] = p;
                    }
                    int s = neibs[0] + neibs[1] + neibs[2] + neibs[3];
                    for (int t = 0; t < 1; t++)
                    {
                        if (s == 0)
                        {
                            obj = Instantiate(PrefMastScript.Prefab_List[6], start + new Vector3(i * size, t * size, j * size), Quaternion.Euler(-90, 0, 0), transform.GetChild(0).transform);
                        }
                        else if (s == 4)
                        {
                            obj = Instantiate(PrefMastScript.Prefab_List[1], start + new Vector3(i * size, t * size, j * size), Quaternion.Euler(-90, 0, 0), transform.GetChild(0).transform);
                        }
                        else
                        {
                            if (s == 3)
                            {
                                hesh = Array.IndexOf(neibs, 0);
                                obj = Instantiate(PrefMastScript.Prefab_List[2], start + new Vector3(i * size, t * size, j * size), Quaternion.Euler(-90, (hesh + 2) * 90, 0), transform.GetChild(0).transform);
                            }
                            else if (s == 1)
                            {
                                hesh = Array.IndexOf(neibs, 1);
                                obj = Instantiate(PrefMastScript.Prefab_List[5], start + new Vector3(i * size, t * size, j * size), Quaternion.Euler(-90, (hesh) * 90, 0), transform.GetChild(0).transform);

                            }
                            else if (s == 2)
                            {
                                hesh = Array.IndexOf(neibs, 1);
                                if (neibs[(hesh + 1) % 4] == 1 || neibs[(hesh + 3) % 4] == 1)
                                {
                                    if (neibs[(hesh + 3) % 4] == 1)
                                    {
                                        hesh = 3;
                                    }
                                    obj = Instantiate(PrefMastScript.Prefab_List[3], start + new Vector3(i * size, t * size, j * size), Quaternion.Euler(-90, (hesh + 1) * 90, 0), transform.GetChild(0).transform);
                                }
                                else
                                {
                                    obj = Instantiate(PrefMastScript.Prefab_List[4], start + new Vector3(i * size, t * size, j * size), Quaternion.Euler(-90, (hesh + 1) * 90, 0), transform.GetChild(0).transform);
                                }
                            }
                        }

                    }
                    //Создание Факлов и Подпорок
                    if (s < 4)
                    {
                        bool BOI_Not_Taken = true;
                        double TruePossibility = 0.00;
                        int r = rand.Next(0, 100);
                        GameObject torch;
                        GameObject support;
                        //Debug.Log(r);
                        //Debug.Log(SupportCount);
                        if (SupportCount >= SupportPossibility[2])
                        {
                            TruePossibility = SupportPossibility[1] * 100;
                        }
                        else
                        {
                            TruePossibility = SupportPossibility[0] * 100;
                        }
                        if (r <= TruePossibility && BOI_Not_Taken)
                        {
                            SupportCount = 0;
                            BOI_Not_Taken = false;
                            List<int> goods = new List<int>();
                            for (int o = 0; o < neibs.Length; o++)
                            {
                                if (neibs[o] == 0)
                                {
                                    goods.Add(o);
                                }
                            }
                            int ind = goods[rand.Next(0, goods.Count)];
                            support = Instantiate(PrefMastScript.Prefab_List[7], start + new Vector3((float)(i + indexses[ind][0]) / 2 * size, -1, (float)(j + indexses[ind][1]) / 2 * size), Quaternion.Euler(-90, (ind - 1) * 90, 0), transform.GetChild(1).GetChild(0).transform);
                        }
                        else
                        {
                            SupportCount++;
                            r = rand.Next(0, 100);
                        }

                        if (TorchCount >= TorchPossibility[2])
                        {
                            TruePossibility = TorchPossibility[1] * 100;
                        }
                        else
                        {
                            TruePossibility = TorchPossibility[0] * 100;
                        }
                        if (r <= TruePossibility && BOI_Not_Taken)
                        {
                            TorchCount = 0;
                            BOI_Not_Taken = false;
                            List<int> goods = new List<int>();
                            for (int o = 0; o < neibs.Length; o++)
                            {
                                if (neibs[o] == 0)
                                {
                                    goods.Add(o);
                                }
                            }
                            int ind = goods[rand.Next(0, goods.Count)];
                            torch = Instantiate(PrefMastScript.Prefab_List[8], start + new Vector3((float)(i + indexses[ind][0]) / 2 * size, (float)0.3, (float)(j + indexses[ind][1]) / 2 * size), Quaternion.Euler(-90, (ind - 1) * 90, 0), transform.GetChild(1).GetChild(0).transform);
                        }
                        else
                        {
                            TorchCount++;
                            r = rand.Next(0, 100);
                        }
                    }
                }
                    //obj.transform.parent = transform;  
                }
            }
            return;
        }
    
	// Use this for initialization
	void Start () {
        List<int[,]> mask = Tech_mine_maker.really_make_mine(200);
        for(int i = 0; i<mask[0].Length/(mask[0].GetUpperBound(0)+1);i++){
        	for(int j = 0; j < mask[0].GetUpperBound(0)+1;j++){
        		mask[0][i,j] = Math.Abs(1-mask[0][i,j]);
        	}
        }
        Generate_World(transform.position, mask, 2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
