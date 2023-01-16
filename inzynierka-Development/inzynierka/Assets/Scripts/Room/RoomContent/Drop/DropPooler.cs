using System;
using System.Collections.Generic;
using Events.Drop;
using Structures;
using Structures.Enums;
using Structures.Wrapper;
using UnityEngine;

namespace Room.RoomContent.Drop
{
    /// <summary>
    ///  Class that stores created and unused drop objects - implement object poll patern
    /// </summary>
    public class DropPooler : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private DropItem DropItemEvent { get; set; }

        [field: SerializeField]
        private List<DropPoolData> DropPoolDatas { get; set; }
        
        [field: SerializeField]
        private int IncriseCount { get; set; }

        [field: SerializeField]
        private ReturnDropToQue QueReturner { get; set; }
        
        [field: SerializeField]
        private Vector3 LiftUpDrop { get; set; } 

        #endregion
        #region variables

        private Dictionary<DropType, Queue<Drop>> poolDrop = new Dictionary<DropType, Queue<Drop>>();

        #endregion
        #region unityCallbacks

        private void OnEnable()
        {
            this.DropItemEvent.DropLoot += this.GenerateDrop;
            this.QueReturner.ReturnToDropQue += this.ReturnToQue;
        }

        private void OnDisable()
        {
            this.DropItemEvent.DropLoot -= this.GenerateDrop;
            this.QueReturner.ReturnToDropQue -= this.ReturnToQue;
        }

        private void Start()
        {
            this.InitDictionary();
        }
        
        #endregion
        #region methods

        /// <summary>
        /// methode that drops gameobject if he is lucky 
        /// </summary>
        private void GenerateDrop(Vector3 position)
        {
            if (this.AllowDrop() == true)
            {
                return;
            }
            
            this.DropReward(position + this.LiftUpDrop);
        }

        /// <summary>
        /// methode that determins if player should be rewarded 
        /// </summary>
        private bool AllowDrop()
        {
            return Tools.GetRandomNumberFromRange(0, 100) > 70;
        }
        
        /// <summary>
        ///  methode that spawns drop object in location of defeated enemy
        /// </summary>
        private void DropReward(Vector3 position)
        {
            int randomNumber = Tools.GetRandomNumberFromRange(0, Enum.GetNames(typeof(DropType)).Length-1);
            DropType toDrop = (DropType)randomNumber;

            if (this.poolDrop[toDrop].Count >0)
            {
                this.poolDrop[toDrop].Dequeue().InitDrop(position);
                return;
            }

            this.AddNewElementsToQue(toDrop);
            this.poolDrop[toDrop].Dequeue().InitDrop(position);
        }

        /// <summary>
        /// methode that adds new drop gameobject to the drop pool
        /// </summary>
        private void AddNewElementsToQue(DropType toDrop)
        {
            Drop toSpawn = this.DropPoolDatas.Find(x => x.Type == toDrop).Prefab;
            for (int i=0; i<this.IncriseCount;i++)
            {
                Drop newProjectile = Instantiate(toSpawn, this.transform);
                this.poolDrop[toDrop].Enqueue(newProjectile);     
            }
        }
        
        /// <summary>
        /// methode that returns drop obejct to pool
        /// </summary>
        private void ReturnToQue(DropType type, Drop dropObject)
        {
            this.poolDrop[type].Enqueue(dropObject);
        }
        
        /// <summary>
        /// methode that initialized drop poll dictionary
        /// </summary>
        private void InitDictionary()
        {
            foreach (DropPoolData projectilePoolData in this.DropPoolDatas)
            {
                this.IndividualProjectileTypeSpawn(projectilePoolData);
            }   
        }
        
        /// <summary>
        /// methode that spawns projectile reward 
        /// </summary>
        private void IndividualProjectileTypeSpawn(DropPoolData projectilePoolData)
        {
            this.poolDrop.Add(projectilePoolData.Type, new Queue<Drop>());
            for (int i = 0; i < projectilePoolData.initialCount; i++)
            {
                Drop newProjectile = Instantiate(projectilePoolData.Prefab, this.transform).GetComponent<Drop>();
                this.poolDrop[projectilePoolData.Type].Enqueue(newProjectile);                        
            }     
        }
        
        #endregion
    }
}
