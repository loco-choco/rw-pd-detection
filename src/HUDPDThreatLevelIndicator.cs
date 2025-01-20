using HUD;
using MoreSlugcats;
using RWCustom;
using UnityEngine;
using System;
namespace PDDetection {
    public class HUDPDThreatLevelIndicator {

        public FAtlasElement warning_element;
        public FAtlasElement danger_element;
	public FSprite sprite;

	public bool display_direction = false;
        public FAtlasElement warning_dir_element;
        public FAtlasElement danger_dir_element;
	public FSprite direction_sprite;
	public float dir_sprite_distance = 25f;

	public Vector2 pos;
	public Vector2 last_pos;
	public float dir;
	public float last_dir;

	public bool is_in_danger = false;
	public bool is_visible = true;

	public HUDPDThreatLevelIndicator(string warning_element_name, string danger_element_name, FContainer container){
	    sprite = new FSprite("Futile_White");
	    container.AddChild(sprite);

	    direction_sprite = new FSprite("Futile_White");
	    container.AddChild(direction_sprite);

	    warning_element = Futile.atlasManager.GetElementWithName(warning_element_name);
	    danger_element = Futile.atlasManager.GetElementWithName(danger_element_name);
	    warning_dir_element = Futile.atlasManager.GetElementWithName(warning_element_name + "-pointer");
	    danger_dir_element = Futile.atlasManager.GetElementWithName(danger_element_name + "-pointer");
	}

	public void Update(){
	    last_pos = pos;
	}

	public void Draw(float timeStacker){
	    Vector2 draw_pos = Vector2.Lerp(last_pos, pos, timeStacker);
	    sprite.isVisible = is_visible;
	    sprite.x = draw_pos.x;
	    sprite.y = draw_pos.y;
	    sprite.element = is_in_danger ? danger_element : warning_element;

	    float draw_dir = Mathf.Lerp(last_dir, dir, timeStacker);
	    direction_sprite.isVisible = is_visible && display_direction;
	    Vector2 dir_sprite_pos = draw_pos + dir_sprite_distance * Custom.DegToVec(dir);
	    direction_sprite.x = dir_sprite_pos.x;
	    direction_sprite.y = dir_sprite_pos.y;
	    direction_sprite.rotation = dir;
	    direction_sprite.element = is_in_danger ? danger_dir_element : warning_dir_element;
	}

	public void ClearSprite(){
	  sprite.RemoveFromContainer();
	  direction_sprite.RemoveFromContainer();
	}
    }
}
