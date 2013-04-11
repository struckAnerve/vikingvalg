using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vikingvalg
{
    public interface IPlaySound
    {
        IManageAudio _audioManager { get; set; }
        String Directory { get; set; }
    }
}
