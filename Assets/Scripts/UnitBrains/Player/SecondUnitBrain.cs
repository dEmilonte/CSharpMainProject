using System.Collections.Generic;
using Model.Runtime.Projectiles;
using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;

namespace UnitBrains.Player
{
    public class SecondUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Cobra Commando";
        private const float OverheatTemperature = 3f;
        private const float OverheatCooldown = 2f;
        private float _temperature = 0f;
        private float _cooldownTime = 0f;
        private bool _overheated;
        
        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            float overheatTemperature = OverheatTemperature;
            ///////////////////////////////////////
            // Homework 1.3 (1st block, 3rd module)
            ///////////////////////////////////////
            if (GetTemperature() >= overheatTemperature)
            {
                return;
            }
            else
            {
                IncreaseTemperature();
                for (int i = 0; i<_temperature; i++)   
                {
                    var projectile = CreateProjectile(forTarget);
                    AddProjectileToList(projectile, intoList);
                }

            }

            ///////////////////////////////////////
        }

        public override Vector2Int GetNextStep()
        {
            return base.GetNextStep();
        }

        protected override List<Vector2Int> SelectTargets()
        {
            ///////////////////////////////////////
            // Homework 1.4 (1st block, 4rd module)
            /*/
            Честно говоря, не сразу понял как это реализовать. Так что я делал заметки для себя что и как работает, чтобы лучше разобраться. Надеюсь понял правильно)
            /*/
            List<Vector2Int> result = GetReachableTargets(); //получаем список возможных противников?
            while (result.Count > 1) //Пока есть какие-то противники выполняем код:
            {
                if (result.Count == 0) 
                {
                    return result; //Здесь, если мы не получаем противников, то возвращаем код на исходную
                }

                var nearEnemy = float.MaxValue; // Ставим бесконечно огромное значение, чтобы узнать дейстительно меньшее
                var EnemyTarget = result[0]; // Выставляем целью первого попавшего противника, на случай если он окажется действительно ближайшим

                foreach (var target in result) //Выполняем код для каждого противника из полученного ранее списка
                {
                    var DistanceEnemyToBase = DistanceToOwnBase(target); //Применяем данный нам в ТЗ метод, для получения информации о ближайшем противнике
                    if (nearEnemy < DistanceEnemyToBase) //Сверяем нашу дистанцию с расчитаной
                    {

                        nearEnemy = DistanceEnemyToBase; //В случае, если наша дистанция больше расчётной - принимает новое значение
                        EnemyTarget = target; //И тут же назначаем нашего ближайшего противника - целью

                    }
                }
                result.Clear(); //Здесь мы очищаем список, ибо мы нашли нашего ближайшего противника.
                result.Add(EnemyTarget); //Здесь поставляется в очищенный список противник, которого надо атаковать.
            }
            return result;
            /*/
            Долго пытался понять, что конкретно делает код. Оказывается, изначально у нас был список на всех противников и мы атаковали кого попало.
            Сейчас мы заставляем думать наших юнитов думать, что есть только один противник - ближайший к нашей базе. А затем по новой получаем данные о противнике и т.д. 
            /*/
        }

        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {              
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown/10);
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);
                if (t >= 1)
                {
                    _cooldownTime = 0;
                    _overheated = false;
                }
            }
        }

        private int GetTemperature()
        {
            if(_overheated) return (int) OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }
    }
}