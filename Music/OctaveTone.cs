﻿using System;

namespace Music
{
    public class OctaveTone
    {
        OctaveTone Base;
        private string name;
        int Modification;
        public string Name
        {
            get
            {
                if (Base == null)
                {
                    return name;
                }
                else
                {
                    return Base.name + new string(Modification > 0 ? '#' : '♭', Math.Abs(Modification));
                }
            }
        }
        public OctaveTone(string Name)
        {
            this.name = Name;
        }
        public OctaveTone(OctaveTone Base, int Modification)
        {
            this.Base = Base;
            this.Modification = Modification;
        }
        public override string ToString()
        {
            return Name;
        }
        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            OctaveTone tone = (OctaveTone)obj;
            if (Base != null)
            {
                return Base.Equals(tone.Base) && Modification == tone.Modification;
            }
            return name == tone.name;
        }
        public override int GetHashCode()
        {
            if (Base != null) {
                return Base.GetHashCode() * 43 + Modification * 13 + 1;
            }
            return Name.GetHashCode();
        }
    }
}