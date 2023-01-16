namespace Interface
{
    /// <summary>
    ///  Interface containing damage receiving methode
    /// </summary>
    public interface IDamage 
    {
        #region methods

        public abstract void TakeDamage(float damageValue);
        
        public abstract bool DeflectProjectile();

        #endregion
    }
}
