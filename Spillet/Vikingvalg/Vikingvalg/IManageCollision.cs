namespace Vikingvalg
{
    interface IManageCollision
    {
        /// <summary>
        /// Legg til et element i listene over elementer som kan kollidere dersom listene ikke allerede inneholder elementet
        /// </summary>
        /// <param name="canCollide">Elementet du vil legge inn</param>
        void AddCollidable(ICanCollide canCollide);

        /// <summary>
        /// Fjern et element i listene over elementer som kan kollidere
        /// </summary>
        /// <param name="toRemove"></param>
        void RemoveCollidable(ICanCollide toRemove);

        /// <summary>
        /// Tømmer kollisjonssjekklistene
        /// </summary>
        void ClearCollisionList();

        /// <summary>
        /// Kall på denne kompoentens Update-funksjon
        /// </summary>
        void Enable();

        /// <summary>
        /// SLutt å kalle på denne komponentens Update-funksjon
        /// </summary>
        void Disable();
    }
}
