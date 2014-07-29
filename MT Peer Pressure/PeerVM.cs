using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MT_Peer_Pressure
{
    public class PeerVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string test
        {
            get
            {
                return "test";// p.ToString();
            }
            set
            {
                test = value;
            }
        }

        private PPTrackVM t;
        public PPTrackVM T { get { return t; } set { t = value; } }
        public Peer p { get; private set; }
        private String effector;
        public String Effector
        {
            get { return effector; }
            set
            {
                effector = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Effector"));
            }
        }

        private List<String> relations;
        public List<String> Relations
        {
            get { return relations; }
            set
            {
                relations = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Relations"));
            }
        }

        private String target;
        public String Target
        {
            get { return target; }
            set
            {
                target = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Target"));
            }
        }

        public PeerVM(Peer pin, PPTrackVM tin)
        {
            p = pin;
            t = tin;
            Effector = cleanName(pin.InputEffector.ToString());
            Target = cleanName(pin.Target.ToString());
            Relations = new List<string>();
            foreach (Relation r in pin.Relations)
            {
                Relations.Add(cleanName(r.ToString()));
            }
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("PeerVM"));
        }

        public void Update()
        {
            Effector = cleanName(p.InputEffector.ToString());
            Target = cleanName(p.Target.ToString());
            Relations = new List<string>();
            foreach (Relation r in p.Relations)
            {
                Relations.Add(cleanName(r.ToString()));
            }
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("PeerVM"));
        }

        //public override string ToString() { return p.ToString(); }
        private string cleanName(string sIn)
        {
            string sOut;

            string[] classes = sIn.Split('.');
            sOut = classes.Last();

            sOut = sOut.TrimEnd("Effector".ToCharArray());
            sOut = sOut.TrimEnd("Relation".ToCharArray());
            sOut = sOut.TrimEnd("Target".ToCharArray());

            return sOut;
        }
    }
}
