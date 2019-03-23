using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avalanche : Spell
{
    public AvalancheConfig Config;

    public override void Cast(Transform source, Vector3 direction)
    {
        var aoe = Instantiate(Config.AOEEffect, direction, Quaternion.identity);
        var aoeScript = aoe.GetComponent<AOEEffect>();
        aoeScript.Initialize(Config.ZoneExpandSpeed, Config.CircleSize, Config.MaxZoneSize, Config.FadeSpeed);

        var rockPosition = new Vector3(direction.x, direction.y + Config.FallDistance);
        var rock = Instantiate(Config.FallingRock, rockPosition, Quaternion.identity);
        var rockScript = rock.GetComponent<FallingRock>();
        rockScript.Initialize(rockPosition, Config.PushForce, Config.PushTime, Config.Damage, Config.FallDistance, Config.TimeBeforeFall, Config.FadeSpeed);
        Destroy(this.gameObject);
    }

    public override SpellConfig GetConfig()
    {
        return Config;
    }
}
