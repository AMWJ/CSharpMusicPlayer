using System;
using System.Collections.Generic;
using System.Text;

namespace Music
{
    public interface IPlayer
    {
        void Play(double offset, Note note);
    }
}