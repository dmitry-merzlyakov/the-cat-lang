using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace TheCat.Infrastructure
{
    public class CompositeParams
    {
        public static CompositeParams Create(string key, string value)
        {
            CompositeParams parms = new CompositeParams();
            parms.AddParam(key, value);
            return parms;
        }

        public CompositeParams()
        {
            CalculateHashCode();
        }

        public string this[string key]
        {
            get { return Parameters[key]; }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private void AddParam(string key, string value)
        {
            Parameters[key] = value;
            CalculateHashCode();
        }

        private void CalculateHashCode()
        {
            hashCode = base.GetHashCode();
            foreach (KeyValuePair<string, string> pair in Parameters)
            {
                hashCode ^= pair.Key.GetHashCode();
                if (pair.Value != null)
                    hashCode ^= pair.Value.GetHashCode();
            }
        }

        private int hashCode;
        private readonly IDictionary<string, string> Parameters = new Dictionary<string, string>();
    }
}
