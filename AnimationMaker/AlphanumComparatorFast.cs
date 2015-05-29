using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationMaker
{
    public class AlphanumComparatorFast : IComparer
    {
        public int Compare(object x, object y)
        {
            string s1 = ((KeyValuePair<string, FrameData>)x).Key;
            string s2 = ((KeyValuePair<string, FrameData>)y).Key;

            string common = "";
            s1.Intersect(s2).All((t) => {
                if(s1.IndexOf(t) == s2.IndexOf(t) && !char.IsDigit(t))common += t;
                return true;
            });
            string a1 = s1.Replace(common, string.Empty);
            string a2 = s2.Replace(common, string.Empty);

            return (int.Parse(a1) - int.Parse(a2));
        }
    }
}
