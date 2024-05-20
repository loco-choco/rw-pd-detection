using HUD;
using MoreSlugcats;
using RWCustom;
using UnityEngine;
using System;
namespace PDDetection 
{
    public class PDThreatIndicator : HudPart
    {
        public HUDCircle warning_circle_left;
        public HUDCircle warning_circle_right;
	public float warning_start_porcentage = 0.05f;
	public float warning_cutoff_porcentage = 0.5f;
	
        public HUDCircle danger_circle_left;	
        public HUDCircle danger_circle_right;	
        public float danger_cutoff_porcentage = 0.9f;

	public HUDPDThreatLevelIndicator warning_level_indicator;
	public HUDPDThreatLevelIndicator danger_level_indicator;

        public Player hudPlayer => hud.owner as Player;
        public RoomCamera cam;
	public float threat_reference = 0.4f; //1.1f is the default
	public float indicator_radius = 40f;

	public PDThreatIndicator(HUD.HUD hud, FContainer fContainer, RoomCamera cam) : base(hud){
	    //Danger Circle
	    danger_circle_right = new HUDCircle(hud, HUDCircle.SnapToGraphic.smallEmptyCircle, fContainer, 1);
	    danger_circle_right.fade = 1f;
	    danger_circle_right.lastFade = 1f;
	    danger_circle_right.circleShader = hud.rainWorld.Shaders["HoldButtonCircle"];
	    danger_circle_right.basicShader = hud.rainWorld.Shaders["Basic"];
	    danger_circle_right.sprite.shader = danger_circle_right.circleShader;
	    danger_circle_right.forceColor = new Color(250/255f, 79/255f, 29/255f);

	    danger_circle_left = new HUDCircle(hud, HUDCircle.SnapToGraphic.smallEmptyCircle, fContainer, 1);
	    danger_circle_left.fade = 1f;
	    danger_circle_left.lastFade = 1f;
	    danger_circle_left.circleShader = hud.rainWorld.Shaders["HoldButtonCircle"];
	    danger_circle_left.basicShader = hud.rainWorld.Shaders["Basic"];
	    danger_circle_left.sprite.shader = danger_circle_left.circleShader;
	    danger_circle_left.forceColor = new Color(250/255f, 79/255f, 29/255f);

            // Warning Circle
	    warning_circle_right = new HUDCircle(hud, HUDCircle.SnapToGraphic.smallEmptyCircle, fContainer, 1);
	    warning_circle_right.fade = 1f;
	    warning_circle_right.lastFade = 1f;
	    warning_circle_right.circleShader = hud.rainWorld.Shaders["HoldButtonCircle"];
	    warning_circle_right.basicShader = hud.rainWorld.Shaders["Basic"];
	    warning_circle_right.sprite.shader = warning_circle_right.circleShader;
	    warning_circle_right.forceColor = new Color(19/255f, 132/255f, 182/255f);

	    warning_circle_left = new HUDCircle(hud, HUDCircle.SnapToGraphic.smallEmptyCircle, fContainer, 1);
	    warning_circle_left.fade = 1f;
	    warning_circle_left.lastFade = 1f;
	    warning_circle_left.circleShader = hud.rainWorld.Shaders["HoldButtonCircle"];
	    warning_circle_left.basicShader = hud.rainWorld.Shaders["Basic"];
	    warning_circle_left.sprite.shader = warning_circle_left.circleShader;
	    warning_circle_left.forceColor = new Color(19/255f, 132/255f, 182/255f);

	    warning_level_indicator = new HUDPDThreatLevelIndicator("pddetection-warning", "pddetection-danger", fContainer);
	    danger_level_indicator = new HUDPDThreatLevelIndicator("pddetection-warning", "pddetection-danger", fContainer);

	    this.cam = cam;
	} 

