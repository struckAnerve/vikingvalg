using System;

namespace Vikingvalg
{
    /// <summary>
    /// Interfacet til komponenten som tar seg av spilltilstander
    /// </summary>
    interface IManageStates
    {
        /// <summary>
        /// Navnet på spilltilstanden
        /// </summary>
        String GameState { get; }
        /// <summary>
        /// Kalles på for å endre spilltilstanden
        /// </summary>
        /// <param name="changeTo">Navnet på spilltilstanden du vil endre til</param>
        void ChangeState(String changeTo);
    }
}
