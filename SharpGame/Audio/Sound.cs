using OpenTK.Audio.OpenAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Audio
{
    public class Sound
    {
        private readonly int id;
        private readonly int buffer;

        public Sound()
        {
            id = AL.GenBuffer();
        }
    }
}
