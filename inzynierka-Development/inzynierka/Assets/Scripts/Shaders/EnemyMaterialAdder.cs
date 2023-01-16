using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shaders
{
    /// <summary>
    ///  Class that implements enemy shader material logic
    /// </summary>
    public class EnemyMaterialAdder : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        public bool DesolveAdded { get; set; } = false;

        [field: SerializeField]
        private MeshRenderer ObjectRenderer { get; set; }

        [field: SerializeField]
        private Material OutlineEffect { get; set; }
        
        [field: SerializeField]
        private Material DesolveEffect { get; set; }
            
        [field: SerializeField]
        private bool AddDesolve { get; set; } = false;  
        
        #endregion

        #region variables

        private Material InstanceOutlineEffect;
        private Material InstanceDesolveEffect;
        private static readonly int OutlineAlpha = Shader.PropertyToID("_Visibility");
        
        #endregion

        #region unityCallbacks

        private void Start()
        {
            this.Init();
        }

        // private void Update()
        // {
        //     // TODO: Remove object after X time
        //     if (AddDesolve == true)
        //     {
        //         this.AddDesolveMaterial();    
        //         return;
        //     }
        //     
        //     this.RemoveDesolveMaterial();
        // }

        #endregion

        #region methods

        /// <summary>
        /// methode that adds desolve material to game object 
        /// </summary>
        public void AddDesolveMaterial()
        {
            if (DesolveAdded == true)
            {
                return;
            }
            
            List<Material> materialsList = new List<Material>();
            materialsList = this.ObjectRenderer.materials.ToList();
            materialsList.Add(InstanceDesolveEffect);
            this.ObjectRenderer.materials = materialsList.ToArray();
            DesolveAdded = true;
        }

        /// <summary>
        /// methode that changes outline intensity 
        /// </summary>
        public void ChangeOutlineIntensity(float newValue)
        {
            InstanceOutlineEffect.SetFloat(OutlineAlpha, newValue);
        }
        
        /// <summary>
        /// methode that initializes material values and adds them to gameobject
        /// </summary>
        private void Init()
        {
            this.InstanceOutlineEffect = new Material(this.OutlineEffect);
            this.InstanceDesolveEffect = new Material(this.DesolveEffect);
            List<Material> materialsList = new List<Material>();
            materialsList = this.ObjectRenderer.materials.ToList();
            materialsList.Add(this.InstanceOutlineEffect);
            this.ObjectRenderer.materials = materialsList.ToArray();
        }
        
        /// <summary>
        /// methode that removes desoble material form gameobject
        /// </summary>
        private void RemoveDesolveMaterial()
        {
            if (DesolveAdded == false)
            {
                return;
            }
            
            List<Material> materialsList = new List<Material>();
            materialsList = this.ObjectRenderer.materials.ToList();
            materialsList.RemoveAt(materialsList.Count-1);
            this.ObjectRenderer.materials = materialsList.ToArray();
            DesolveAdded = false;
        }
        
        #endregion

    }
}
