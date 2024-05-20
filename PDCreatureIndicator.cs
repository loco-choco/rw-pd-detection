using HUD;
using MoreSlugcats;
using RWCustom;
using UnityEngine;
using System;
namespace PDDetection 
{
    public class PDCreatureIndicator : HudPart
    {
	public float warning_start_porcentage = 0.05f;
	public float warning_cutoff_porcentage = 0.5f;
        public float danger_cutoff_porcentage = 0.9f;

	private int max_number_of_indicators = 20;
	public HUDPDThreatLevelIndicator[] creatures_level_indicator;

        public Player hudPlayer => hud.owner as Player;
        public RoomCamera cam;
	public float threat_reference = 0.4f; //1.1f is the default
	public float indicator_radius = 55f;

	public PDCreatureIndicator(HUD.HUD hud, FContainer fContainer, RoomCamera cam) : base(hud){
	    creatures_level_indicator = new HUDPDThreatLevelIndicator[max_number_of_indicators];
	    for(int i = 0 ; i < max_number_of_indicators ; i++) 
		    creatures_level_indicator[i] = new HUDPDThreatLevelIndicator("pddetection-warning", "pddetection-danger", fContainer);
	    this.cam = cam;
	}

	public override void ClearSprites(){
	    for(int i = 0 ; i < max_number_of_indicators ; i++) 
		creatures_level_indicator[i].ClearSprite();
	}
	
	int amount_of_indicators_to_draw = 0;
	public override void Update(){
	    Vector2 player_pos = Vector2.zero;
	    amount_of_indicators_to_draw = 0;

	    bool show_hud = false;

	    if(hudPlayer != null){
	        player_pos = hudPlayer.mainBodyChunk.pos - cam.pos;
		show_hud = !hudPlayer.dead && hudPlayer.room != null && !hudPlayer.inShortcut;
	    }

	    for (int i = 0 ; i < max_number_of_indicators; i++){
                creatures_level_indicator[i].is_visible = show_hud;
	    }

            if(hudPlayer.room == null) return;

	    for (int i = 0; i < hudPlayer.room.abstractRoom.creatures.Count; i++){
	        var abscreature = hudPlayer.room.abstractRoom.creatures[i];
	        if (abscreature.realizedCreature != null && abscreature.realizedCreature != hudPlayer){
		  var creature = abscreature.realizedCreature;

		  TypeOfDangerCreatureIsToPlayer(creature, out bool is_creature_dangerous, out bool is_player_in_danger);

		  if(amount_of_indicators_to_draw < max_number_of_indicators && is_creature_dangerous){
		      var indicator = creatures_level_indicator[amount_of_indicators_to_draw];
		      indicator.Update();
		      indicator.is_in_danger = is_player_in_danger;
		      indicator.is_visible = show_hud;
		      indicator.pos = creature.mainBodyChunk.pos - cam.pos;
		      indicator.display_direction = false;
		      if(indicator.pos.x < 0 || indicator.pos.x > hud.rainWorld.options.ScreenSize.x
			|| indicator.pos.y < 0 || indicator.pos.y > hud.rainWorld.options.ScreenSize.y){
			indicator.pos = (indicator.pos - player_pos).normalized * indicator_radius + player_pos;
			indicator.dir = Custom.VecToDeg(indicator.pos - player_pos);
			indicator.display_direction = true;
		      }
		      amount_of_indicators_to_draw++;
		    }		  
		}
	    }
	    for (int i = amount_of_indicators_to_draw ; i < max_number_of_indicators; i++){
                creatures_level_indicator[i].is_visible = false;
	    }
	}
        public void TypeOfDangerCreatureIsToPlayer(Creature creature, out bool is_creature_dangerous, out bool is_player_in_danger){
	    is_creature_dangerous = false; 
	    is_player_in_danger = false;
	    if(creature.Template.dangerousToPlayer == 0f || creature.dead) return;

	    var abs_creature_ai = creature.abstractCreature.abstractAI;
	    if (abs_creature_ai == null || abs_creature_ai.RealAI == null) return;

	    var tracker = abs_creature_ai.RealAI.tracker;
	    if(tracker == null) return;

	    var threatTracker = abs_creature_ai.RealAI.threatTracker;

	    var preyTracker = abs_creature_ai.RealAI.preyTracker;

	    var noise_tracker = abs_creature_ai.RealAI.noiseTracker;
	    if(noise_tracker != null){
	       is_creature_dangerous = noise_tracker.soundToExamine != null 
			  || noise_tracker.mysteriousNoiseCounter > 0;
	    }
	    float highest_priority = 0f;
	    float player_priority = 0f;
	    bool is_player_wanted_dead = false;
	    /*
	    for (int i = 0; i < tracker.CreaturesCount; i++){
		var creature_rep = tracker.GetRep(i);
		if(highest_priority <= creature_rep.priority){ 
		    highest_priority = creature_rep.priority;
		}

	  	if (creature_rep.representedCreature == hudPlayer.abstractCreature){
		    player_priority = creature_rep.priority;
		    is_player_wanted_dead = creature_rep.dynamicRelationship.currentRelationship.GoForKill;
		}
	    }*/
	    //is_player_in_danger = is_player_wanted_dead 
		    //&& ((player_priority > 0.01f) ? player_priority >= highest_priority : false);
	    if(threatTracker != null && threatTracker.mostThreateningCreature!=null){
	    /*for (int i = 0; i < threatTracker.threatCreatures.Count; i++){
		var creature_rep = threatTracker.threatCreatures[i].creature;
	  	if (creature_rep.representedCreature == hudPlayer.abstractCreature){
		    is_player_in_danger = true;
		}
	    }*/
	    is_player_in_danger = (threatTracker.mostThreateningCreature.representedCreature == hudPlayer.abstractCreature);
	    }
	    
	    if(preyTracker != null && preyTracker.currentPrey!= null){
	    /*for (int i = 0; i < preyTracker.TotalTrackedPrey; i++){
		var creature_rep = preyTracker.prey[i].critRep;
	  	if (creature_rep.representedCreature == hudPlayer.abstractCreature){
		    is_player_in_danger = true;
		}
	    }*/
	    is_player_in_danger = (preyTracker.currentPrey.critRep.representedCreature == hudPlayer.abstractCreature);
	    }
	    is_creature_dangerous |= is_player_in_danger;
	}

	public override void Draw(float timeStacker){
	    for(int i = 0; i < max_number_of_indicators; i++)
	        creatures_level_indicator[i].Draw(timeStacker);
	}
    }
}
