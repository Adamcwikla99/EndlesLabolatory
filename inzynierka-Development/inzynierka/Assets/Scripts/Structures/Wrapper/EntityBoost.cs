namespace Structures.Wrapper
{
    /// <summary>
    ///  data wraper for entity boost variables information
    /// </summary>
    public class EntityBoost
    {
        public int BonusMaxHP;
        public float BonusProjectilePower;
        public float BonusProjectileSpeed;

        public EntityBoost(int bonusMaxHP, float bonusProjectilePower, float bonusProjectileSpeed)
        {
            this.BonusMaxHP = bonusMaxHP;
            this.BonusProjectilePower = bonusProjectilePower;
            this.BonusProjectileSpeed = bonusProjectileSpeed;
        }
        
        public EntityBoost()
        {
            this.BonusMaxHP = 0;
            this.BonusProjectilePower = 0;
            this.BonusProjectileSpeed = 0;
        }

        public void CalculateNewBoost(int beatenFloors, float bonusMaxHPScale, float bonusProjectilePowerScale, float bonusProjectileSpeedScale)
        {
            this.BonusProjectilePower = beatenFloors * bonusProjectilePowerScale;
            this.BonusMaxHP = (int)(beatenFloors * bonusMaxHPScale);
            this.BonusProjectileSpeed = beatenFloors * bonusProjectileSpeedScale;
            
        }
        
    }
}
