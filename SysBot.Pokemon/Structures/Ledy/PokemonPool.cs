﻿using System.Collections.Generic;
using System.IO;
using PKHeX.Core;
using SysBot.Base;

namespace SysBot.Pokemon
{
    public class PokemonPool<T> : List<T> where T : PKM, new()
    {
        public readonly int ExpectedSize = new T().Data.Length;

        public readonly IPoolSettings Settings;

        public PokemonPool(IPoolSettings settings)
        {
            Settings = settings;
        }

        public bool Randomized => Settings.DistributeShuffled;

        private int Counter;

        public T GetRandomPoke()
        {
            var choice = this[Counter];
            Counter = (Counter + 1) % Count;
            if (Counter == 0 && Randomized)
                Util.Shuffle(this);
            return choice;
        }

        public T GetRandomSurprise()
        {
            int ctr = 0;
            while (true)
            {
                var rand = GetRandomPoke();
                if (rand is PK8 pk8 && DisallowSurpriseTrade(pk8))
                    continue;

                ctr++; // if the pool has no valid matches, yield out eventually
                if (ctr > Count * 2)
                    return rand;
            }
        }

        public bool Reload()
        {
            return LoadFolder(Settings.DistributeFolder);
        }

        public readonly Dictionary<string, LedyRequest<T>> Files = new Dictionary<string, LedyRequest<T>>();

        public bool LoadFolder(string path)
        {
            Clear();
            Files.Clear();
            if (!Directory.Exists(path))
                return false;

            var loadedAny = false;
            var files = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories);
            var matchFiles = LoadUtil.GetFilesOfSize(files, ExpectedSize);

            int surpriseBlocked = 0;
            foreach (var file in matchFiles)
            {
                var data = File.ReadAllBytes(file);
                var pkm = PKMConverter.GetPKMfromBytes(data);
                if (!(pkm is T dest))
                    continue;

                if (dest.Species == 0 || !new LegalityAnalysis(dest).Valid || !(dest is PK8 pk8))
                {
                    LogUtil.LogInfo("SKIPPED: Provided pk8 is not valid: " + dest.FileName, nameof(PokemonPool<T>));
                    continue;
                }

                if (DisallowSurpriseTrade(pk8))
                {
                    LogUtil.LogInfo("Provided pk8 has a special ribbon and can't be Surprise Traded: " + dest.FileName, nameof(PokemonPool<T>));
                    surpriseBlocked++;
                }

                if (Settings.ResetHOMETracker)
                    pk8.Tracker = 0;

                Add(dest);
                var fn = Path.GetFileNameWithoutExtension(file);
                Files.Add(fn, new LedyRequest<T>(dest, fn));
                loadedAny = true;
            }

            if (surpriseBlocked == Count)
                LogUtil.LogInfo("Surprise trading will fail; failed to load any compatible files.", nameof(PokemonPool<T>));

            return loadedAny;
        }

        private static bool DisallowSurpriseTrade(PK8 pk8)
        {
            return pk8.RibbonClassic || pk8.RibbonPremier || pk8.RibbonBirthday;
        }
    }
}