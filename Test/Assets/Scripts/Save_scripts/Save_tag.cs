using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1.Not_unity_scripts.new_scripts
{
	public class Save_tag
	{
        List<string> tags = new List<string>();
        public Save_tag(string tag)
        {
            tags.Add(tag);

        }
        public Save_tag copy()
        {
            Save_tag st = new Save_tag(tags[0]);
            for (int i = 1; i < tags.Count; i++)
            {
                st.add_tag(tags[i]);
            }
            return (st);
        }
        public int tags_count()
        {
            return (tags.Count);
        }
        public string get_tag(int n)
        {
            string t = "";
            if (n >= 0 && n < tags.Count)
            {
                t = tags[n];
            }
            return (t);
        }
        public void add_tag(string tag)
        {
            tags.Add(tag);
        }
        public string full_name()
        {
            string k = get_tag(0);
            for (int i = 1; i < tags_count(); i++)
            {
                k += " --- " + get_tag(i);
            }
            return (k);
        }
	}
}
