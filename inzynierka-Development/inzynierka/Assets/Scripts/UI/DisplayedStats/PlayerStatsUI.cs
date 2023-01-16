using System.Globalization;
using Events.UI;
using UnityEngine;

namespace UI.DisplayedStats
{
    /// <summary>
    ///  Class that implements player stats boosting logic
    /// </summary>
    public class PlayerStatsUI : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private TMPro.TMP_Text Arrows { get; set; }
        
        [field: SerializeField]
        private TMPro.TMP_Text Granades { get; set; }
        
        [field: SerializeField]
        private TMPro.TMP_Text Rckets { get; set; }
        
        [field: SerializeField]
        private TMPro.TMP_Text HP { get; set; }
        
        [field: SerializeField]
        private TMPro.TMP_Text MaxHP { get; set; }
        
        [field: SerializeField]
        private TMPro.TMP_Text Money { get; set; }
        
        [field: SerializeField]
        private DisplayedUIStats StatsChangeEvents { get; set; }

        #endregion

        #region variables

    

        #endregion

        #region unityCallbacks

        private void OnEnable()
        {
            this.EnableEvents();
        }
        
        private void OnDisable()
        {
            this.DisableEvents();
        }
 
        #endregion

        #region methods

        /// <summary>
        /// methode that sets initiality displayed player hp, max hp and money
        /// </summary>
        private void InitialDurabilitySetup(float hp, float maxHp, float money)
        {
            this.HP.text = hp.ToString(CultureInfo.CurrentCulture);
            this.MaxHP.text = maxHp.ToString(CultureInfo.CurrentCulture);
            this.Money.text = money.ToString(CultureInfo.CurrentCulture);
        }
        
        /// <summary>
        /// methode that sets displayed amunition counts
        /// </summary>
        private void InitialAmmunitionSetup(int arrows, int granades, int rockets)
        {
            this.Arrows.text = arrows.ToString();
            this.Granades.text = granades.ToString();
            this.Rckets.text = rockets.ToString();
        }

        #endregion
                
        /// <summary>
        /// methode that assigns actions methods
        /// </summary>
        private void EnableEvents()
        {
            this.StatsChangeEvents.InitialDurabilitySetup += this.InitialDurabilitySetup;
            this.StatsChangeEvents.InitialAmmunitionSetup += this.InitialAmmunitionSetup;
            this.StatsChangeEvents.SetArrows += this.SetArrows;
            this.StatsChangeEvents.SetGranades += this.SetGranades;
            this.StatsChangeEvents.SetRockets += this.SetRckets;
            this.StatsChangeEvents.Hp += this.SetHp;
            this.StatsChangeEvents.MaxHp += this.SetMaxHp;
            this.StatsChangeEvents.Money += this.SetMoney;
        }

        /// <summary>
        /// methode that unassigned actions methods
        /// </summary>
        private void DisableEvents()
        {
            this.StatsChangeEvents.InitialDurabilitySetup -= this.InitialDurabilitySetup;
            this.StatsChangeEvents.InitialAmmunitionSetup -= this.InitialAmmunitionSetup;
            this.StatsChangeEvents.SetArrows -= this.SetArrows;
            this.StatsChangeEvents.SetGranades -= this.SetGranades;
            this.StatsChangeEvents.SetRockets -= this.SetRckets;
            this.StatsChangeEvents.Hp -= this.SetHp;
            this.StatsChangeEvents.MaxHp -= this.SetMaxHp;
            this.StatsChangeEvents.Money -= this.SetMoney;
        }
        
        /// <summary>
        /// methode that sets displayed amount of arrows ammunition on UI 
        /// </summary>
        private void SetArrows(int value)
        {
            this.Arrows.text = value.ToString();            
        }

        /// <summary>
        /// methode that sets displayed amount of graneds ammunition on UI 
        /// </summary>
        private void SetGranades(int value)
        {
            this.Granades.text = value.ToString();                 
        }
        
        /// <summary>
        /// methode that sets displayed amount of rockets ammunition on UI 
        /// </summary>        
        private void SetRckets(int value)
        {
            this.Rckets.text = value.ToString();                 
        }
        
        /// <summary>
        /// methode that sets displayed player hp on UI 
        /// </summary>
        private void SetHp(float value)
        {
            this.HP.text = value.ToString("F2");                 
        }
        
        /// <summary>
        /// methode that sets displayed player max hp on UI 
        /// </summary>
        private void SetMaxHp(float value)
        {
            this.MaxHP.text = value.ToString("F2");     
        }
        
        /// <summary>
        /// methode that sets displayed player money count on UI 
        /// </summary>
        private void SetMoney(float value)
        {
            this.Money.text = value.ToString("F2");     
        }
        
    }
}
