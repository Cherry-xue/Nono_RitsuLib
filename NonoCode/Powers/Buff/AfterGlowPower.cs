using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace Nono.NonoCode.Powers;

// 余晖-计数器
public sealed class AfterGlowPower : NonoPower
{
    public override PowerType Type => PowerType.Buff;
    //定义能力类型：增益
    public override PowerStackType StackType => PowerStackType.Counter;
    //定义叠加类型：计数器
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => 
    [
        HoverTipFactory.FromPower<BurnPower>(),
        HoverTipFactory.FromPower<HaloPower>()
    ];
    //显示燃烧和光晕的相关信息
}