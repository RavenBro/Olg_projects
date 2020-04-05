using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1.Not_unity_scripts.new_scripts
{
    public class Save_message
    {
        string tag = "";
        string text = "";
        public Save_message(string tag, string text)
        {
            this.tag = tag;
            this.text = text;
        }
        public string get_tag()
        {
            return (tag);
        }
        public Save_message(string raw_save_message)
        {
            String[] tmp = raw_save_message.Split('|');
            text = "";
            if (tmp.Length > 1)
            {
                tag = tmp[0];
                for (int i = 1; i < tmp.Length; i++)
                {
                    text += tmp[i];
                    if (i != tmp.Length - 1)
                    {
                        text += "|";
                    }
                }
            }
        }
        public string full_text()
        {
            return ("{"+tag + "|" + text+"}");
        }
        public string get_text()
        {
            return (text);
        }
        public void set_text(string text)
        {
            this.text = text;
        }
    }
}
