using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vikingvalg
{
    abstract class InGameLevel
    {
        InGameManager inGameService;

        private Player _player1;

        public InGameLevel(InGameManager inGameService, Player player1)
        {
            this.inGameService = inGameService;
            _player1 = player1;
        }

        public abstract void InitializeLevel();
    }
}
