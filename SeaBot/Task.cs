﻿// SeaBotCore
// Copyright (C) 2018 - 2019 Weespin
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SeaBotCore.BotMethods;
using SeaBotCore.Data;
using SeaBotCore.Data.Definitions;
using SeaBotCore.Utils;

namespace SeaBotCore
{
    public class Task
    {
        public interface IGameTask
        {
            string Action { get; }
            uint Time { get; }
            Dictionary<string, object> CustomObjects { get; }
        }

        public class TakeFish : IGameTask
        {
            public TakeFish(Boat boat)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                //calculate turns :thinking:
                var started = TimeUtils.FromUnixTime(boat.ProdStart);
                var b = Definitions.BoatDef.Items.Item.First(n => n.DefId == 1).Levels.Level
                    .First(n => n.Id == Core.GlobalData.BoatLevel);
                var turns = Math.Round((TimeUtils.FixedUTCTime - started).TotalSeconds / b.TurnTime);
                CustomObjects.Add("inst_id", boat.InstId);
                if (turns > b.TurnCount)
                {
                    CustomObjects.Add("turns", b.TurnCount);
                }
                else
                {
                    CustomObjects.Add("turns", turns);
                }

                Core.GlobalData.Boats.First(n => n.InstId == boat.InstId).ProdStart =
                    TimeUtils.GetEpochTime();
            }

