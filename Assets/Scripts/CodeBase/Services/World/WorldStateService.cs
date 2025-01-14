using System;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services.World
{
    public interface IWorldStateService : IService
    {
        event Action<int> CollectedChanged;
        event Action<float> FinishedDistanceChanged;
        void Collect(int coin);
        void UpdateFinishedDistance(float distance);
        void CleanData();
        void Load();
        LevelStaticData GetLevelData();
    }

    public class WorldStateService : IWorldStateService
    {
        private const string StaticDataPath = "Static Data/LevelData";
        
        private int _collected;
        private float _finishedDistance;

        public event Action<int> CollectedChanged;
        public event Action<float> FinishedDistanceChanged;

        private LevelStaticData _levelStaticData;
        
        public void Load()
        {
            _levelStaticData = Resources
                .Load<LevelStaticData>(StaticDataPath);
        }

        public LevelStaticData GetLevelData()
        {
            return _levelStaticData;
        }
        
        public void Collect(int coin)
        {
            _collected += coin;
            CollectedChanged?.Invoke(_collected);
        }

        public void UpdateFinishedDistance(float distance)
        {
            _finishedDistance = distance;
            FinishedDistanceChanged?.Invoke(_finishedDistance);
        }

        public void CleanData()
        {
            _collected = 0;
            _finishedDistance = 0;
        }
    }
}