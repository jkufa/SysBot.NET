﻿using System;

namespace SysBot.Pokemon
{
    public static class PokeDataOffsets
    {
        public const uint BoxStartOffset = 0x4293D8B0;
        public const uint TrainerDataOffset = 0x42935E48;
        public const uint SoftBanUnixTimespanOffset = 0x4298f9f8;

        public const uint IsConnectedOffset = 0x2f865c78;

        /* Link Trade Offsets */
        public const uint LinkTradePartnerPokemonOffset = 0x2E32206A;
        public const uint LinkTradePartnerNameOffset = 0xAC84173C;

        /* Suprise Trade Offsets */
        public const uint SupriseTradePartnerPokemonOffset = 0x429344d0;

        public const uint SupriseTradeLockSlot = 0x4293462c;
        public const uint SupriseTradeLockBox = 0x42934628;

        public const uint SupriseTradeSearchOffset = 0x42934634;
        public const uint SupriseTradeSearch_Empty = 0x00000000;
        public const uint SupriseTradeSearch_Searching = 0x01000000;
        public const uint SupriseTradeSearch_Found = 0x0200012C;

        public const uint SupriseTradePartnerNameOffset = 0x42934638;

        /* Route 5 Daycare */
        //public const uint DayCareSlot_1_WildArea_Present = 0x429e4EA8;
        //public const uint DayCareSlot_2_WildArea_Present = 0x429e4ff1;

        //public const uint DayCareSlot_1_WildArea = 0x429E4EA9;
        //public const uint DayCareSlot_2_WildArea = 0x429e4ff2;

        //public const uint DayCare_WildArea_Unknown = 0x429e513a;

        public const uint DayCare_Wildarea_Step_Counter = 0x429e513c;

        //public const uint DayCare_WildArea_EggSeed = 0x429e5140;
        public const uint DayCare_Wildarea_Egg_Is_Ready = 0x429e5148;

        /* Wild Area Daycare */
        //public const uint DayCareSlot_1_Route5_Present = 0x429e4bf0;
        //public const uint DayCareSlot_2_Route5_Present = 0x429e4d39;

        //public const uint DayCareSlot_1_Route5 = 0x429e4bf1;
        //public const uint DayCareSlot_2_Route5 = 0x429e4d3a;

        //public const uint DayCare_Route5_Unknown = 0x429e4e82;

        public const uint DayCare_Route5_Step_Counter = 0x429e4e84;

        //public const uint DayCare_Route5_EggSeed = 0x429e4e88;
        public const uint DayCare_Route5_Egg_Is_Ready = 0x429e4e90;

        public const int BoxFormatSlotSize = 0x158;
        public const int TrainerDataLength = 0x110;

        #region ScreenDetection
        /*
         * Not used for now, might be used someday
        public const uint MenuOffset = 0x00000036;
        public const uint MenuOpen = 0x7DC80000;
        public const uint MenuClosed = 0xBBE00000;
        */

        public const uint CurrentScreenOffset = 0x68dcbc90;

        public const uint CurrentScreen_Overworld = 0xFFFF5127;

        public const uint CurrentScreen_Box = 0xFF00D59B;
        public const uint CurrentScreen_Box_WaitingForOffer = 0xC800B483;
        public const uint CurrentScreen_Box_ConfirmOffer = 0xFF00B483;

        public const uint CurrentScreen_Softbann = 0xFF000000;

        //public const uint CurrentScreen_YMenu = 0xFFFF7983;
        #endregion

        public static uint GetTrainerNameOffset(TradeMethod tradeMethod)
        {
            return tradeMethod switch
            {
                TradeMethod.LinkTrade => LinkTradePartnerNameOffset,
                TradeMethod.SupriseTrade => SupriseTradePartnerNameOffset,
                _ => throw new ArgumentException(nameof(tradeMethod)),
            };
        }

        public static uint GetDaycareStepCounterOffset(SwordShieldDaycare daycare)
        {
            return daycare switch
            {
                SwordShieldDaycare.WildArea => DayCare_Wildarea_Step_Counter,
                SwordShieldDaycare.Route5 => DayCare_Route5_Step_Counter,
                _ => throw new ArgumentException(nameof(daycare)),
            };
        }

        public static uint GetDaycareEggIsReadyOffset(SwordShieldDaycare daycare)
        {
            return daycare switch
            {
                SwordShieldDaycare.WildArea => DayCare_Wildarea_Egg_Is_Ready,
                SwordShieldDaycare.Route5 => DayCare_Route5_Egg_Is_Ready,
                _ => throw new ArgumentException(nameof(daycare)),
            };
        }
    }
}
