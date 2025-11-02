using System;

namespace XRL.World.Parts.Mutation
{
  [Serializable]
  public class Gearlink_FORGETTABLE_ObjectImpermanence : BaseMutation
  {
    public bool observed;
    public Gearlink_FORGETTABLE_ObjectImpermanence()
    {
      base.Type = "Mental";
      this.observed = true;
    }

    public override bool CanLevel() => false;

    public override string GetDescription() => "You are easily forgotten.";

    public override string GetLevelText(int level) => GetDescription();
    
    public override bool WantEvent(int ID, int cascade)
    {
      if (!base.WantEvent(ID, cascade))
        return ID == BeforeApplyDamageEvent.ID;
      return true;
    }

    public override bool HandleEvent(BeforeApplyDamageEvent E)
    {
      if (E.Actor.IsPlayer())
        this.observed = true;
      return base.HandleEvent(E);
    }
    public override void Register(GameObject Object, IEventRegistrar Registrar)
    {
      Registrar.Register("BeforeTakeAction");
      Registrar.Register("CustomRender");
      base.Register(Object, Registrar);
    }

    public override bool FireEvent(Event E)
    {
      if (E.ID == "BeforeTakeAction")
      {
        if (this.observed && !The.Player.HasLOSTo(this.ParentObject))
          this.observed = false;
      }
      else if (E.ID == "CustomRender")
      { 
        Cell cell = this.ParentObject.CurrentCell;
        if (cell != null)
        {
          LightLevel light = cell.GetLight();
          if (observed || light == LightLevel.LitRadar || light == LightLevel.Radar || light == LightLevel.Interpolight || light == LightLevel.Omniscient)
            this.ParentObject.Render.Visible = true;
          else
            this.ParentObject.Render.Visible = false;
        }
      }
      return base.FireEvent(E);
    }
  }
}