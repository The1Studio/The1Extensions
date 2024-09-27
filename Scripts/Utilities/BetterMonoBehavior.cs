﻿#nullable enable
namespace TheOne.Extensions
{
    using UnityEngine;
    #if THEONE_UNITASK
    using System.Threading;
    #else
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    #endif

    public class BetterMonoBehavior : MonoBehaviour
    {
        #if THEONE_UNITASK
        private CancellationTokenSource? disableCts;

        public CancellationToken GetCancellationTokenOnDisable()
        {
            return (this.disableCts ??= new CancellationTokenSource()).Token;
        }

        protected virtual void OnDisable()
        {
            this.disableCts?.Cancel();
            this.disableCts?.Dispose();
            this.disableCts = null;
        }
        #else
        private readonly HashSet<IEnumerator> runningCoroutines = new HashSet<IEnumerator>();

        public new void StartCoroutine(IEnumerator coroutine)
        {
            if (!this.runningCoroutines.Add(coroutine)) throw new InvalidOperationException("Coroutine is already running");
            base.StartCoroutine(coroutine.Finally(() => this.runningCoroutines.Remove(coroutine)));
        }

        public new void StopCoroutine(IEnumerator coroutine)
        {
            if (!this.runningCoroutines.Remove(coroutine)) throw new InvalidOperationException("Coroutine is not running");
            base.StopCoroutine(coroutine);
            (coroutine as IDisposable)?.Dispose();
        }

        public IEnumerator GatherCoroutines(params IEnumerator[] coroutines)
        {
            try
            {
                var count = coroutines.Length;
                coroutines.ForEach(coroutine => this.StartCoroutine(coroutine.Then(() => --count)));
                yield return new WaitUntil(() => count is 0);
            }
            finally
            {
                coroutines.ForEach(this.StopCoroutine);
            }
        }

        public IEnumerator GatherCoroutines(IEnumerable<IEnumerator> coroutines)
        {
            return this.GatherCoroutines(coroutines.ToArray());
        }

        protected virtual void OnDisable()
        {
            this.runningCoroutines.SafeForEach(this.StopCoroutine);
        }
        #endif
    }
}