using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Not_unity_scripts;
using Assets.Scripts.Not_unity_scripts.Events;
using Assets.Scripts.Not_unity_scripts.Conditions;
using Assets.Scripts.Not_unity_scripts.Actions;

namespace ConsoleApplication1.Not_unity_scripts.new_scripts
{
      class Saves_master
    {
          public static File_worker fw;
          public Saves_master()
          {
              fw = new File_worker();
          }
          
          public static void save_Inventory(Save_tag path,Inventory inv)
          {
              Save_tag a = path.copy();
              a.add_tag("IDs");
              save_list_of_long_ints(a, inv.codes);
              Save_tag b = path.copy();
              b.add_tag("numbers");
              save_list_of_floats(b, inv.numbers);
              
          }
          public static Inventory load_Inventory(Save_tag path)
          {
              Inventory inv = new Inventory();
              Save_tag a = path.copy();
              a.add_tag("IDs");
             inv.codes= load_list_of_long_ints(a);
              Save_tag b = path.copy();
              b.add_tag("numbers");
             inv.numbers= load_list_of_floats(b);
              return (inv);
          }
          public static void save_list_of_long_ints(Save_tag path, List<long> n)
          {
              string k = "";
              if (n.Count != 0)
              {
                  k = "" + n[0];
                  for (int i = 1; i < n.Count; i++)
                  {
                      k += "%" + n[i];
                  }
              }
              try
              {
                  fw.save_string(fw.cache, path, k);
              }
              catch (Exception e)
              {
                  System.Console.WriteLine("error");
              }
          }
          public static List<long> load_list_of_long_ints(Save_tag path)
          {
              List<long> tmp = new List<long>();
              string k=fw.read_save(path);
              string[] b =k.Split('%');
              for (int i=0;i<b.Length;i++)
              {
                  try
            {
                long p = Int64.Parse(b[i]);
                tmp.Add(p);
            }
            catch (Exception e)
            {

            }
              }
             
              return (tmp);
          }
          public static void save_list_of_ints(Save_tag path, List<int> n)
          {
              string k = "";
              if (n.Count != 0)
              {
                  k = "" + n[0];
                  for (int i = 1; i < n.Count; i++)
                  {
                      k += "%" + n[i];
                  }
              }
              try
              {
                  fw.save_string(fw.cache, path, k);
              }
              catch (Exception e)
              {
                  System.Console.WriteLine("error");
              }
          }
          public static List<int> load_list_of_ints(Save_tag path)
          {
              List<int> tmp = new List<int>();
              string k = fw.read_save(path);
              string[] b = k.Split('%');
              for (int i = 0; i < b.Length; i++)
              {
                  try
                  {
                      int p = Int32.Parse(b[i]);
                      tmp.Add(p);
                  }
                  catch (Exception e)
                  {

                  }
              }

              return (tmp);
          }     
          public static void save_list_of_strings(Save_tag path, List<string> n)
          {
              string k = "";
              if (n.Count != 0)
              {
                  k = "" + n[0];
                  for (int i = 1; i < n.Count; i++)
                  {
                      k += "`" + n[i];
                  }
              }
              try
              {
                  fw.save_string(fw.cache, path, k);
              }
              catch (Exception e)
              {
                  System.Console.WriteLine("error");
              }
          }
          public static List<string> load_list_of_strings(Save_tag path)
          {
              List<string> tmp = new List<string>();
              string k = fw.read_save(path);
              string[] b = k.Split('`');
              for (int i = 0; i < b.Length; i++)
              {
                  try
                  {
                      tmp.Add(b[i]);
                  }
                  catch (Exception e)
                  {

                  }
              }

              return (tmp);
          }
          public static void save_Event(Save_tag path,Event e)
          {
              Save_tag a = path.copy();
              a.add_tag("ID");
              save_long(a, e.get_ID());
              List<float> parameters = new List<float>();
              Save_tag c = path.copy();
              c.add_tag("text");
              save_string(c, e.get_info());
              Save_tag b = path.copy();
              b.add_tag("params");
              if (e.get_ID() == 1)
              {
                  Block_broken bb = (Block_broken)e;
                  parameters.Add(bb.ID_broken);
                  parameters.Add(bb.x);
                  parameters.Add(bb.y);
                  parameters.Add(bb.location);

              }
              else if (e.get_ID() == 2)
              {
                  Player_entered_fraction_base pefb = (Player_entered_fraction_base)e;
                  parameters.Add(pefb.fraction);
              }
              else if (e.get_ID() == 3)
              {
                  Players_inventory_changed pic = (Players_inventory_changed)e;
                  parameters.Add(pic.item_ID);
                  parameters.Add(pic.count);
              }
              save_list_of_floats(b, parameters);
          }
          public static Event load_Event(Save_tag path)
          {
              Event e = null;
              Save_tag a = path.copy();
              a.add_tag("ID");
              long ID = load_long(a);
              Save_tag b = path.copy();
              b.add_tag("text");
              string text = load_string(b);
              Save_tag c = path.copy();
              c.add_tag("params");
              List<float> ps = load_list_of_floats(c);
              if (ID == 1)
              {
                  e = new Block_broken((int)(ps[0]), (int)(ps[1]), (int)(ps[2]), (int)(ps[3]));
              }
              else if (ID == 2)
              {
                  e=new Player_entered_fraction_base((int)(ps[0]));
              }
              else if (ID==3)
              {
                  e=new Players_inventory_changed((long)(ps[0]),ps[1]);
              }
              return (e);
          }
          public static void save_list_of_Events(Save_tag path,List<Event> e)
      {
          Save_tag r = path.copy();
          r.add_tag("count");
          save_int(r, e.Count);
          for (int i = 0; i < e.Count; i++)
          {
              Save_tag er = path.copy();
              er.add_tag("" + i);
              save_Event(er, e[i]);
          }
      }
          public static List<Event> load_list_of_Events(Save_tag path)
          {
              List<Event> e = new List<Event>();
              Save_tag r = path.copy();
              r.add_tag("count");
              int p=load_int(r);
              for (int i=0;i<p;i++)
              {
                  Save_tag er = path.copy();
                  er.add_tag("" + i);
                  e.Add(load_Event(er));
              }

              return (e);
          }
          public static void save_Game_Action(Save_tag path, Game_Action c)
          {
              Save_tag a = path.copy();
              a.add_tag("ID");
              save_long(a, c.get_ID());
              List<float> parameters = new List<float>();
              Save_tag e = path.copy();
              e.add_tag("text");
              save_string(e, c.get_info());
              Save_tag b = path.copy();
              b.add_tag("params");
              if (c.get_ID() == 1)
              {
                  Give_player_something gpi = (Give_player_something)c;
                  parameters.Add(gpi.ID_given);
                  parameters.Add(gpi.amount_given);
              }
              else if (c.get_ID() == 2)
              {
                  Shift_quest_from_archive sqfa = (Shift_quest_from_archive)c;
                  parameters.Add(sqfa.code);
              }
              else if (c.get_ID() == 3)
              {
                  Change_relations cr = (Change_relations)c;
                  parameters.Add(cr.fraction);
                  parameters.Add(cr.delta);
              }
              else if (c.get_ID() == 4)
              {
                  Start_dialogue sd = (Start_dialogue)c;
                  Save_tag q= path.copy();
                  q.add_tag("dialogue");
                  save_Dialogue(q, sd.dia);
 
              }
              else if (c.get_ID() == 5)
              {
                  Deal_damage_to_player ddtp = (Deal_damage_to_player)c;
                  parameters.Add(ddtp.amount);
              }
              save_list_of_floats(b,parameters);
          }
          public static Game_Action load_Game_Action(Save_tag path)
          {
              Game_Action c = null;
              Save_tag a = path.copy();
              a.add_tag("ID");
              long ID =load_long(a);
              List<float> ps = new List<float>();
              Save_tag e = path.copy();
              e.add_tag("text");
              string s =load_string(e);
              Save_tag b = path.copy();
              b.add_tag("params");
              ps=load_list_of_floats(b);
              if (ID == 1)
              {
                  c = new Give_player_something((long)ps[0], ps[1]);

              }
              else if (ID == 2)
              {
                  c=new Shift_quest_from_archive((int)(ps[0]));
              }
              else if (ID==3)
              {
                  c=new Change_relations((int)ps[0],ps[1]);
              }
              else if (ID==4)
              {
                  Save_tag q = path.copy();
                  q.add_tag("dialogue");
                  Dialogue dd = load_Dialogue(q);
                  c = new Start_dialogue(dd);
 
              }
              else if (c.get_ID() == 5)
              {
                  c = new Deal_damage_to_player(ps[0]);
              }
              return (c);
          }
          public static void save_list_of_Game_Actions(Save_tag path, List<Game_Action> e)
          {
              Save_tag r = path.copy();
              r.add_tag("count");
              save_int(r, e.Count);
              for (int i = 0; i < e.Count; i++)
              {
                  Save_tag er = path.copy();
                  er.add_tag("" + i);
                  save_Game_Action(er, e[i]);
              }
          }
          public static List<Game_Action> load_list_of_Game_Actions(Save_tag path)
          {
              List<Game_Action> e = new List<Game_Action>();
              Save_tag r = path.copy();
              r.add_tag("count");
              int p = load_int(r);
              for (int i = 0; i < p; i++)
              {
                  Save_tag er = path.copy();
                  er.add_tag("" + i);
                  e.Add(load_Game_Action(er));
              }

              return (e);
          }
          public static void save_bool(Save_tag path, bool b)
          {
              int n = 0;
              if (b)
              {
                  n = 1;
              }
              save_int(path,n);
          }
          public static bool load_bool(Save_tag path)
          {
              int n = load_int(path);
              bool c = false;
              if (n == 1)
              { 
                  c = true;
              }
              return(c);
          }
          public static void save_Conditions_mask(Save_tag path, Conditions_mask c)
          {
              Save_tag a = path.copy();
              a.add_tag("simple_mask");
              save_bool(a, c.true_for_AND_mask_false_for_OR_mask);

          }
          public static Conditions_mask load_Conditions_mask(Save_tag path)
          {
              Save_tag a = path.copy();
              a.add_tag("simple_mask");
              Conditions_mask cm = new Conditions_mask(load_bool(a));
              return (cm);
          }
          public static void save_Condition(Save_tag path,Condition c)
          {
              Save_tag a = path.copy();
              a.add_tag("ID");
              save_long(a, c.get_ID());
              List<float> parameters = new List<float>();
              Save_tag e = path.copy();
              e.add_tag("text");
              save_string(e, c.get_info());
              Save_tag b = path.copy();
              b.add_tag("params");
              if (c.get_ID() == 1)
              {
                  parameters.Add(0.0f);
              }
              else if (c.get_ID() == 2)
              {
                  Cumulative_condition cc = (Cumulative_condition)c;
                  parameters.Add(cc.get_counter());
                  parameters.Add(cc.max_count);
              }
              else if (c.get_ID() == 3)
              {
                  Player_have_in_inventory phii = (Player_have_in_inventory)c;
                  parameters.Add(phii.item_ID);
                  parameters.Add(phii.count);
              }
              save_list_of_floats(b, parameters);
          }
          public static Condition load_Condition(Save_tag path)
          {
              Condition e = null;
              Save_tag a = path.copy();
              a.add_tag("ID");
              long ID = load_long(a);
              Save_tag b = path.copy();
              b.add_tag("text");
              string text = load_string(b);
              Save_tag c = path.copy();
              c.add_tag("params");
              List<float> ps = load_list_of_floats(c);
              if (ID == 1)
              {
                  e = new Empty_Condition();
              }
              else if (ID == 2)
              {
                  e = new Cumulative_condition((int)ps[1], (int)ps[0]);
              }
              else if (ID == 3)
              {
                  e = new Player_have_in_inventory((long)ps[0], ps[1]);
              }
              return(e);
          }
          public static void save_list_of_Conditions(Save_tag path, List<Condition> e)
          {
              Save_tag r = path.copy();
              r.add_tag("count");
              save_int(r, e.Count);
              for (int i = 0; i < e.Count; i++)
              {
                  Save_tag er = path.copy();
                  er.add_tag("" + i);
                  save_Condition(er, e[i]);
              }
          }
          public static List<Condition> load_list_of_Conditions(Save_tag path)
          {
              List<Condition> e = new List<Condition>();
              Save_tag r = path.copy();
              r.add_tag("count");
              int p = load_int(r);
              for (int i = 0; i < p; i++)
              {
                  Save_tag er = path.copy();
                  er.add_tag("" + i);
                  e.Add(load_Condition(er));
              }

              return (e);
          }
          public static void save_list_of_floats(Save_tag path, List<float> n)
          {
              string k = "";
              if (n.Count != 0)
              {
                   k = "" + n[0];
                  for (int i = 1; i < n.Count; i++)
                  {
                      k += "%" + n[i];
                  }  
              }
              
              try
              {
                  fw.save_string(fw.cache, path, k);
              }
              catch (Exception e)
              {
                  System.Console.WriteLine("error");
              }
          } 
          public static List<float> load_list_of_floats(Save_tag path)
          {
              List<float> tmp = new List<float>();
              string k = fw.read_save(path);
              string[] b = k.Split('%');
              for (int i = 0; i < b.Length; i++)
              {
                  try
                  {
                      float p =(float)(Double.Parse(b[i]));
                      tmp.Add(p);
                  }
                  catch (Exception e)
                  {

                  }
              }

              return (tmp);
          }
          public static void save_ints_matrix(Save_tag path,int[,] matrix)
          {
              string k = "";
              int ee = matrix.GetUpperBound(0)+1;
              for (int i = 0; i < ee; i++)
              {
                  for (int j = 0; j < matrix.Length / ee; j++)
                  {
                      k += matrix[i, j] + "";
                      if (j != (matrix.Length / ee - 1))
                      {
                          k += "%";
                      }
                  }
                  if (i != ee - 1)
                  {
                      k += "W";
                  }
              }
              try
              {
                  fw.save_string(fw.cache, path, k);
              }
              catch (Exception e)
              {
                  System.Console.WriteLine("error");
              }
          }
          public static int[,] load_ints_matrix(Save_tag path)
          {
              string source = fw.read_save(path);
              string[] ps = source.Split('W');
              int a = ps.Length;
              int b = ps[0].Split('%').Length;
              int[,] matrix = new int[a, b];
              for (int i = 0; i < a; i++)
              {
                  string[] tmp = ps[i].Split('%');
                  for (int j = 0; j < b; j++)
                  {
                      matrix[i, j] = Int32.Parse(tmp[j]);
                  }
              }
              return (matrix);
          }
          public static void save_int(Save_tag path, int n)
        {
            string k = "" + n;
            try
            {
                fw.save_string(fw.cache, path, k);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("error");
            }
        }
          public static int load_int(Save_tag path)
        {
            int n = Int32.MinValue;
            try
            {
                n = Int32.Parse(fw.read_save(path));
            }
            catch (Exception e)
            {

            }
            return (n);
        }
          public static void save_double(Save_tag path, double n)
        {
            string k = "" +n;
                fw.save_string(fw.cache, path, k);
        }
          public static double load_double(Save_tag path)
        {
            double n = Int32.MinValue;
            try {n = Double.Parse(fw.read_save(path));}
            catch (Exception e){}
            return (n);
        }
          public static void save_long(Save_tag path, long n)
        {
            string k = "" +n;
                fw.save_string(fw.cache, path, k);
        }
          public static long load_long(Save_tag path)
        {
            long n = Int32.MinValue;
            try {n = Int64.Parse(fw.read_save(path));}
            catch (Exception e){}
            return (n);
        }
          public static void save_string(Save_tag path, string n)
        {
            string k = "" +n;
                fw.save_string(fw.cache, path, k);
        }
          public static string load_string(Save_tag path)
        {
            string k="ERROR!!!";
            try {k=fw.read_save(path);}
            catch (Exception e){}
            return (k);
        }
          public static void save_Trigger(Save_tag path, Trigger t)
          {
              Save_tag a = path.copy();
              a.add_tag("events");
              save_list_of_Events(a, t.events);
              Save_tag b = path.copy();
              b.add_tag("conditions");
              save_list_of_Conditions(b, t.conditions);
              Save_tag c = path.copy();
              c.add_tag("actions");
              save_list_of_Game_Actions(c, t.actions);
              Save_tag d = path.copy();
              d.add_tag("deactivate_actions");
              save_list_of_Game_Actions(d, t.deactivate_actions);
              Save_tag e = path.copy();
              e.add_tag("conditions_mask");
              save_Conditions_mask(e, t.cm);
              Save_tag f = path.copy();
              f.add_tag("description");
              save_string(f, t.description);
              Save_tag g = path.copy();
              g.add_tag("active");
              save_bool(g, t.is_active());
              Save_tag h = path.copy();
              h.add_tag("disposable");
              save_bool(h, t.disposable);
              Save_tag i = path.copy();
              i.add_tag("endless");
              save_bool(i, t.endless);
              Save_tag j = path.copy();
              j.add_tag("time_to_deactivate");
              save_int(j, t.time_to_deactivate);
              Save_tag k = path.copy();
              k.add_tag("start_time");
              save_int(k, t.get_start_time());

          }
          public static Trigger load_Trigger(Save_tag path)
          {
              Save_tag a = path.copy();
              a.add_tag("events");
              List<Event> a1=load_list_of_Events(a);
              Save_tag b = path.copy();
              b.add_tag("conditions");
             List<Condition> b1= load_list_of_Conditions(b);
              Save_tag c = path.copy();
              c.add_tag("actions");
              List<Game_Action> c1 =load_list_of_Game_Actions(c);
              Save_tag d = path.copy();
              d.add_tag("deactivate_actions");
             List<Game_Action> d1= load_list_of_Game_Actions(d);
              Save_tag e = path.copy();
              e.add_tag("conditions_mask");
              Conditions_mask e1= load_Conditions_mask(e);
              Save_tag f = path.copy();
              f.add_tag("description");
              string f1= load_string(f);
              Save_tag g = path.copy();
              g.add_tag("active");
              bool g1= load_bool(g);
              Save_tag h = path.copy();
              h.add_tag("disposable");
              bool h1 = load_bool(h);
              Save_tag i = path.copy();
              i.add_tag("endless");
              bool i1= load_bool(i);
              Save_tag j = path.copy();
              j.add_tag("time_to_deactivate");
              int j1=load_int(j);
              Save_tag k = path.copy();
              k.add_tag("start_time");
              int k1 =load_int(k);
              Trigger tr = new Trigger(k1, i1, g1, a1, b1, c1, h1, j1, d1, e1, f1);
              return (tr);
          }
          public static void save_list_of_Triggers(Save_tag path, List<Trigger> e)
          {
              Save_tag r = path.copy();
              r.add_tag("count");
              save_int(r, e.Count);
              for (int i = 0; i < e.Count; i++)
              {
                  Save_tag er = path.copy();
                  er.add_tag("" + i);
                  save_Trigger(er, e[i]);
              }
          }
          public static List<Trigger> load_list_of_Triggers(Save_tag path)
          {
              List<Trigger> e = new List<Trigger>();
              Save_tag r = path.copy();
              r.add_tag("count");
              int p = load_int(r);
              for (int i = 0; i < p; i++)
              {
                  Save_tag er = path.copy();
                  er.add_tag("" + i);
                  e.Add(load_Trigger(er));
              }

              return (e);
          }
          public static void save_Console_message(Save_tag path, Console_message c)
          {
              List<string> strs = new List<string>() { c.author,c.message,c.time};
              save_list_of_strings(path,strs);
          }
          public static Console_message load_Console_message(Save_tag path)
          {
              List<string> strs=load_list_of_strings(path);
              Console_message cm = new Console_message(strs[0], strs[1], strs[2]);
              return (cm);
          }
          public static void save_Exchange_Pattern(Save_tag path, Exchange_Pattern e)
          {
              List<float> n = new List<float>() { e.ID_bought,e.amount_bought,e.ID_sell,e.amount_sell};
              save_list_of_floats(path,n);
          }
          public static Exchange_Pattern load_Exchange_Pattern(Save_tag path)
          {
              List<float> n = load_list_of_floats(path);
              Exchange_Pattern e = new Exchange_Pattern((long)n[0], (long)n[2], n[1], n[3]);
              return (e);
          }
          public static void save_list_of_Console_message(Save_tag path, List<Console_message> e)
          {
              Save_tag r = path.copy();
              r.add_tag("count");
              save_int(r, e.Count);
              for (int i = 0; i < e.Count; i++)
              {
                  Save_tag er = path.copy();
                  er.add_tag("" + i);
                  save_Console_message(er, e[i]);
              }
          }
          public static List<Console_message> load_list_of_Console_messages(Save_tag path)
          {
              List<Console_message> e = new List<Console_message>();
              Save_tag r = path.copy();
              r.add_tag("count");
              int p = load_int(r);
              for (int i = 0; i < p; i++)
              {
                  Save_tag er = path.copy();
                  er.add_tag("" + i);
                  e.Add(load_Console_message(er));
              }

              return (e);
          }
          public static void save_list_of_Exchange_Patterns(Save_tag path, List<Exchange_Pattern> e)
          {
              Save_tag r = path.copy();
              r.add_tag("count");
              save_int(r, e.Count);
              for (int i = 0; i < e.Count; i++)
              {
                  Save_tag er = path.copy();
                  er.add_tag("" + i);
                  save_Exchange_Pattern(er, e[i]);
              }
          }
          public static List<Exchange_Pattern> load_list_of_Exchange_Patterns(Save_tag path)
          {
              List<Exchange_Pattern> e = new List<Exchange_Pattern>();
              Save_tag r = path.copy();
              r.add_tag("count");
              int p = load_int(r);
              for (int i = 0; i < p; i++)
              {
                  Save_tag er = path.copy();
                  er.add_tag("" + i);
                  e.Add(load_Exchange_Pattern(er));
              }

              return (e);
          }
          public static void save_Trader(Save_tag path, Trader t)
          {
              Save_tag a = path.copy();
              a.add_tag("Inventory");
              save_Inventory(a, t);
              Save_tag b = path.copy();
              b.add_tag("inf_res");
              save_bool(b, t.infinity_resources);
              Save_tag c = path.copy();
              c.add_tag("change_rel");
              save_bool(c, t.change_relations);
              Save_tag d = path.copy();
              d.add_tag("fraction");
              save_int(d, t.fraction);
              Save_tag e = path.copy();
              e.add_tag("change");
              save_double(e, t.changing_per_purchase);
              Save_tag f = path.copy();
              f.add_tag("ex_patterns");
              save_list_of_Exchange_Patterns(f, t.patterns);
          }
          public static Trader load_Trader(Save_tag path)
          {
              Save_tag a = path.copy();
              a.add_tag("Inventory");
              Inventory a1=load_Inventory(a);
              Save_tag b = path.copy();
              b.add_tag("inf_res");
              bool b1 =load_bool(b);
              Save_tag c = path.copy();
              c.add_tag("change_rel");
              bool c1 =load_bool(c);
              Save_tag d = path.copy();
              d.add_tag("fraction");
              int d1= load_int(d);
              Save_tag e = path.copy();
              e.add_tag("change");
              float e1=(float)load_double(e);
              Save_tag f = path.copy();
              f.add_tag("ex_patterns");
              List<Exchange_Pattern> f1=load_list_of_Exchange_Patterns(f);
              Trader tr = new Trader(a1, f1, b1, c1, d1, e1);
              return (tr);
          }
          public static void save_Speech(Save_tag path, Speech s)
          {
              Save_tag a = path.copy();
              a.add_tag("from_player");
              save_bool(a, s.from_player);
              Save_tag b = path.copy();
              b.add_tag("text");
              save_string(b, s.text);
              Save_tag c = path.copy();
              c.add_tag("Actions");
              save_list_of_Game_Actions(c, s.actions);
          }
          public static Speech load_Speech(Save_tag path)
          {
              Save_tag a = path.copy();
              a.add_tag("from_player");
              bool a1=load_bool(a);
              Save_tag b = path.copy();
              b.add_tag("text");
              String b1=load_string(b);
              Save_tag c = path.copy();
              c.add_tag("Actions");
              List<Game_Action> c1 = load_list_of_Game_Actions(c);
              return(new Speech(b1,a1,c1));
          }
          public static void save_list_of_Speeches(Save_tag path, List<Speech> e)
          {
              Save_tag r = path.copy();
              r.add_tag("count");
              save_int(r, e.Count);
              for (int i = 0; i < e.Count; i++)
              {
                  Save_tag er = path.copy();
                  er.add_tag("" + i);
                  save_Speech(er, e[i]);
              }
          }
          public static List<Speech> load_list_of_Speeches(Save_tag path)
          {
              List<Speech> e = new List<Speech>();
              Save_tag r = path.copy();
              r.add_tag("count");
              int p = load_int(r);
              for (int i = 0; i < p; i++)
              {
                  Save_tag er = path.copy();
                  er.add_tag("" + i);
                  e.Add(load_Speech(er));
              }

              return (e);
          }
          public static void save_Graph(Save_tag path,Graph g)
          {
              Save_tag a = path.copy();
              a.add_tag("starts");
              save_list_of_ints(a, g.get_link_starts());
              Save_tag b = path.copy();
              b.add_tag("ends");
              save_list_of_ints(b, g.get_link_ends());

          }
          public static Graph load_Graph(Save_tag path)
          {
              Save_tag a = path.copy();
              a.add_tag("starts");
              List<int>a1=load_list_of_ints(a);
              Save_tag b = path.copy();
              b.add_tag("ends");
              List<int>b1=load_list_of_ints(a);
              Graph g = new Graph(a1, b1);
              return (g);
          }
          public static void save_Dialogue(Save_tag path, Dialogue d)
          {
              Save_tag a = path.copy();
              a.add_tag("speaker");
              Save_tag b = path.copy();
              b.add_tag("graph");
              Save_tag c = path.copy();
              c.add_tag("speeches");
              save_string(a, d.speaker);
              save_Graph(b, d.graph);
              save_list_of_Speeches(c, d.speeches);
          }
          public static Dialogue load_Dialogue(Save_tag path)
          {
              Save_tag a = path.copy();
              a.add_tag("speaker");
              Save_tag b = path.copy();
              b.add_tag("graph");
              Save_tag c = path.copy();
              c.add_tag("speeches");
              string a1 = load_string(a);
              Graph b1 = load_Graph(b);
              List<Speech> c1 = load_list_of_Speeches(c);
              Dialogue d = new Dialogue(a1, b1, c1);
              return (d);
          }
      }
}
