﻿using System;
using System.IO;
using PKHeX.Core;
using SysBot.Base;

namespace SysBot.Pokemon
{
    public class PokeTradeDetail<TPoke> : IEquatable<PokeTradeDetail<TPoke>> where TPoke : PKM, new()
    {
        public int Code;
        public readonly TPoke TradeData;
        public readonly PokeTradeTrainerInfo Trainer;
        public readonly IPokeTradeNotifier<TPoke> Notifier;
        public readonly PokeTradeType Type;
        public readonly bool IsSynchronized;

        private const int RandomCode = -1;
        public bool IsRandomCode => Code == RandomCode;

        public string? SourcePath { get; set; }
        public string? DestinationPath { get; set; }

        public PokeTradeDetail(TPoke pkm, PokeTradeTrainerInfo info, IPokeTradeNotifier<TPoke> notifier, PokeTradeType type, int code = RandomCode)
        {
            Code = code;
            TradeData = pkm;
            Trainer = info;
            Notifier = notifier;
            Type = type;
            IsSynchronized = IsRandomCode;
        }

        public void TradeInitialize(PokeRoutineExecutor routine) => Notifier.TradeInitialize(routine, this);
        public void TradeSearching(PokeRoutineExecutor routine) => Notifier.TradeSearching(routine, this);
        public void TradeCanceled(PokeRoutineExecutor routine, PokeTradeResult msg) => Notifier.TradeCanceled(routine, this, msg);

        public void TradeFinished(PokeRoutineExecutor routine, TPoke result)
        {
            Notifier.TradeFinished(routine, this, result);
            RelocateProcessedFile(routine);
        }

        public void SendNotification(PokeRoutineExecutor routine, string message) => Notifier.SendNotification(routine, this, message);

        private void RelocateProcessedFile(PokeRoutineExecutor completedBy)
        {
            if (SourcePath == null || !Directory.Exists(Path.GetDirectoryName(SourcePath)) || !File.Exists(SourcePath))
                return;
            if (DestinationPath == null || !Directory.Exists(Path.GetDirectoryName(DestinationPath)))
                return;

            if (File.Exists(DestinationPath))
                File.Delete(DestinationPath);
            File.Move(SourcePath, DestinationPath);
            LogUtil.LogInfo("Moved processed trade to destination folder.", completedBy.Connection.Name);
        }

        public bool Equals(PokeTradeDetail<TPoke>? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return ReferenceEquals(Trainer, other.Trainer);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PokeTradeDetail<TPoke>)obj);
        }

        public override int GetHashCode() => Trainer.GetHashCode();
        public override string ToString() => $"{Trainer.TrainerName} - {Code}";

        public string Summary(int i)
        {
            if (TradeData.Species == 0)
                return $"{i:00}: {Trainer.TrainerName}";
            return $"{i:00}: {Trainer.TrainerName}, {(Species)TradeData.Species}";
        }
    }
}