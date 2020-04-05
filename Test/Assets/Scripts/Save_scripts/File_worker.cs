using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleApplication1.Not_unity_scripts.new_scripts
{
     class File_worker
    {
      public List<Save_message> cache=new List<Save_message>();
      public string saves = "saves.txt";
         public File_worker()
        {
            File.AppendAllText(saves,"");
            string source = File.ReadAllText(saves);
            cache=string_to_save_message_list(source);
        }
        int index_of_this_tag(string simple_tag,List<Save_message> sm)
        {
            int n=-1;
            for (int i=0;i<sm.Count;i++)
            {
                if (sm[i].get_tag().Equals(simple_tag))
                {
                    n=i;
                    break;
                }
            }
            return(n);
        }
        public  List<Save_message> string_to_save_message_list(string s)
        {
            List<Save_message> mes=new List<Save_message>();
            string raw="";
            int c=0;
            char[] chars = s.ToCharArray();
            for (int i=0;i<chars.Length;i++)
            {
                if (chars[i]=='{')
                {
                    if (c != 0)
                    {
                        raw += chars[i];
                    }
                    c++;
                }
                else if (chars[i]=='}')
                {
                    if (c != 1)
                    {
                        raw += chars[i];
                    }
                    c--;
                }
                else
                {
                     raw+=chars[i];
                }
                if (c==0&&!raw.Equals(""))
                {
                    mes.Add(new Save_message(raw));
                     raw="";
                }
            }
           
            return(mes);
        }
        public void save_string(List<Save_message> mes,Save_tag tag,string s)
         {
             
             string simple_tag=tag.get_tag(0);
             int k=index_of_this_tag(simple_tag,mes);
            if (k==-1)
            {
                k=mes.Count;
                mes.Add(new Save_message(simple_tag,""));
            }
            if (tag.tags_count()==1)
            {
                mes[k].set_text(s);
            }
            else
            {
                Save_tag new_tag = new Save_tag(tag.get_tag(1));
                for (int i=2;i<tag.tags_count();i++)
                {
                    new_tag.add_tag(tag.get_tag(i));
                }
                List<Save_message> tmp =string_to_save_message_list(mes[k].get_text());
                save_string(tmp,new_tag,s);
                string d = "";
                for (int i = 0; i < tmp.Count; i++)
                {
                    d += tmp[i].full_text();
                }
                mes[k].set_text(d);
            }

         }
        public string read_save(Save_tag t)
        {
            string s="";
            List<Save_message> tmp = cache;
            for (int i=0;i<(t.tags_count()-1);i++)
            {
                int k=index_of_this_tag(t.get_tag(i),tmp);
                if (k!=-1)
                {
                    tmp=string_to_save_message_list(tmp[k].get_text());
                }
                else
                {
                    tmp=new List<Save_message>();
                    break;
                }
            }
            int r=index_of_this_tag(t.get_tag(t.tags_count()-1),tmp);
                if (r!=-1)
                {
                    s=tmp[r].get_text();
                }
            return(s);
        }
        public void save_to_file()
        {
            string s="";
            for (int i=0;i<cache.Count;i++)
            {
                s+=cache[i].full_text();
            }
            File.WriteAllText(saves,s);
        }
        public void clear_all_saves()
        {
            cache.Clear();
        }
    }
}
