﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music;

namespace MusicParser
{
    public interface ISongParser
    {
		Song Parse(String representation);
    }
}