            public string Action => "finish_boat_prod";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class ActivateEvent : IGameTask
        {
            public ActivateEvent(int def_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("def_id", def_id);
            }

            public string Action => "activate_event";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class ActivateItemsOfferTask : IGameTask
        {
            public ActivateItemsOfferTask(int def_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("def_id", def_id);
            }

            public string Action => "activate_items_offer";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class ActivateMerchantOfferTask : IGameTask
        {
            public ActivateMerchantOfferTask(int def_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("def_id", def_id);
            }

            public string Action => "activate_merchant_offer";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class ActivatePackTask : IGameTask
        {
            public ActivatePackTask(int def_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("def_id", def_id);
            }

            public string Action => "activate_pack";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class ActivateShipTask : IGameTask
        {
            public ActivateShipTask(int inst_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "activate_ship";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class AssignCaptainTask : IGameTask
        {
            public AssignCaptainTask(int ship_inst_id, int captain_inst_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("ship_inst_id", ship_inst_id);
                CustomObjects.Add("captain_inst_id", captain_inst_id);
            }

            public string Action => "assign_ship_captain";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class BuyBoatTask : IGameTask
        {
            public BuyBoatTask(int def_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("type", "boat");
                CustomObjects.Add("payment_type", "standard");
                CustomObjects.Add("def_id", def_id);
            }

            public string Action => "purchase";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class BuyBuildAreaTask : IGameTask
        {
            public BuyBuildAreaTask(int def_id, int x, int y, bool gem = false)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("type", "buildarea");
                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("x", x);
                CustomObjects.Add("y", y);
                CustomObjects.Add("payment_type", gem
                    ? "gem"
                    : "standard");
            }

            public string Action => "purchase";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class BuyBuildingTask : IGameTask
        {
            public BuyBuildingTask(int def_id, int x, int y, bool gem = false)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("type", "building");

                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("x", x);
                CustomObjects.Add("y", y);
                CustomObjects.Add("payment_type", gem
                    ? "gem"
                    : "standard");
            }

            public string Action => "purchase";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class BuyCaptainTask : IGameTask
        {
            public BuyCaptainTask(int def_id, int price_def_id, bool gem = false)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("type", "captain");

                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("price_def_id", price_def_id);
                CustomObjects.Add("payment_type", gem
                    ? "gem"
                    : "standard");
            }

            public string Action => "purchase";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class BuyItemsOfferTask : IGameTask
        {
            public BuyItemsOfferTask()
            {
                Time = (uint) TimeUtils.GetEpochTime();
                //todo reverse this
                throw new NotImplementedException();
            }

            public string Action => "purchase";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class BuyMoreSailorsTask : IGameTask
        {
            public BuyMoreSailorsTask(int amount)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("type", "sailors");

                CustomObjects.Add("amount", amount);
            }

            public string Action => "purchase";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class BuySailorsTask : IGameTask
        {
            public BuySailorsTask(int amount)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("type", "sailors");

                CustomObjects.Add("amount", amount);
            }

            public string Action => "purchase";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class BuyShipTask : IGameTask
        {
            public BuyShipTask(int def_id, bool gem = false)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("type", "ship");
                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("payment_type", gem
                    ? "gem"
                    : "standard");
            }

            public string Action => "purchase";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class BuyShipsOfferShipTask : IGameTask
        {
            public BuyShipsOfferShipTask(int def_id, string type, int itemid, bool gem = false)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("type", type);
                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("item_id", itemid);
                CustomObjects.Add("payment_type", gem
                    ? "gem"
                    : "standard");
            }

            public string Action => "purchase";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class CancelShipTask : IGameTask
        {
            public CancelShipTask(int inst_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "cancel_ship";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class ConfirmAchievementTask : IGameTask
        {
            public ConfirmAchievementTask(int def_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("def_id", def_id);
            }

            public string Action => "confirm_achievement";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }
        public class OutpostSendShipTask : IGameTask
        {
            public OutpostSendShipTask(int inst_id,int outpost_id,int crew)
            {
                Time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
                CustomObjects.Add("outpost_id", outpost_id);
                CustomObjects.Add("crew", crew);
                CustomObjects.Add("player_level", Core.GlobalData.Level);
            }

            public string Action => "send_ship_outpost";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }
        public class ConfirmBarrelTask : IGameTask
        {
            public ConfirmBarrelTask(int def_id, string type, int amount, int material_id, int player_lvl)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("material_id", material_id);
                CustomObjects.Add("amount", amount);
                CustomObjects.Add("player_level", player_lvl);
                CustomObjects.Add("type", type);
                CustomObjects.Add("def_id", def_id);
            }

            public string Action => "confirm_barrel";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class ConfirmContractTask : IGameTask
        {
            public ConfirmContractTask(int def_id,int quest_id,ContractorDefinitions.Rewards rewards)
            {
                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("quest_id", quest_id);
                Time = (uint) TimeUtils.GetEpochTime();
                if(rewards.Reward.Count>0)
                {
                   StringBuilder rew = new StringBuilder();
                foreach(var reward in rewards.Reward)
                    { 
                        rew.Append("\n<reward>");
                        rew.Append("\n<type>" + reward.Type+ "</type>");
                        rew.Append("\n<id>" + reward.Id + "</id>");
                        rew.Append("\n<amount>" + reward.Amount + "</amount>");
                        rew.Append("\n</reward>");
                    }
                    CustomObjects.Add("rewards", rew.ToString());
                }
            }

            public string Action => "confirm_contractor_quest";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class ConfirmGlobalContractorMilestoneTask : IGameTask
        {
            public ConfirmGlobalContractorMilestoneTask()
            {
                Time = (uint) TimeUtils.GetEpochTime();
                //todo reverse this
                throw new NotImplementedException();
            }

            public string Action => "confirm_contractor_quest";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class ConfirmGlobalContractorObjectiveTask : IGameTask
        {
            public ConfirmGlobalContractorObjectiveTask()
            {
                Time = (uint) TimeUtils.GetEpochTime();
                //todo reverse this
                throw new NotImplementedException();
            }

            public string Action => "confirm_global_contractor_quest";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class ConfirmMuseumTask : IGameTask
        {
            public ConfirmMuseumTask(int turns)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("turns", turns);
            }

            public string Action => "confirm_museum";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class ConfirmSocialContractRewardsTask : IGameTask
        {
            public ConfirmSocialContractRewardsTask(int inst_ids)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_ids", inst_ids);
            }

            public string Action => "confirm_social_contract_rewards";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class ConfirmSocialContractTask : IGameTask
        {
            public ConfirmSocialContractTask(int inst_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "confirm_social_contract";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class ConfirmUpgradeableTask : IGameTask
        {
            public ConfirmUpgradeableTask(int def_id, int player_lvl)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("player_level", player_lvl);
            }

            public string Action => "confirm_upgradeable";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class DeactivateShipTask : IGameTask
        {
            public DeactivateShipTask(int inst_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "deactivate_ship";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class DeleteAllPlayerMaterialsTask : IGameTask
        {
            private DeleteAllPlayerMaterialsTask()
            {
                Time = (uint) TimeUtils.GetEpochTime();
            }

            public string Action => "delete_all_player_materials";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class DockShipTaskOutPost : IGameTask
        {
            public DockShipTaskOutPost(Ship ship, bool usecapt, int ship_capacity, int ship_capacity_used, int sailors,
                int sailors_used, int debug_loc_lvl, int progress, int goalprogress,int uniqueid)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", ship.InstId);
                CustomObjects.Add("player_level", Core.GlobalData.Level);
                CustomObjects.Add("debug_capacity", ship_capacity);
                CustomObjects.Add("debug_capacity_used", ship_capacity_used);
                CustomObjects.Add("debug_sailors", sailors);
                CustomObjects.Add("debug_sailors_used", sailors_used);
                CustomObjects.Add("debug_location_level", debug_loc_lvl);
                if (usecapt)
                {
                    if (ship.CaptainId != 0)
                    {
                        CustomObjects.Add("debug_captain_id", ship.CaptainId);
                        CustomObjects.Add("debug_captain_def_id",
                            Core.GlobalData.CaptainsNew.Where(n => n.InstId == ship.CaptainId).FirstOrDefault()
                                ?.InstId);
                    }
                }

                CustomObjects.Add("debug_uniqueId",uniqueid);


                CustomObjects.Add("debug_progress", progress);
                CustomObjects.Add("debug_goalProgress", goalprogress);
            }


            public string Action
            {
                get => "dock_ship_outpost";
            }

            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class DockShipTaskGlobalContractor : IGameTask
        {
            public DockShipTaskGlobalContractor(Ship ship, bool usecapt, int ship_capacity, int ship_capacity_used,
                int sailors, int sailors_used, int debug_loc_lvl,int uniqueid)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", ship.InstId);
                CustomObjects.Add("player_level", Core.GlobalData.Level);
                CustomObjects.Add("debug_capacity", ship_capacity);
                CustomObjects.Add("debug_capacity_used", ship_capacity_used);
                CustomObjects.Add("debug_sailors", sailors);
                CustomObjects.Add("debug_sailors_used", sailors_used);
                CustomObjects.Add("debug_location_level", debug_loc_lvl);
                if (usecapt)
                {
                    if (ship.CaptainId != 0)
                    {
                        CustomObjects.Add("debug_captain_id", ship.CaptainId);
                        CustomObjects.Add("debug_captain_def_id",
                            Core.GlobalData.CaptainsNew.Where(n => n.InstId == ship.CaptainId).FirstOrDefault()
                                ?.InstId);
                    }
                }

                CustomObjects.Add("debug_uniqueId", uniqueid);
            }


            public string Action => "dock_ship_global_contractor";

            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class DockShipSocialContractor : IGameTask
        {
            public DockShipSocialContractor(Ship ship, bool usecapt, int ship_capacity, int ship_capacity_used,
                int sailors, int sailors_used, int debug_loc_lvl,int uniqueid)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", ship.InstId);
                CustomObjects.Add("player_level", Core.GlobalData.Level);
                CustomObjects.Add("debug_capacity", ship_capacity);
                CustomObjects.Add("debug_capacity_used", ship_capacity_used);
                CustomObjects.Add("debug_sailors", sailors);
                CustomObjects.Add("debug_sailors_used", sailors_used);
                CustomObjects.Add("debug_location_level", debug_loc_lvl);
                if (usecapt)
                {
                    if (ship.CaptainId != 0)
                    {
                        CustomObjects.Add("debug_captain_id", ship.CaptainId);
                        CustomObjects.Add("debug_captain_def_id",
                            Core.GlobalData.CaptainsNew.Where(n => n.InstId == ship.CaptainId).FirstOrDefault()
                                ?.InstId);
                    }
                }

                CustomObjects.Add("debug_uniqueId", uniqueid);
            }

            public string Action => "dock_ship_social_contract";

            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class DockShipTaskContractor : IGameTask
        {
            public DockShipTaskContractor(Ship ship,  bool usecapt, int ship_capacity,
                int ship_capacity_used, int sailors, int sailors_used, int debug_loc_lvl, int contractid, int progress,
                int goalprogress, string objectivetypeid,int uniqueid)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", ship.InstId);
                CustomObjects.Add("player_level", Core.GlobalData.Level);
                CustomObjects.Add("debug_capacity", ship_capacity);
                CustomObjects.Add("debug_capacity_used", ship_capacity_used);
                CustomObjects.Add("debug_sailors", sailors);
                CustomObjects.Add("debug_sailors_used", sailors_used);
                CustomObjects.Add("debug_location_level", debug_loc_lvl);
                if (usecapt)
                {
                    if (ship.CaptainId != 0)
                    {
                        CustomObjects.Add("debug_captain_id", ship.CaptainId);
                        CustomObjects.Add("debug_captain_def_id",
                            Core.GlobalData.CaptainsNew.Where(n => n.InstId == ship.CaptainId).FirstOrDefault()
                                ?.InstId);
                    }
                }

                CustomObjects.Add("debug_uniqueId", uniqueid);

                var contractor = Definitions.ConDef.Items.Item.Where(n => n.DefId == ship.TargetId);

                CustomObjects.Add("debug_contractId", contractid);
                CustomObjects.Add("debug_progress", progress);
                CustomObjects.Add("debug_goalProgress", goalprogress);
                CustomObjects.Add("debug_objectiveTypeId", objectivetypeid);
            }


            public string Action => "dock_ship_contractor";


            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class FinishBoatProducingTask : IGameTask
        {
            public FinishBoatProducingTask(int inst_id, int turns)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
                CustomObjects.Add("turns", turns);
            }

            public string Action => "finish_boat_prod";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }
      

        public class FinishBuildingProducingTask : IGameTask
        {
            public FinishBuildingProducingTask(int inst_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "finish_building_prod";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class FinishBuildingUpgradeTask : IGameTask
        {
            public FinishBuildingUpgradeTask(int inst_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "finish_building_upg";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class FinishPersonalGuideTask : IGameTask
        {
            public FinishPersonalGuideTask(int inst_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "finish_personal_guide_task";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class GiftConfirmTask : IGameTask
        {
            public GiftConfirmTask(int inst_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "confirm_gift";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class StartBuildingProducingTask : IGameTask
        {
            public StartBuildingProducingTask(int inst_id, int prod_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
                CustomObjects.Add("prod_id", prod_id);
            }

            public string Action => "start_building_prod";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class StartBuildingUpgradeTask : IGameTask
        {
            public StartBuildingUpgradeTask(int inst_id, int prod_id, int lvl, string debug_type,
                int debug_defId, int debug_tileX, int debug_tileY)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
                CustomObjects.Add("payment_type", "standard");
                CustomObjects.Add("level", lvl);
                CustomObjects.Add("debug_type", debug_type);
                CustomObjects.Add("debug_defId", debug_defId);
                CustomObjects.Add("debug_tileX", debug_tileX);
                CustomObjects.Add("debug_tileY", debug_tileY);
            }

            public string Action => "start_building_upg";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class SendShipMarketplaceTask : IGameTask
        {
            public SendShipMarketplaceTask(int inst_id, int def_id, int dest_id, int amount)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
                CustomObjects.Add("dest_id", dest_id);
                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("amount", amount);
            }

            public string Action => "send_ship_marketplace";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class UnloadShipTask : IGameTask
        {
            public UnloadShipTask(int inst_id, int player_level, Enums.EObject eobj, int debug_capacity,
                int debug_capacity_used, int debug_sailors, int debug_sailors_used, int debug_locationlevel,
                Captain cpt = null, int debug_uniqueid = 1)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("debug_capacity_used", debug_capacity_used);
                CustomObjects.Add("debug_sailors_used", debug_sailors_used);
                CustomObjects.Add("debug_capacity", debug_capacity);
                CustomObjects.Add("debug_uniqueid", debug_uniqueid);
                CustomObjects.Add("debug_sailors", debug_sailors);
                CustomObjects.Add("debug_location_level", debug_locationlevel);
                CustomObjects.Add("inst_id", inst_id);
                CustomObjects.Add("player_level", player_level);

                if (cpt != null)
                {
                    CustomObjects.Add("debug_captain_id", cpt.InstId);
                    CustomObjects.Add("debug_captain_def_id", cpt.DefId);
                }

                switch (eobj)
                {
                    case Enums.EObject.upgradeable:
                        Action = "unload_ship_upgradeable";
                        break;
                    case Enums.EObject.marketplace:
                        Action = "unload_ship_marketplace";
                        break;
                    case Enums.EObject.dealer:
                        Action = "unload_ship_dealer";
                        break;
                    case Enums.EObject.wreck:
                        Action = "unload_ship_wreck";
                        break;
                    case Enums.EObject.lost_treasure:
                        Action = "unload_ship_treasure";
                        break;
                }
            }

            public string Action { get; } = string.Empty;

            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class SendShipUpgradeableTask : IGameTask
        {
            public SendShipUpgradeableTask(Ship ship, Upgradeable destination, int amount)
            {
                var destination_levels = Definitions.UpgrDef.Items.Item.First(n => n.DefId == destination.DefId).Levels
                    .Level.FirstOrDefault(n => n.Id == destination.Level);
                Time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", ship.InstId);
                CustomObjects.Add("dest_id", destination.DefId);
                CustomObjects.Add("dest_amount", destination_levels.Amount);
                CustomObjects.Add("dest_material_koef",
                    destination_levels.MaterialKoef != 0 ? destination_levels.MaterialKoef : 1);
                CustomObjects.Add("dest_sailors", destination_levels.Sailors);
                CustomObjects.Add("amount", amount == 0 ? AutoShipUtils.GetCapacity(ship) : amount);
                CustomObjects.Add("player_level", Core.GlobalData.Level);
            }

            public string Action => "send_ship_upgradeable";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class LoadShipUpgradeableTask : IGameTask
        {
            public LoadShipUpgradeableTask(int inst_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "load_ship_upgradeable";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class RemoveMaterialTask : IGameTask
        {
            public RemoveMaterialTask(int def_id, int amount)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("amount", amount);
            }

            public string Action => "remove_material";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class ConfirmUpgradableTask : IGameTask
        {
            public ConfirmUpgradableTask(int def_id, int player_lvl)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("player_lvl", player_lvl);
            }

            public string Action => "confirm_upgradeable";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class UnloadShipContactorTask : IGameTask
        {
            public UnloadShipContactorTask(int inst_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "unload_ship_contractor";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }


        public class UnloadShipGlobalContractorTask : IGameTask
        {
            public UnloadShipGlobalContractorTask(int inst_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "unload_ship_global_contractor";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class UnloadShipOutpostTask : IGameTask
        {
            public UnloadShipOutpostTask(int inst_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "unload_ship_outpost";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class UnloadShipSocialContractTask : IGameTask
        {
            public UnloadShipSocialContractTask(int inst_id)
            {
                Time = (uint) TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "unload_ship_social_contract";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }

        public class HeartBeat : IGameTask
        {
            public HeartBeat()
            {
                Time = (uint) TimeUtils.GetEpochTime();
            }

            public string Action => "heartbeat";
            public uint Time { get; }

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
        }
    }
}