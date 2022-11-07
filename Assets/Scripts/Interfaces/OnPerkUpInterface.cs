using System;
using Assets.Scripts.Types;

public interface OnPerkUpInterface
{
    public event Action<PerkType> OnPerkUp;
}
