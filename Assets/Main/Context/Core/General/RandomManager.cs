using System;
using System.Collections.Generic;
using Main.Infrastructure.Context;
using Main.Infrastructure.Utils.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Context.Core.General
{
    public sealed class RandomManager : ICoreContextUnit
    {
        public void Bind()
        {
            Random.InitState(DateTime.Now.Millisecond);
        }
        
        public void OnActivateScene()
        {
        }
        
        public int Next(int max)
        {
            return Random.Range(0, max);
        }

        public int Next(int min, int max)
        {
            return Random.Range(min, max);
        }
        
        public bool NextBool()
        {
            return Random.Range(0, 2) == 0;
        }
        
        public float Next(float min, float max)
        {
            return Random.Range(min, max);
        }

        public T RandomFromList<T>(List<T> list)
        {
            var nextIndex = Next(0, list.Count);
            return list[nextIndex];
        }
        
        public T RandomFromArray<T>(T[] array)
        {
            var nextIndex = Next(0, array.Length);
            return array[nextIndex];
        }
        
        public float SymmetricNext(float min, float max)
        {
            var value = Random.Range(min, max);
            return NextBool() ? value : -value;
        }

        public Vector3 RandomOnUnitSphere()
        {
            return Random.onUnitSphere;
        }
        
        public Vector3 RandomInsideUnitSphere()
        {
            return Random.insideUnitSphere;
        }

        public void ShuffleList<T>(List<T> list)
        {
            list.Shuffle(this);
        }

        public int NextWithSeed(int seed, int max)
        {
            var oldState = Random.state;
            Random.InitState(seed);
            var randomNumber = Next(max);
            Random.state = oldState;
            return randomNumber;
        }
    }
}