	public override void Update(){
	    float threat = getThreatLevel();
	    float threat_porcentage = Mathf.InverseLerp(0f, threat_reference, threat) - warning_start_porcentage;

	    Vector2 pos = Vector2.zero;
	    bool show_hud = false;
	    if(hudPlayer != null){
	        pos = hudPlayer.mainBodyChunk.pos - cam.pos;
		show_hud = !hudPlayer.dead && !hudPlayer.inShortcut;
	    }

	    //Danger Circle
	    float danger_threat_porcentage = Mathf.Clamp(threat_porcentage, 0f, danger_cutoff_porcentage);
	    danger_circle_left.Update();
	    danger_circle_left.snapGraphic = HUDCircle.SnapToGraphic.smallEmptyCircle;
	    danger_circle_left.snapRad = 0.45f;
	    danger_circle_left.snapThickness = 0.45f;
	    danger_circle_left.rad = indicator_radius;
	    danger_circle_left.thickness = danger_threat_porcentage * indicator_radius / 2;
	    danger_circle_left.pos = pos;
	    danger_circle_left.sprite.rotation = 180f + 180f * warning_start_porcentage;
	    danger_circle_left.visible = show_hud;

	    danger_circle_right.Update();
	    danger_circle_right.snapGraphic = HUDCircle.SnapToGraphic.smallEmptyCircle;
	    danger_circle_right.snapRad = 0.45f;
	    danger_circle_right.snapThickness = 0.45f;
	    danger_circle_right.rad = indicator_radius;
	    danger_circle_right.thickness = danger_threat_porcentage * indicator_radius / 2;
	    danger_circle_right.pos = pos;
	    danger_circle_right.sprite.rotation = 180f - 360f * danger_threat_porcentage / 2 - 180f * warning_start_porcentage;
	    danger_circle_right.visible = show_hud;

	    //Warning Circle
	    float warn_threat_porcentage = Mathf.Clamp(threat_porcentage, 0f, warning_cutoff_porcentage);
	    warning_circle_left.Update();
	    warning_circle_left.snapGraphic = HUDCircle.SnapToGraphic.smallEmptyCircle;
	    warning_circle_left.snapRad = 0.45f;
	    warning_circle_left.snapThickness = 0.45f;
	    warning_circle_left.rad = indicator_radius;
	    warning_circle_left.thickness = warn_threat_porcentage * indicator_radius / 2;
	    warning_circle_left.pos = pos;
	    warning_circle_left.sprite.rotation = 180f + 180f * warning_start_porcentage;
	    warning_circle_left.visible = show_hud;

	    warning_circle_right.Update();
	    warning_circle_right.snapGraphic = HUDCircle.SnapToGraphic.smallEmptyCircle;
	    warning_circle_right.snapRad = 0.45f;
	    warning_circle_right.snapThickness = 0.45f;
	    warning_circle_right.rad = indicator_radius;
	    warning_circle_right.thickness = warn_threat_porcentage * indicator_radius / 2;
	    warning_circle_right.pos = pos;
	    warning_circle_right.sprite.rotation = 180f - 360f * warn_threat_porcentage / 2 - 180f * warning_start_porcentage;
	    warning_circle_right.visible = show_hud;

	    //Danger Level Indicator
	    danger_level_indicator.Update();
	    danger_level_indicator.is_visible = show_hud && threat_porcentage > warning_cutoff_porcentage;
	    danger_level_indicator.is_in_danger = true; 
	    danger_level_indicator.pos = pos + Vector2.up * indicator_radius;

	    //Warninig Level Indicator
	    warning_level_indicator.Update();
	    warning_level_indicator.is_visible = show_hud && threat_porcentage > 0.025f;
	    warning_level_indicator.is_in_danger = false;
	    warning_level_indicator.pos = pos + Vector2.down * indicator_radius;
	}

	public override void Draw(float timeStacker){
	    danger_circle_left.Draw(timeStacker); 
	    danger_circle_right.Draw(timeStacker); 
	    warning_circle_left.Draw(timeStacker);
	    warning_circle_right.Draw(timeStacker);
	    danger_level_indicator.Draw(timeStacker);
	    warning_level_indicator.Draw(timeStacker);
	}

	private float getThreatLevel(){
	    if (hudPlayer.abstractCreature.world.game.GameOverModeActive) return 0f;
			
	    if (hudPlayer.abstractCreature.world.game.manager.musicPlayer != null)
		return hudPlayer.abstractCreature.world.game.manager.musicPlayer.threatTracker.currentMusicAgnosticThreat;
		
	    if (hudPlayer.abstractCreature.world.game.manager.fallbackThreatDetermination == null)
		hudPlayer.abstractCreature.world.game.manager.fallbackThreatDetermination = new ThreatDetermination(0);

	    return hudPlayer.abstractCreature.world.game.manager.fallbackThreatDetermination.currentMusicAgnosticThreat;		
	}
    }
}
