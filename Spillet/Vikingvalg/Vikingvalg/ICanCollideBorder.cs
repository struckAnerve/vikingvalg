﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Vikingvalg
{
    public interface ICanCollideBorder
    {
        Rectangle FootBox { get; }
        bool BlockedLeft { get; set; }
        bool BlockedRight { get; set; }
        bool BlockedTop { get; set; }
        bool BlockedBottom { get; set; }
    }
}